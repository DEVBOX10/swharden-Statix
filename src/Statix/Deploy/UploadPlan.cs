using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Statix.Deploy
{
    public class UploadPlan
    {
        private List<SyncFile> RemoteFiles = new List<SyncFile>();
        private List<UploadAction> UploadActions = new List<UploadAction>();

        public UploadPlan()
        {

        }

        public SyncFile[] GetUpdatedFiles()
        {
            List<SyncFile> newFiles = new List<SyncFile>();
            foreach (UploadAction action in UploadActions)
            {
                switch (action.SyncAction)
                {
                    case SyncAction.Skip:
                        SyncFile skippedFile = GetRemoteFile(RemoteFiles, action.LocalFile);
                        newFiles.Add(skippedFile);
                        break;

                    case SyncAction.Create:
                    case SyncAction.Replace:
                        SyncFile uploadedFile = SyncFile.FromRemoteFile(
                            remotePath: action.LocalFile.RemotePath,
                            hash: action.LocalFile.Hash,
                            size: action.LocalFile.Size,
                            date: SyncFile.ToIso8601(DateTime.Now));
                        newFiles.Add(uploadedFile);
                        break;

                    default:
                        throw new InvalidOperationException($"unsupported sync action: {action.SyncAction}");
                }
            }

            return newFiles.ToArray();
        }

        /// <summary>
        /// Clear all information known about remote files
        /// </summary>
        public void RemoteClear()
        {
            RemoteFiles.Clear();
        }

        /// <summary>
        /// Update information about remote files from a JSON file created after a previous sync
        /// </summary>
        public void AddKnownJson(string json)
        {
            SyncFile[] knownFiles = SyncJson.FromJson(json);
            RemoteFiles.AddRange(knownFiles);
            Console.WriteLine($"Loaded information about {knownFiles.Length} remote files");
        }

        /// <summary>
        /// Return an array of actions to perform when it's time to execute the upload
        /// </summary>
        public UploadAction[] GetActions() => UploadActions.ToArray();

        public void AddKnownFile(string remotePath, string hash, int size, string date)
        {
            SyncFile remote = SyncFile.FromRemoteFile(remotePath, hash, size, date);
            RemoteFiles.Add(remote);
        }

        /// <summary>
        /// Upload a file if one with the same hash is not already present
        /// </summary>
        public SyncAction AddFile(string localFilePath, string remotePath)
        {
            if (!File.Exists(localFilePath))
                throw new FileNotFoundException(localFilePath);

            SyncFile local = SyncFile.FromLocalFile(localFilePath, remotePath);
            SyncFile remote = GetRemoteFile(RemoteFiles, local);

            SyncAction uploadAction = SyncAction.Create;
            if (remote != null)
                uploadAction = (local.Hash == remote.Hash) ? SyncAction.Skip : SyncAction.Replace;

            var actn = new UploadAction(local, uploadAction);
            UploadActions.Add(actn);
            return actn.SyncAction;
        }

        /// <summary>
        /// Recurisvely upload all files in a folder (skipping existing files with the same hash)
        /// </summary>
        public void AddFolder(string localFolder, string remoteFolder)
        {
            if (!Directory.Exists(localFolder))
                throw new DirectoryNotFoundException(localFolder);

            localFolder = Path.GetFullPath(localFolder).Replace("\\", "/");
            string[] localFiles = Directory.GetFiles(localFolder, "*", SearchOption.AllDirectories);
            foreach (string localFile in localFiles)
            {
                string remoteFile = localFile.Replace(localFolder, remoteFolder).Replace("\\", "/");
                AddFile(localFile, remoteFile);
            }
        }

        /// <summary>
        /// Return the remote file with the same remote path as the local file
        /// </summary>
        public static SyncFile GetRemoteFile(List<SyncFile> syncFiles, SyncFile localFile)
        {
            SyncFile[] files = syncFiles.Where(x => x.RemotePath == localFile.RemotePath).ToArray();

            if (files.Length > 1)
                throw new InvalidOperationException("more than 1 file with the same remote path");

            return files.Length > 0 ? files[0] : null;
        }
    }
}

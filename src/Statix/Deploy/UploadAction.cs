namespace Statix.Deploy
{
    public class UploadAction
    {
        public readonly SyncFile LocalFile;
        public readonly SyncAction SyncAction;

        public UploadAction(SyncFile localFile, SyncAction action)
        {
            LocalFile = localFile;
            SyncAction = action;
        }

        public override string ToString()
        {
            return $"{SyncAction} {LocalFile.LocalPath} {LocalFile.RemotePath}";
        }
    }
}
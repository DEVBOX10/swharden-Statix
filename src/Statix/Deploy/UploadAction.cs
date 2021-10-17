using System;
using System.Collections.Generic;
using System.Text;

namespace Statix.Deploy
{
    public class UploadAction
    {
        public readonly string LocalPath;
        public readonly string RemotePath;
        public readonly SyncAction Action;

        public UploadAction(SyncFile localFile, SyncAction action)
        {
            LocalPath = localFile.LocalPath;
            RemotePath = localFile.RemotePath;
            Action = action;
        }

        public override string ToString()
        {
            return $"{Action} {LocalPath} {RemotePath}";
        }
    }
}

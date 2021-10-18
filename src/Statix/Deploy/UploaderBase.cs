using System;

namespace Statix.Deploy
{
    public abstract class UploaderBase : IDisposable
    {
        public void RemoteDeploy(Plan plan)
        {
            foreach (TrackedFile file in plan.GetTrackedFiles())
            {
                if (!file.ExistsLocally)
                    RemoteDelete(file);
                else if (!file.ExistsRemotely)
                    RemoteCreate(file);
                else if (file.LocalHash != file.RemoteHash)
                    RemoteReplace(file);
                else
                    RemoteSkip(file);
            }
        }

        protected abstract void RemoteCreate(TrackedFile file);
        protected abstract void RemoteReplace(TrackedFile file);
        protected abstract void RemoteDelete(TrackedFile file);
        protected abstract void RemoteSkip(TrackedFile file);
        public abstract void Dispose();
    }
}

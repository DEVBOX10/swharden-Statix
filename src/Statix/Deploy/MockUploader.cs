using System;
using System.Collections.Generic;
using System.Text;

namespace Statix.Deploy
{
    public class MockUploader : UploaderBase
    {
        public MockUploader()
        {
            Console.WriteLine("Mock Uploader: Connected");
        }

        protected override void RemoteCreate(TrackedFile file)
        {
            Console.WriteLine($"Creating: {file.RemotePath}");
            file.HasBeenUploaded();
        }

        protected override void RemoteReplace(TrackedFile file)
        {
            Console.WriteLine($"Replacing: {file.RemotePath}");
            file.HasBeenUploaded();
        }

        protected override void RemoteDelete(TrackedFile file)
        {
            Console.WriteLine($"Deleting: {file.RemotePath}");
            file.HasBeenDeleted();
        }

        protected override void RemoteSkip(TrackedFile file)
        {
            Console.WriteLine($"Unchanged: {file.RemotePath}");
        }

        public override void Dispose()
        {

        }
    }
}

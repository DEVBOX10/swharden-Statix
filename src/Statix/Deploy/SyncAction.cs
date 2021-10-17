namespace Statix.Deploy
{
    public enum SyncAction
    {
        /// <summary>
        /// Upload the file which does not currently exist at the remote path
        /// </summary>
        Create,

        /// <summary>
        /// Replace the existing remote file with one that has a different hash
        /// </summary>
        Replace,

        /// <summary>
        /// Skip the file because one with the same hash already exists
        /// </summary>
        Skip,
    }
}

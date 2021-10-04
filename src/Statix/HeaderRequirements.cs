using System.Collections.Generic;

namespace Statix
{
    /// <summary>
    /// Defines expectations for headers and performs checks to ensure important components exist.
    /// </summary>
    public class HeaderRequirements
    {
        public bool RequireHeader = true;
        public bool RequireTitle = true;
        public bool RequireDescription = true;
        public bool RequireDate = true;

        public string[] GetMissing(Header header)
        {
            if (!header.HasHeader)
                return new string[] { "header" };

            List<string> missing = new List<string>();

            if (RequireTitle && string.IsNullOrWhiteSpace(header.Title))
                missing.Add("title");

            if (RequireDescription && string.IsNullOrWhiteSpace(header.Description))
                missing.Add("description");

            if (RequireDate && string.IsNullOrWhiteSpace(header.Date))
                missing.Add("date");

            return missing.ToArray();
        }
    }
}
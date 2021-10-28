using System;

namespace Statix.Sitemap
{
    public class Url
    {
        public string Location;
        public DateTime Modified = DateTime.MinValue;
        public ChangeFreq ChangeFreq = ChangeFreq.Unknown;
        public double Priority = double.NaN;
    }
}
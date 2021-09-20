using System;
using System.Collections.Generic;
using System.Text;

namespace Statix.Plugin
{
    public interface IHtmlPlugin
    {
        string[] Apply(string[] lines);
    }
}

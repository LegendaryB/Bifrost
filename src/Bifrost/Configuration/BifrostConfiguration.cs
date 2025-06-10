using System.Collections.Generic;

namespace Bifrost.Configuration
{
    internal class BifrostConfiguration
    {
        public Dictionary<string, string> Servers { get; set; } = new Dictionary<string, string>();
    }
}

using System.Collections.Generic;

namespace Bifrost.Configuration
{
    internal class BifrostConfiguration
    {
        public bool AutoSave { get; set; }
        public Dictionary<string, string> Items { get; set; } = new Dictionary<string, string>();
    }
}

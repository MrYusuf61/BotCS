using PluginCS.Databases;
using PluginCS.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotCS.Utils
{
    internal class ClientPlugin
    {
        public string PluginID { get; set; }
        public IPlugin Plugin { get; set; }
        public string PluginDllName { get; set; }
        public List<string> Events { get; set; } = new List<string>();

    }
}

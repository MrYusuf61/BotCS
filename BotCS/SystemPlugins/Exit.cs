using DSharpPlus;
using PluginCS.Plugins;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotCS.SystemPlugins
{
    public class Exit : IConsolePlugin
    {
        public string Name => "System Exit Command";

        public string AuthorName => "BotCS";

        public double Version => 1.0;

        public string Description => "";

        public bool ClientEnabled => false;

        public List<string> Aliases => new() { "exit", "close" };

        public void OnCalled(string[] args)
        {
            Environment.Exit(0);
        }

        public void OnLoad(DiscordClient client) { }
    }
}

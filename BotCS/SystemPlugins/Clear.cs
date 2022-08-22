using DSharpPlus;
using PluginCS;
using PluginCS.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotCS.SystemPlugins
{
    public class Clear : IConsolePlugin
    {
        public string Name => "System Clear Command";

        public string AuthorName => "BotCS";

        public double Version => 1.0;

        public string Description => "It is used to clear the text on the console.";

        public bool ClientEnabled => false;

        public List<string> Aliases => new() { "clear", "cls" };

        public void OnCalled(string[] args)
        {
            Logger.Clear();
        }

        public void OnLoad(DiscordClient client) { }
    }
}

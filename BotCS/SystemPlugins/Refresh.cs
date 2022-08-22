using BotCS.Discord;
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
    public class Refresh : IConsolePlugin
    {
        public string Name => "System Refresh Command";

        public string AuthorName => "BotCS";

        public double Version => 1.0;

        public string Description => "Restart the client and reloads the plugins.";

        public bool ClientEnabled => false;

        public List<string> Aliases => new() { "refresh" , "restart_client" };

        public void OnCalled(string[] args)
        {
            try
            {
                Client.Refresh();
                Logger.WriteLine("{green}Client refresh finished.");
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            
        }

        public void OnLoad(DiscordClient client) { }
    }
}

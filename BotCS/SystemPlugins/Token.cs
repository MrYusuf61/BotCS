using DSharpPlus;
using PluginCS;
using PluginCS.Databases;
using PluginCS.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotCS.SystemPlugins
{
    public class Token : IConsolePlugin
    {
        public string Name => "System Token Command";

        public string AuthorName => "BotCS";

        public double Version => 1.0;

        public bool ClientEnabled => false;

        public List<string> Aliases => new() { "token" };

        public string Description => "Used to exchange Discord bot token.";

        public void OnCalled(string[] args)
        {
            if (args.Length == 0)
                Logger.WriteLine("Please type a Token");
            else
            {
                if (JsonDatabase.Set("token", args[0]))
                    Logger.WriteLine("{green}The token has been successfully exchanged.");
                else
                    Logger.WriteLine("{yellow}There were a few mishaps, can you try again?");
            }
        }

        public void OnLoad(DiscordClient client) 
        {
            if (!JsonDatabase.Has("token"))
                JsonDatabase.Set("token", "");
        }
    }
}

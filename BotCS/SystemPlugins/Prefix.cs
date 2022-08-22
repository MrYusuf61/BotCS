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
    public class Prefix : IConsolePlugin
    {
        public string Name => "System Prefix Command";

        public string AuthorName => "BotCS";

        public double Version => 1.0;

        public string Description => "It is used to change the prefix of the bot.";

        public bool ClientEnabled => false;

        public List<string> Aliases => new() { "prefix" };

        public void OnCalled(string[] args)
        {
            string Out;
            if (args.Length != 0 && !string.IsNullOrEmpty(args[0]))
            {
                JsonDatabase.Set("prefix", args[0]);
                Out = "{green}Your transaction has been completed successfully{end}.";
            }
            else
            {
                Out = "{blue}You must enter a Prefix{end}.";
            }
            Logger.WriteLine(Out);
        }

        public void OnLoad(DiscordClient client) 
        {
            if (!JsonDatabase.Has("prefix"))
                JsonDatabase.Set("prefix", "");
        }
    }
}

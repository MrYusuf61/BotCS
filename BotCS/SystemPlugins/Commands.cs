using BotCS.Utils;
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
    public class Commands : IConsolePlugin
    {
        public string Name => "System Commands Command";

        public string AuthorName => "BotCS";

        public double Version => 1.0;

        public string Description => "It shows the commands installed in the bot and can also \"enable\" and \"disable\" it.";

        public bool ClientEnabled => true;

        public List<string> Aliases => new() { "commands", "client_commands" };

        private static Dictionary<string, string> commands { get; } = new() { { "enable", "Used to activate a command. Usage pattern {yellow2}\"commands enable ID\"{end}." }, { "disable", "Used to deactivate a command. Usage pattern {yellow2}\"commands disable ID\"{end}." } };

        public void OnCalled(string[] args)
        {
            string Out = "";
            if (args.Length == 0)
            {
                Out = "{blue}Currently installed commands{end}. {yellow2}For more details please type the {yellow}\"{yellow2}commands help{yellow}\"{end}.\n";
                Out += string.Join("\n", PluginLoader.Commands.Select(cmd => $"\t{getCommandInfo(cmd)}"));
            }
            else
            {
                if (commands.TryGetValue(args[0].ToLower(), out string _))
                {
                    var cmd = args[0].ToLower();
                    args = args.Skip(1).ToArray();
                    if (args.Length == 0)
                    {
                        WriteParamError();
                        return;
                    }

                    if (cmd == "enable" || cmd == "disable")
                    {
                        if (changeCommandStat((cmd == "enable"), args[0]))
                            Out = "{green}Your actions have been successfully applied{end}.";
                        else
                            Out = "{red}No command found with the ID you typed{end}.";
                    }
                }
                else
                {
                    if (args[0].ToLower() == "help")
                    {
                        Out = "{blue}Parameters of the commands command that can be used.{end}\n";
                        foreach (var Key in commands.Keys)
                        {
                            Out += $"{{cyan}}{Key}{{end}} : {commands[Key]}\n";
                        }
                    }
                    else WriteParamError();
                }
            }
            Logger.WriteLine(Out);
        }

        public void OnLoad(DiscordClient client)
        {
            if (!JsonDatabase.Has("DISABLED"))
                JsonDatabase.Set("DISABLED", new List<object>());
        }

        private static string getCommandInfo(ClientCommand cmd)
        {
            string commandName = "{blue}Unknown{end}";
            string commandAuthorName = "{blue}Unknown{end}";
            string commandVersion = "{blue}Unknown{end}";
            string commandID = $"{{yellow2}}{Helper.GuidToID(cmd.CommandID)}{{end}}";

            try
            {
                commandName = $"{{green}}{cmd.Command.Name}{{end}}";
            }
            catch (NotImplementedException) { }
            try
            {
                commandAuthorName = $"{cmd.Command.AuthorName}";
            }
            catch (NotImplementedException) { }
            try
            {
                commandVersion = $"{{cyan}}{cmd.Command.Version:0.0000}{{end}}";
            }
            catch (NotImplementedException) { }

            return $"({commandID}){commandName} {{cyan}}V{{end}}{commandVersion}, Developed by {{yellow2}}{commandAuthorName}{{end}} ({(cmd.IsEnabled() ? "{green}Enable{end}" : "{red}Disable{end}")})";
        }

        private static void WriteParamError() => Logger.WriteLine("Parameters are missing. You can type {yellow2}\"commands help\"{end} to get information about the parameters.");
        
        private static bool changeCommandStat(bool enable, string id)
        {
            foreach (var item in PluginLoader.Commands)
            {
                if (Helper.GuidToID(item.CommandID) == id)
                {
                    if (enable != item.IsEnabled())
                    {
                        var listID = "DISABLED";
                        var disabledList = JsonDatabase.GetList(listID);
                        
                        if (!enable) disabledList.Add(id);
                        else disabledList.Remove(id);

                        JsonDatabase.Set(listID, disabledList);
                        return true;
                    }
                    else return true;
                }
            }
            return false;
        }
    }
}

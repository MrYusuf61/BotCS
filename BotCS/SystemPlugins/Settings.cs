using BotCS.Discord;
using BotCS.Utils;
using DSharpPlus;
using DSharpPlus.Entities;
using PluginCS;
using PluginCS.Databases;
using PluginCS.Objects;
using PluginCS.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BotCS.SystemPlugins
{
    public class Settings : IConsolePlugin
    {
        public string Name => "System Settings Command";

        public string AuthorName => "BotCS";

        public double Version => 1.0;

        public string Description => "Command used to change certain settings of the bot.";

        public bool ClientEnabled => true;

        public List<string> Aliases => new() { "settings" };

        private readonly Dictionary<string, string> commands = new()
        {
            { "drp", "Used to set discord rich presence." },
            { "client", "Used to change client settings." },
            { "prefix", "It is used to set bot prefixes." },
            { "owners", "Sets the bot's owners." },
        };


        #region Drp Settings
        private readonly Dictionary<string, string> drpAliases = new()
        {
            { "status", "It is used to set the status of the bot.\n        Values:\n        " + Helper.GetEnumValues(typeof(UserStatus),".\n        ") },
            { "activity-name", "Activity name." },
            { "activity-type", "Used to change the activity type.\n        Values:\n        " + Helper.GetEnumValues(typeof(ActivityType),".\n        ") },
            { "stream-url", "stream url." },
        };

        private static void DrpMenu(MenuResult res)
        {
            if (res.Command == "status")
            {
                if (res.Args.Count < 1)
                {
                    if (int.TryParse(res.Args[0], out int value))
                    {
                        string? name = Enum.GetName(typeof(UserStatus), value);
                        if (string.IsNullOrEmpty(name)) Logger.WriteLine("{red}Wrong parameter{end}\nAvailable values:\n        " + Helper.GetEnumValues(typeof(UserStatus), ".\n        "));
                        else
                        {
                            Client.SaveUserStatus((UserStatus)value);
                            goto SUC_SAVE;
                        }
                    }
                    else Logger.WriteLine("{red}Wrong parameter{end}\nAvailable values:\n        " + Helper.GetEnumValues(typeof(UserStatus), ".\n        "));
                }
                else goto WRITE_HELP;
            }
            else if (res.Command == "activity-type")
            {
                if (res.Args.Count < 1)
                {
                    if (int.TryParse(res.Args[0], out int value))
                    {
                        string? name = Enum.GetName(typeof(ActivityType), value);
                        if (string.IsNullOrEmpty(name)) Logger.WriteLine("{red}Wrong parameter{end}\nAvailable values:\n        " + Helper.GetEnumValues(typeof(ActivityType), ".\n        "));
                        else
                        {
                            Client.SaveActivityType((ActivityType)value);
                            goto SUC_SAVE;
                        }
                    }
                    else Logger.WriteLine("{red}Wrong parameter{end}\nAvailable values:\n        " + Helper.GetEnumValues(typeof(ActivityType), ".\n        "));
                }
                else goto WRITE_HELP;
            }
            else if (res.Command == "activity-name")
            {
                if (res.Args.Count < 1)
                {
                    Client.SaveActivityName(string.Join(" ", res.Args));
                    goto SUC_SAVE;
                }
                else goto WRITE_HELP;
            }
            else if (res.Command == "stream-url")
            {
                if (res.Args.Count < 1)
                {
                    Client.SaveActivityName(string.Join(" ", res.Args));
                    goto SUC_SAVE;
                }
                else goto WRITE_HELP;
            }

            return;

        WRITE_HELP:
            Logger.WriteLine("{red}Please provide a parameter{end}.{blue}You can type {yellow}\"help\"{blue} to get help{end}.");
            return;

        SUC_SAVE:
            Logger.WriteLine("{green}Successfully saved{end}.");
        }
        #endregion

        #region Prefix Settings
        private readonly Dictionary<string, string> prefixAliases = new()
        {
            { "set", "It is used to change the global prefix." },
            { "get", "Used to see specific prefix." },
            { "set-guild", "Used to change the guild specific prefix." },
            { "get-guild", "Used to see a guild-specific prefix." },
        };

        private void prefixMenu(MenuResult res)
        {
            if (res.Command == "set")
            {
                if (res.Args.Count > 0)
                {
                    JsonDatabase.Set("prefix", res.Args[0]);
                    Logger.WriteLine("{green}Successfully saved{end}.");
                }
                else goto WRITE_HELP;
            }
            else if (res.Command == "get")
            {
                Logger.WriteLine("{yellow2}" + (JsonDatabase.GetString("prefix") ?? "{blue}Unknown") + "{end}");
            }
            else if (res.Command == "set-guild")
            {
                if (res.Args.Count > 1)
                {
                    JsonDatabase.Set("prefix." + res.Args[0], res.Args[1]);
                    Logger.WriteLine("{green}Successfully saved{end}.");
                }
                else goto WRITE_HELP;
            }
            else if (res.Command == "get-guild")
            {
                if (res.Args.Count > 0)
                {
                    Logger.WriteLine("{yellow2}" + JsonDatabase.GetString("prefix." + res.Args[0]));
                }
                else goto WRITE_HELP;
            }

            return;
        WRITE_HELP:
            Logger.WriteLine("{red}Please provide a parameter{end}.{blue}You can type {yellow}\"help\"{blue} to get help{end}.");
        }
        #endregion

        #region Owners Settings
        private readonly Dictionary<string, string> ownersAliases = new()
        {
            { "get", "Used to see Bot owners." },
            { "add", "It is used to add owner to the bot." },
            { "remove", "It is used to remove owner to the bot." },
        };

        private void ownersMenu(MenuResult res)
        {
            if (res.Command == "get")
            {
                var tempList = Bot.Owners;
                if (tempList.Count != 0) Logger.WriteLine($"[ {{yellow2}}\"{string.Join("\"{end}, {yellow2}\"", tempList)}\"{{end}} ]");
                else Logger.WriteLine("[ ]");
            }
            else if (res.Command == "add")
            {
                if (res.Args.Count > 0 && !string.IsNullOrEmpty(res.Args[0]))
                {
                    var tempList = Bot.Owners;
                    if (!tempList.Contains(res.Args[0]))
                    {
                        tempList.Add(res.Args[0]);
                        JsonDatabase.Set("owners", tempList);
                        Logger.WriteLine("{green}Successfully saved{end}.");
                    }
                    else Logger.WriteLine("{yellow2}The id written is already set as owner{end}.");
                }
                else goto WRITE_HELP;
            }
            else if (res.Command == "remove")
            {
                if (res.Args.Count > 0)
                {
                    var tempList = Bot.Owners;
                    if (tempList.Contains(res.Args[0]))
                    {
                        tempList.Remove(res.Args[0]);
                        JsonDatabase.Set("owners", tempList);
                        Logger.WriteLine("{green}Successfully saved{end}.");
                    }
                    else Logger.WriteLine("{yellow2}The typed ID could not be found{end}.");
                }
                else goto WRITE_HELP;
            }

            return;
        WRITE_HELP:
            Logger.WriteLine("{red}Please provide a parameter{end}.{blue}You can type {yellow}\"help\"{blue} to get help{end}.");
        }
        #endregion

        #region Client Settings
        private readonly Dictionary<string, string> clientAliases = new()
        {
            { "auto-reconnect", "Automatic reconnection setting.(True or False)" },
            { "always-cache-members", "Sets whether the client should attempt to cache members if exclusively using unprivileged intents.(True or False)" },
            { "gateway-compression-level", "Sets the level of compression for WebSocket traffic.\n        Values:\n        " + Helper.GetEnumValues(typeof(GatewayCompressionLevel),".\n        ") },
            { "intents", "Sets the gateway intents for this client. If set, the client will only receive events that they specify with intents.\n        Values:\n        " + Helper.GetEnumValues(typeof(DiscordIntents),".\n        ") },
            { "message-cache-size", "Sets the size of the global message cache.Setting this to 0 will disable message caching entirely. Defaults to 1024." },
            { "name", "Renames the bot. {red}CANNOT BE RETURNED{end}." },
        };

        private void clientMenu(MenuResult res)
        {
            if (res.Command == "auto-reconnect")
            {
                if (res.Args.Count > 0 && !string.IsNullOrEmpty(res.Args[0]))
                {
                    if (bool.TryParse(res.Args[0], out bool value))
                    {
                        JsonDatabase.Set("auto-reconnect", value);
                        goto SUC_SAVE;
                    }
                    else goto WRITE_HELP;
                }
                else goto WRITE_HELP;
            }
            else if (res.Command == "always-cache-members")
            {
                if (res.Args.Count > 0 && !string.IsNullOrEmpty(res.Args[0]))
                {
                    if (bool.TryParse(res.Args[0], out bool value))
                    {
                        JsonDatabase.Set("always-cache-members", value);
                        goto SUC_SAVE;
                    }
                    else goto WRITE_HELP;
                }
                else goto WRITE_HELP;
            }
            else if (res.Command == "gateway-compression-level")
            {
                if (res.Args.Count < 1)
                {
                    if (byte.TryParse(res.Args[0], out byte value))
                    {
                        string? name = Enum.GetName(typeof(GatewayCompressionLevel), value);
                        if (string.IsNullOrEmpty(name)) Logger.WriteLine("{red}Wrong parameter{end}\nAvailable values:\n        " + Helper.GetEnumValues(typeof(GatewayCompressionLevel), ".\n        "));
                        else
                        {
                            JsonDatabase.Set("gateway-compression-level", (int)value);
                            goto SUC_SAVE;
                        }
                    }
                    else Logger.WriteLine("{red}Wrong parameter{end}\nAvailable values:\n        " + Helper.GetEnumValues(typeof(GatewayCompressionLevel), ".\n        "));
                }
                else goto WRITE_HELP;
            }
            else if (res.Command == "intents")
            {
                if (res.Args.Count < 1)
                {
                    if (int.TryParse(res.Args[0], out int value))
                    {
                        string? name = Enum.GetName(typeof(DiscordIntents), value);
                        if (string.IsNullOrEmpty(name)) Logger.WriteLine("{red}Wrong parameter{end}\nAvailable values:\n        " + Helper.GetEnumValues(typeof(DiscordIntents), ".\n        "));
                        else
                        {
                            JsonDatabase.Set("intents", value);
                            goto SUC_SAVE;
                        }
                    }
                    else Logger.WriteLine("{red}Wrong parameter{end}\nAvailable values:\n        " + Helper.GetEnumValues(typeof(DiscordIntents), ".\n        "));
                }
                else goto WRITE_HELP;
            }
            else if (res.Command == "message-cache-size")
            {
                if (res.Args.Count > 0 && !string.IsNullOrEmpty(res.Args[0]))
                {
                    if (int.TryParse(res.Args[0], out int value))
                    {
                        JsonDatabase.Set("message-cache-size", value);
                        goto SUC_SAVE;
                    }
                    else goto WRITE_HELP;
                }
                else goto WRITE_HELP;
            }
            else if (res.Command == "name")
            {
                if (!string.IsNullOrEmpty(res.Args[0]))
                {
                    var result = Logger.ReadLine("Are you sure you want to rename the bot ? (Yes/No)");
                    if (result.ToLower() == "yes")
                    {
                        result = Logger.ReadLine($"The bot name will be changed from \"{client.CurrentUser.Username}#{client.CurrentUser.Discriminator}\" to \"{string.Join(" ", res.Args)}#????\" ?(Yes/No)");
                        if (result.ToLower() == "yes")
                        {
                            var task = client.UpdateCurrentUserAsync(string.Join(" ", res.Args));
                            task.Wait();
                            Thread.Sleep(1000);
                            Logger.WriteLine($"{{green}}Successfully saved{{end}}. Bot Name = \"{client.CurrentUser.Username}#{client.CurrentUser.Discriminator}\"");
                        }
                        else return;
                    }
                    else return;
                }
                else goto WRITE_HELP;
            }

            return;
        WRITE_HELP:
            Logger.WriteLine("{red}Please provide a parameter{end}.{blue}You can type {yellow}\"help\"{blue} to get help{end}.");
            return;
        SUC_SAVE:
            Logger.WriteLine("{green}Successfully saved{end}.");
        }
        #endregion

        public void OnCalled(string[] args)
        {
            if (args.Length != 0)
            {
                string cmd = args[0];
                args = args.Skip(1).ToArray();
                if (commands.TryGetValue(cmd.ToLower(), out var value))
                {
                    if (cmd.ToLower() == "drp")
                    {
                        Logger.StartMenu(drpAliases, DrpMenu, "{green}You have entered the drp menu{end}.{yellow2} You can use the {blue}exit{yellow2} command to exit from here.");
                        return;
                    }
                    else if (cmd.ToLower() == "prefix")
                    {
                        Logger.StartMenu(prefixAliases, prefixMenu, "{green}You have entered the prefix menu{end}.{yellow2} You can use the {blue}exit{yellow2} command to exit from here.");
                        return;
                    }
                    else if (cmd.ToLower() == "owners")
                    {
                        Logger.StartMenu(ownersAliases, ownersMenu, "{green}You have entered the owners menu{end}.{yellow2} You can use the {blue}exit{yellow2} command to exit from here.");
                        return;
                    }
                    else if (cmd.ToLower() == "client")
                    {
                        Logger.StartMenu(clientAliases, clientMenu, "{green}You have entered the client menu{end}.{yellow2} You can use the {blue}exit{yellow2} command to exit from here.");
                        return;
                    }
                }
                else
                {
                    if (cmd.ToLower() == "help")
                    {
                        string Out = "{blue}Parameters of the settings command that can be used.{end}\n";
                        foreach (var Key in commands.Keys)
                        {
                            Out += $"{{cyan}}{Key}{{end}} : {commands[Key]}\n";
                        }
                        Logger.WriteLine(Out);
                    }
                    else
                    {
                        WriteParamError();
                    }
                }
            }
            else WriteParamError();
        }

        public void OnLoad(DiscordClient client)
        {
            Settings.client = client;
        }

        static DiscordClient client = null;

        private static void WriteParamError() => Logger.WriteLine("Parameters are missing. You can type {yellow2}\"settings help\"{end} to get information about the parameters.");
    }
}

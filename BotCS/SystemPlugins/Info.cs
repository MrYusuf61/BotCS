using DSharpPlus;
using DSharpPlus.Entities;
using PluginCS;
using PluginCS.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BotCS.SystemPlugins
{
    public class Info : IConsolePlugin
    {
        public string Name => "System Info Command";

        public string AuthorName => "BotCS";

        public double Version => 1.0;

        public string Description => "It is used to get various information such as client, guilds.";

        public bool ClientEnabled => true;

        public List<string> Aliases => new() { "info" };

        private static Dictionary<string, string> commands { get; } = new Dictionary<string, string>() { { "client", "Returns information about the client. Usage pattern {yellow2}\"info client\"{end}." }, { "guilds", "Lists the names of the guilds the bot is in. Usage pattern {yellow2}\"info guilds\"{end}." }, { "guild", "It gives detailed information of the guild belonging to the given id. Usage pattern {yellow2}\"info guild [id]\"{end}." }, { "user", "It gives detailed information of the user belonging to the given id. Usage pattern {yellow2}\"info user [id]\"{end}." } };

        public void OnCalled(string[] args)
        {
            if (client != null)
            {
                if (args.Length > 0)
                {
                    if (commands.TryGetValue(args[0].ToLower(), out var _))
                    {
                        string cmd = args[0].ToLower();
                        args = args.Skip(1).ToArray();
                        string Out = "";
                        if (cmd == "client")
                        {
                            ulong memberSize = 0;
                            List<string> owners = new();
                            if (client != null && client.Guilds != null)
                            {
                                memberSize = (ulong)client.Guilds.Values.Sum(item => item.MemberCount - 1);
                            }

                            if (client != null && client.CurrentApplication != null && client.CurrentApplication.Owners != null)
                            {
                                foreach (var item in client.CurrentApplication.Owners)
                                {
                                    owners.Add($"({item.Id}){item.Username}#{item.Discriminator}");
                                }
                            }

                            Out = @$"{{blue}}Information about the client{{end}}.
    {{cyan}}Client Name: {{yellow2}}BotCS
    {{cyan}}Client Version: {{yellow2}}{Program.Version:0.0000}
    {{cyan}}Discord Client Name: {{yellow2}}DSharpPlus
    {{cyan}}Discord Client Version: {{yellow2}}{client.VersionString}
    {{cyan}}Discord Client Gateway Version: {{yellow2}}{client.GatewayVersion}
    {{cyan}}Discord Client Ping: {{yellow2}}{client.Ping}MS
    {{cyan}}Client Ram Usage: {{yellow2}}{Program.GetCurrentRam()}
    {{cyan}}Client Guilds: {{yellow2}}{(client.Guilds != null ? client.Guilds.Count : 0)}
    {{cyan}}Client Members: {{yellow2}}{memberSize}
    {{cyan}}Client ID: {{yellow2}}{(client.CurrentUser != null ? client.CurrentUser.Id : 0)}
    {{cyan}}Client Application Owners: {{yellow2}}{string.Join(", ",owners)}
";
                        }
                        else if (cmd == "guilds")
                        {
                            Out = "{blue}Information about the top 40 guilds{end}.\n";
                            if (client != null && client.Guilds != null)
                            {
                                for (int i = 0; i < (client.Guilds.Count > 40 ? 40 : client.Guilds.Count); i++)
                                {
                                    var guildKey = client.Guilds.Keys.ToList()[i];
                                    var guild = client.Guilds[guildKey];
                                    Out += $"({{cyan}}{guild.Id}{{end}}){{yellow2}}{guild.Name}{{end}} - {{blue}}{guild.MemberCount}{{end}}\n";
                                }
                            }
                        }
                        else if (cmd == "guild")
                        {
                            if (args.Length != 0)
                            {
                                try
                                {
                                    if (ulong.TryParse(args[0], out ulong guildID))
                                    {
                                        var guildTask = client.GetGuildAsync(guildID);
                                        if (guildTask.Status != TaskStatus.WaitingForActivation)
                                        {
                                            guildTask.Wait();
                                            var guild = guildTask.Result;
                                            guildTask.Dispose();

                                            List<string> botRoles = new();
                                            string botPermission = Enum.GetName(typeof(Permissions), guild.CurrentMember.Permissions);
                                            if (guild.CurrentMember.Roles != null)
                                            {
                                                foreach (var role in guild.CurrentMember.Roles)
                                                {
                                                    botRoles.Add($"@{role.Name}");
                                                }
                                            }

                                            Out = @$"{{blue}}Detailed guild information{{end}}.
    {{cyan}}Guild Name: {{yellow2}}{guild.Name}
    {{cyan}}Guild ID: {{yellow2}}{guild.Id}
    {{cyan}}Guild Owner Name: {{yellow2}}{guild.Owner.Username}#{guild.Owner.Discriminator}
    {{cyan}}Guild Owner ID: {{yellow2}}{guild.OwnerId}
    {{cyan}}Guild Created Date: {{yellow2}}{guild.CreationTimestamp.DateTime.ToUniversalTime()}
    {{cyan}}Bot Name In Guild: {{yellow2}}{guild.CurrentMember.DisplayName}
    {{cyan}}Guild Members Count: {{yellow2}}{guild.MemberCount}
    {{cyan}}Guild Roles Count: {{yellow2}}{guild.Roles.Count}
    {{cyan}}Roles Bot Has In Guild: {{yellow2}}{string.Join(", ",botRoles)}
    {{cyan}}Bot's Permission In Guild: {{yellow2}}{botPermission}
";
                                        }
                                        else
                                        {
                                            Out = "{blue}Guild not found{end}.";
                                        }
                                    }
                                    else
                                    {
                                        Out = "{red}The id entered was not properly{end}.";
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Logger.Error(ex);
                                }
                            }
                            else
                            {
                                Out = "{blue}You must enter a guild id{end}. Example usage {yellow2}\"info guild 1234567890\"{end}.";
                            }
                        }
                        else if (cmd == "user")
                        {
                            if (args.Length != 0)
                            {
                                try
                                {
                                    if (ulong.TryParse(args[0], out ulong userID))
                                    {
                                        var userTask = GetUser(userID);

                                        if (userTask.Status != TaskStatus.WaitingForActivation)
                                        {
                                            userTask.Wait();
                                            var user = userTask.Result;
                                            userTask.Dispose();
                                            Out = @$"{{blue}}Detailed user information{{end}}.
    {{cyan}}User Name: {{yellow2}}{user.Username}#{user.Discriminator}
    {{cyan}}User ID: {{yellow2}}{user.Id}
    {{cyan}}User Avatar Url:{{yellow2}}{user.AvatarUrl}
    {{cyan}}User Banner Url: {{yellow2}}{(string.IsNullOrEmpty(user.BannerUrl) ? (user.BannerColor.HasValue? user.BannerColor.ToString(): "Auto Color"): user.BannerUrl)}
    {{cyan}}User Created Date: {{yellow2}}{user.CreationTimestamp.DateTime.ToUniversalTime()}
    {{cyan}}User Is Bot: {{yellow2}}{user.IsBot}
    {{cyan}}User Flags: {{yellow2}}{user.Flags}
    ";
                                        }
                                        else
                                        {
                                            Out = "{blue}User not found{end}.";
                                        }
                                    }
                                    else
                                    {
                                        Out = "{red}The id entered was not properly{end}.";
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Logger.Error(ex);
                                }
                            }
                            else
                            {
                                Out = "{blue}You must enter a user id{end}. Example usage {yellow2}\"info user 1234567890\"{end}.";
                            }
                        }

                        Logger.WriteLine(Out);
                    }
                    else
                    {
                        if (args[0].ToLower() != "help") WriteParamError();
                        else
                        {
                            string Out = "{blue}Parameters of the info command that can be used.{end}\n";
                            foreach (var Key in commands.Keys)
                            {
                                Out += $"{{cyan}}{Key}{{end}} : {commands[Key]}\n";
                            }
                            Logger.WriteLine(Out);
                        }
                    }
                }
                else
                {
                    WriteParamError();
                }
            }
            else
            {
                Logger.WriteLine("This command is currently unavailable.{yellow2}Reason : client not found.");
            }
        }

        public void OnLoad(DiscordClient client)
        {
            this.client = client;
        }

        private Task<DiscordUser> GetUser(ulong id)
        {
            var userTask = client.GetUserAsync(id);
            if (userTask.Status == TaskStatus.WaitingForActivation)
            {
                Thread.Sleep(client.Ping * 4);
                userTask = client.GetUserAsync(id);
            }
            return userTask;
        }

        private static void WriteParamError() => Logger.WriteLine("Parameters are missing. You can type {yellow2}\"info help\"{end} to get information about the parameters.");

        private DiscordClient client = null;
    }
}

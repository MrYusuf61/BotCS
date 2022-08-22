using BotCS.Utils;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using PluginCS;
using PluginCS.Databases;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotCS.Discord
{
    internal static class Client
    {
        private static DiscordClient _client = null;

        [DebuggerHidden]
        public static void Init()
        {
            Refresh();
        }

        public static void Refresh()
        {
            if (_client != null)
            {
                try
                {
                    _client.DisconnectAsync().Wait();
                }
                catch (Exception) { }
                _client.Dispose();
            }

            _client = null;
            if (!JsonDatabase.Has("token"))
            {
                Logger.WriteLine("{cyan}Please enter a token. You can use the \"help\" command for help.");
                return;
            }

            try
            {
                GatewayCompressionLevel gcl = GatewayCompressionLevel.Stream;
                DiscordIntents intents = DiscordIntents.AllUnprivileged;
                try
                {
                    gcl = (GatewayCompressionLevel)((byte)JsonDatabase.GetInt("gateway-compression-level"));
                }
                catch (Exception) { }

                try
                {
                    intents = (DiscordIntents)JsonDatabase.GetInt("intents");
                }
                catch (Exception) { }


                _client = new DiscordClient(new DiscordConfiguration()
                {
                    Token = JsonDatabase.GetString("token"),
                    TokenType = TokenType.Bot,
                    AutoReconnect = JsonDatabase.GetBool("auto-reconnect") ?? true,
                    AlwaysCacheMembers = JsonDatabase.GetBool("always-cache-members") ?? true,
                    GatewayCompressionLevel = gcl,
                    Intents = intents,
                    MessageCacheSize = JsonDatabase.GetInt("message-cache-size") ?? 1024,
                    MinimumLogLevel = Microsoft.Extensions.Logging.LogLevel.None,
                });
            }
            catch (ArgumentNullException)
            {
                Logger.WriteLine("{cyan}Please enter a token. You can use the \"help\" command for help.");
                return;
            }

            RefreshPlugins();
            try
            {
                var result = GetClientStatus();
                _client.ConnectAsync(result.Activity, result.UserStatus);
            }
            catch (Exception ex) { Logger.Error(ex); }
        }

        private static void RefreshPlugins()
        {
            try
            {
                foreach (var consolePlugin in PluginLoader.ConsolePlugins)
                {
                    bool clientEnabled = false;
                    try
                    {
                        clientEnabled = consolePlugin.ClientEnabled;
                    }
                    catch (Exception) { }
                    if (clientEnabled)
                        try
                        {
                            try
                            {
                                consolePlugin.OnLoad(_client);
                            }
                            catch (NotImplementedException) { }

                            string pluginName = "{blue}Unknown{end}";
                            string authorName = "{blue}Unknown{end}";
                            try
                            {
                                pluginName = consolePlugin.Name;
                            }
                            catch (Exception) { }
                            try
                            {
                                authorName = consolePlugin.AuthorName;
                            }
                            catch (Exception) { }
                            Logger.WriteLine($"{{cyan}}{pluginName}{{green}} Plugin Loaded. {{yellow2}}Developed by {authorName}.");
                        }
                        catch (Exception e)
                        {
                            Logger.Error(e);
                        }
                }
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }

            try
            {
                foreach (var plugin in PluginLoader.Plugins)
                {
                    if (plugin != null && plugin.Plugin != null)
                        try
                        {
                            try
                            {

                                plugin.Plugin.OnLoad(_client);
                            }
                            catch (NotImplementedException) { }

                            string pluginName = "{blue}Unknown{end}";
                            string authorName = "{blue}Unknown{end}";
                            try
                            {
                                pluginName = plugin.Plugin.Name;
                            }
                            catch (Exception) { }
                            try
                            {
                                authorName = plugin.Plugin.AuthorName;
                            }
                            catch (Exception) { }

                            Logger.WriteLine($"{{cyan}}{pluginName}{{green}} Plugin Loaded. {{yellow2}}Developed by {authorName}.");
                        }
                        catch (Exception e)
                        {
                            Logger.Error(e);
                        }
                }
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
            Logger.WriteLine("{green}Plugin load finished.");
            try
            {
                try
                {
                    _client.SocketErrored -= client_SocketErrored;
                }
                catch (Exception) { }
                _client.SocketErrored += client_SocketErrored;
                try
                {
                    _client.SocketOpened -= client_SocketOpened;
                }
                catch (Exception) { }
                _client.SocketOpened += client_SocketOpened;
                try
                {
                    _client.SocketClosed -= client_SocketClosed;
                }
                catch (Exception) { }
                _client.SocketClosed += client_SocketClosed;
                try
                {
                    _client.Ready -= client_Ready;
                }
                catch (Exception) { }
                _client.Ready += client_Ready;
                try
                {
                    _client.Resumed -= client_Resumed;
                }
                catch (Exception) { }
                _client.Resumed += client_Resumed;
                try
                {
                    _client.Heartbeated -= client_Heartbeated;
                }
                catch (Exception) { }
                _client.Heartbeated += client_Heartbeated;
                try
                {
                    _client.Zombied -= client_Zombied;
                }
                catch (Exception) { }
                _client.Zombied += client_Zombied;
                try
                {
                    _client.ChannelCreated -= client_ChannelCreated;
                }
                catch (Exception) { }
                _client.ChannelCreated += client_ChannelCreated;
                try
                {
                    _client.ChannelUpdated -= client_ChannelUpdated;
                }
                catch (Exception) { }
                _client.ChannelUpdated += client_ChannelUpdated;
                try
                {
                    _client.ChannelDeleted -= client_ChannelDeleted;
                }
                catch (Exception) { }
                _client.ChannelDeleted += client_ChannelDeleted;
                try
                {
                    _client.DmChannelDeleted -= client_DmChannelDeleted;
                }
                catch (Exception) { }
                _client.DmChannelDeleted += client_DmChannelDeleted;
                try
                {
                    _client.ChannelPinsUpdated -= client_ChannelPinsUpdated;
                }
                catch (Exception) { }
                _client.ChannelPinsUpdated += client_ChannelPinsUpdated;
                try
                {
                    _client.GuildCreated -= client_GuildCreated;
                }
                catch (Exception) { }
                _client.GuildCreated += client_GuildCreated;
                try
                {
                    _client.GuildAvailable -= client_GuildAvailable;
                }
                catch (Exception) { }
                _client.GuildAvailable += client_GuildAvailable;
                try
                {
                    _client.GuildUpdated -= client_GuildUpdated;
                }
                catch (Exception) { }
                _client.GuildUpdated += client_GuildUpdated;
                try
                {
                    _client.GuildDeleted -= client_GuildDeleted;
                }
                catch (Exception) { }
                _client.GuildDeleted += client_GuildDeleted;
                try
                {
                    _client.GuildUnavailable -= client_GuildUnavailable;
                }
                catch (Exception) { }
                _client.GuildUnavailable += client_GuildUnavailable;
                try
                {
                    _client.GuildDownloadCompleted -= client_GuildDownloadCompleted;
                }
                catch (Exception) { }
                _client.GuildDownloadCompleted += client_GuildDownloadCompleted;
                try
                {
                    _client.GuildEmojisUpdated -= client_GuildEmojisUpdated;
                }
                catch (Exception) { }
                _client.GuildEmojisUpdated += client_GuildEmojisUpdated;
                try
                {
                    _client.GuildStickersUpdated -= client_GuildStickersUpdated;
                }
                catch (Exception) { }
                _client.GuildStickersUpdated += client_GuildStickersUpdated;
                try
                {
                    _client.GuildIntegrationsUpdated -= client_GuildIntegrationsUpdated;
                }
                catch (Exception) { }
                _client.GuildIntegrationsUpdated += client_GuildIntegrationsUpdated;
                try
                {
                    _client.ScheduledGuildEventCreated -= client_ScheduledGuildEventCreated;
                }
                catch (Exception) { }
                _client.ScheduledGuildEventCreated += client_ScheduledGuildEventCreated;
                try
                {
                    _client.ScheduledGuildEventUpdated -= client_ScheduledGuildEventUpdated;
                }
                catch (Exception) { }
                _client.ScheduledGuildEventUpdated += client_ScheduledGuildEventUpdated;
                try
                {
                    _client.ScheduledGuildEventDeleted -= client_ScheduledGuildEventDeleted;
                }
                catch (Exception) { }
                _client.ScheduledGuildEventDeleted += client_ScheduledGuildEventDeleted;
                try
                {
                    _client.ScheduledGuildEventCompleted -= client_ScheduledGuildEventCompleted;
                }
                catch (Exception) { }
                _client.ScheduledGuildEventCompleted += client_ScheduledGuildEventCompleted;
                try
                {
                    _client.ScheduledGuildEventUserAdded -= client_ScheduledGuildEventUserAdded;
                }
                catch (Exception) { }
                _client.ScheduledGuildEventUserAdded += client_ScheduledGuildEventUserAdded;
                try
                {
                    _client.ScheduledGuildEventUserRemoved -= client_ScheduledGuildEventUserRemoved;
                }
                catch (Exception) { }
                _client.ScheduledGuildEventUserRemoved += client_ScheduledGuildEventUserRemoved;
                try
                {
                    _client.GuildBanAdded -= client_GuildBanAdded;
                }
                catch (Exception) { }
                _client.GuildBanAdded += client_GuildBanAdded;
                try
                {
                    _client.GuildBanRemoved -= client_GuildBanRemoved;
                }
                catch (Exception) { }
                _client.GuildBanRemoved += client_GuildBanRemoved;
                try
                {
                    _client.GuildMemberAdded -= client_GuildMemberAdded;
                }
                catch (Exception) { }
                _client.GuildMemberAdded += client_GuildMemberAdded;
                try
                {
                    _client.GuildMemberRemoved -= client_GuildMemberRemoved;
                }
                catch (Exception) { }
                _client.GuildMemberRemoved += client_GuildMemberRemoved;
                try
                {
                    _client.GuildMemberUpdated -= client_GuildMemberUpdated;
                }
                catch (Exception) { }
                _client.GuildMemberUpdated += client_GuildMemberUpdated;
                try
                {
                    _client.GuildMembersChunked -= client_GuildMembersChunked;
                }
                catch (Exception) { }
                _client.GuildMembersChunked += client_GuildMembersChunked;
                try
                {
                    _client.GuildRoleCreated -= client_GuildRoleCreated;
                }
                catch (Exception) { }
                _client.GuildRoleCreated += client_GuildRoleCreated;
                try
                {
                    _client.GuildRoleUpdated -= client_GuildRoleUpdated;
                }
                catch (Exception) { }
                _client.GuildRoleUpdated += client_GuildRoleUpdated;
                try
                {
                    _client.GuildRoleDeleted -= client_GuildRoleDeleted;
                }
                catch (Exception) { }
                _client.GuildRoleDeleted += client_GuildRoleDeleted;
                try
                {
                    _client.InviteCreated -= client_InviteCreated;
                }
                catch (Exception) { }
                _client.InviteCreated += client_InviteCreated;
                try
                {
                    _client.InviteDeleted -= client_InviteDeleted;
                }
                catch (Exception) { }
                _client.InviteDeleted += client_InviteDeleted;
                try
                {
                    _client.MessageCreated -= client_MessageCreated;
                }
                catch (Exception) { }
                _client.MessageCreated += client_MessageCreated;
                try
                {
                    _client.MessageAcknowledged -= client_MessageAcknowledged;
                }
                catch (Exception) { }
                _client.MessageAcknowledged += client_MessageAcknowledged;
                try
                {
                    _client.MessageUpdated -= client_MessageUpdated;
                }
                catch (Exception) { }
                _client.MessageUpdated += client_MessageUpdated;
                try
                {
                    _client.MessageDeleted -= client_MessageDeleted;
                }
                catch (Exception) { }
                _client.MessageDeleted += client_MessageDeleted;
                try
                {
                    _client.MessagesBulkDeleted -= client_MessagesBulkDeleted;
                }
                catch (Exception) { }
                _client.MessagesBulkDeleted += client_MessagesBulkDeleted;
                try
                {
                    _client.MessageReactionAdded -= client_MessageReactionAdded;
                }
                catch (Exception) { }
                _client.MessageReactionAdded += client_MessageReactionAdded;
                try
                {
                    _client.MessageReactionRemoved -= client_MessageReactionRemoved;
                }
                catch (Exception) { }
                _client.MessageReactionRemoved += client_MessageReactionRemoved;
                try
                {
                    _client.MessageReactionsCleared -= client_MessageReactionsCleared;
                }
                catch (Exception) { }
                _client.MessageReactionsCleared += client_MessageReactionsCleared;
                try
                {
                    _client.MessageReactionRemovedEmoji -= client_MessageReactionRemovedEmoji;
                }
                catch (Exception) { }
                _client.MessageReactionRemovedEmoji += client_MessageReactionRemovedEmoji;
                try
                {
                    _client.PresenceUpdated -= client_PresenceUpdated;
                }
                catch (Exception) { }
                _client.PresenceUpdated += client_PresenceUpdated;
                try
                {
                    _client.UserSettingsUpdated -= client_UserSettingsUpdated;
                }
                catch (Exception) { }
                _client.UserSettingsUpdated += client_UserSettingsUpdated;
                try
                {
                    _client.UserUpdated -= client_UserUpdated;
                }
                catch (Exception) { }
                _client.UserUpdated += client_UserUpdated;
                try
                {
                    _client.VoiceStateUpdated -= client_VoiceStateUpdated;
                }
                catch (Exception) { }
                _client.VoiceStateUpdated += client_VoiceStateUpdated;
                try
                {
                    _client.VoiceServerUpdated -= client_VoiceServerUpdated;
                }
                catch (Exception) { }
                _client.VoiceServerUpdated += client_VoiceServerUpdated;
                try
                {
                    _client.ThreadCreated -= client_ThreadCreated;
                }
                catch (Exception) { }
                _client.ThreadCreated += client_ThreadCreated;
                try
                {
                    _client.ThreadUpdated -= client_ThreadUpdated;
                }
                catch (Exception) { }
                _client.ThreadUpdated += client_ThreadUpdated;
                try
                {
                    _client.ThreadDeleted -= client_ThreadDeleted;
                }
                catch (Exception) { }
                _client.ThreadDeleted += client_ThreadDeleted;
                try
                {
                    _client.ThreadListSynced -= client_ThreadListSynced;
                }
                catch (Exception) { }
                _client.ThreadListSynced += client_ThreadListSynced;
                try
                {
                    _client.ThreadMemberUpdated -= client_ThreadMemberUpdated;
                }
                catch (Exception) { }
                _client.ThreadMemberUpdated += client_ThreadMemberUpdated;
                try
                {
                    _client.ThreadMembersUpdated -= client_ThreadMembersUpdated;
                }
                catch (Exception) { }
                _client.ThreadMembersUpdated += client_ThreadMembersUpdated;
                try
                {
                    _client.ApplicationCommandCreated -= client_ApplicationCommandCreated;
                }
                catch (Exception) { }
                _client.ApplicationCommandCreated += client_ApplicationCommandCreated;
                try
                {
                    _client.ApplicationCommandUpdated -= client_ApplicationCommandUpdated;
                }
                catch (Exception) { }
                _client.ApplicationCommandUpdated += client_ApplicationCommandUpdated;
                try
                {
                    _client.ApplicationCommandDeleted -= client_ApplicationCommandDeleted;
                }
                catch (Exception) { }
                _client.ApplicationCommandDeleted += client_ApplicationCommandDeleted;
                try
                {
                    _client.IntegrationCreated -= client_IntegrationCreated;
                }
                catch (Exception) { }
                _client.IntegrationCreated += client_IntegrationCreated;
                try
                {
                    _client.IntegrationUpdated -= client_IntegrationUpdated;
                }
                catch (Exception) { }
                _client.IntegrationUpdated += client_IntegrationUpdated;
                try
                {
                    _client.IntegrationDeleted -= client_IntegrationDeleted;
                }
                catch (Exception) { }
                _client.IntegrationDeleted += client_IntegrationDeleted;
                try
                {
                    _client.StageInstanceCreated -= client_StageInstanceCreated;
                }
                catch (Exception) { }
                _client.StageInstanceCreated += client_StageInstanceCreated;
                try
                {
                    _client.StageInstanceUpdated -= client_StageInstanceUpdated;
                }
                catch (Exception) { }
                _client.StageInstanceUpdated += client_StageInstanceUpdated;
                try
                {
                    _client.StageInstanceDeleted -= client_StageInstanceDeleted;
                }
                catch (Exception) { }
                _client.StageInstanceDeleted += client_StageInstanceDeleted;
                try
                {
                    _client.InteractionCreated -= client_InteractionCreated;
                }
                catch (Exception) { }
                _client.InteractionCreated += client_InteractionCreated;
                try
                {
                    _client.ComponentInteractionCreated -= client_ComponentInteractionCreated;
                }
                catch (Exception) { }
                _client.ComponentInteractionCreated += client_ComponentInteractionCreated;
                try
                {
                    _client.ModalSubmitted -= client_ModalSubmitted;
                }
                catch (Exception) { }
                _client.ModalSubmitted += client_ModalSubmitted;
                try
                {
                    _client.ContextMenuInteractionCreated -= client_ContextMenuInteractionCreated;
                }
                catch (Exception) { }
                _client.ContextMenuInteractionCreated += client_ContextMenuInteractionCreated;
                try
                {
                    _client.TypingStarted -= client_TypingStarted;
                }
                catch (Exception) { }
                _client.TypingStarted += client_TypingStarted;
                try
                {
                    _client.UnknownEvent -= client_UnknownEvent;
                }
                catch (Exception) { }
                _client.UnknownEvent += client_UnknownEvent;
                try
                {
                    _client.WebhooksUpdated -= client_WebhooksUpdated;
                }
                catch (Exception) { }
                _client.WebhooksUpdated += client_WebhooksUpdated;
                try
                {
                    _client.ClientErrored -= client_ClientErrored;
                }
                catch (Exception) { }
                _client.ClientErrored += client_ClientErrored;
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
        }

        public static (DiscordActivity Activity, UserStatus UserStatus) GetClientStatus()
        {
            string name = JsonDatabase.GetString("activity-name")
                  ,stream_url = JsonDatabase.GetString("stream-url");
            ActivityType aType = ActivityType.Playing;
            UserStatus userStatus = UserStatus.Online;

            try
            {
                aType = (ActivityType)JsonDatabase.GetInt("activity-type");
            }
            catch (Exception) { }

            try
            {
                userStatus = (UserStatus)JsonDatabase.GetInt("user-status");
            }
            catch (Exception) { }


            return (new DiscordActivity() { Name = name, ActivityType = aType, StreamUrl = stream_url }, userStatus);
        }

        public static void RefreshClientStatus()
        {
            var result = GetClientStatus();
            _client.UpdateStatusAsync(result.Activity, result.UserStatus);
        }

        public static void SaveClientStatus(string name = null, string stream_url = null, ActivityType aType = ActivityType.Playing, UserStatus userStatus = UserStatus.Online)
        {
            JsonDatabase.Set("activity-name", name);
            JsonDatabase.Set("activity-type", (int)aType);
            JsonDatabase.Set("stream-url", stream_url);
            JsonDatabase.Set("user-status", (int)userStatus);
            RefreshClientStatus();
        }

        public static void SaveActivityName(string name)
        {
            JsonDatabase.Set("activity-name", name);
            RefreshClientStatus();
        }

        public static void SaveActivityStreamUrl(string stream_url)
        {
            JsonDatabase.Set("stream-url", stream_url);
            RefreshClientStatus();
        }

        public static void SaveActivityType(ActivityType aType = ActivityType.Playing)
        {
            JsonDatabase.Set("activity-type", (int)aType);
            RefreshClientStatus();
        }

        public static void SaveUserStatus(UserStatus userStatus = UserStatus.Online)
        {
            JsonDatabase.Set("user-status", (int)userStatus);
            RefreshClientStatus();
        }

        #region Client Event Methods
        private static Task client_ClientErrored(DiscordClient sender, ClientErrorEventArgs e)
        {
            foreach (var item in PluginLoader.ClientErroredEvents)
            {
                Task.Factory.StartNew(() =>
                {
                    try
                    {
                        item.OnCalled(e);
                    }
                    catch (Exception e) { Logger.Error(e); }
                });
            }
            return null;
        }

        private static Task client_WebhooksUpdated(DiscordClient sender, WebhooksUpdateEventArgs e)
        {
            foreach (var item in PluginLoader.WebhooksUpdatedEvents)
            {
                Task.Factory.StartNew(() =>
                {
                    try
                    {
                        item.OnCalled(e);
                    }
                    catch (Exception e) { Logger.Error(e); }
                });
            }
            return null;
        }

        private static Task client_UnknownEvent(DiscordClient sender, UnknownEventArgs e)
        {
            foreach (var item in PluginLoader.UnknownEventEvents)
            {
                Task.Factory.StartNew(() =>
                {
                    try
                    {
                        item.OnCalled(e);
                    }
                    catch (Exception e) { Logger.Error(e); }
                });
            }
            return null;
        }

        private static Task client_TypingStarted(DiscordClient sender, TypingStartEventArgs e)
        {
            foreach (var item in PluginLoader.TypingStartedEvents)
            {
                Task.Factory.StartNew(() =>
                {
                    try
                    {
                        item.OnCalled(e);
                    }
                    catch (Exception e) { Logger.Error(e); }
                });
            }
            return null;
        }

        private static Task client_ContextMenuInteractionCreated(DiscordClient sender, ContextMenuInteractionCreateEventArgs e)
        {
            foreach (var item in PluginLoader.ContextMenuInteractionCreatedEvents)
            {
                Task.Factory.StartNew(() =>
                {
                    try
                    {
                        item.OnCalled(e);
                    }
                    catch (Exception e) { Logger.Error(e); }
                });
            }
            return null;
        }

        private static Task client_ModalSubmitted(DiscordClient sender, ModalSubmitEventArgs e)
        {
            foreach (var item in PluginLoader.ModalSubmittedEvents)
            {
                Task.Factory.StartNew(() =>
                {
                    try
                    {
                        item.OnCalled(e);
                    }
                    catch (Exception e) { Logger.Error(e); }
                });
            }
            return null;
        }

        private static Task client_ComponentInteractionCreated(DiscordClient sender, ComponentInteractionCreateEventArgs e)
        {
            foreach (var item in PluginLoader.ComponentInteractionCreatedEvents)
            {
                Task.Factory.StartNew(() =>
                {
                    try
                    {
                        item.OnCalled(e);
                    }
                    catch (Exception e) { Logger.Error(e); }
                });
            }
            return null;
        }

        private static Task client_InteractionCreated(DiscordClient sender, InteractionCreateEventArgs e)
        {
            foreach (var item in PluginLoader.InteractionCreatedEvents)
            {
                Task.Factory.StartNew(() =>
                {
                    try
                    {
                        item.OnCalled(e);
                    }
                    catch (Exception e) { Logger.Error(e); }
                });
            }
            return null;
        }

        private static Task client_StageInstanceDeleted(DiscordClient sender, StageInstanceDeleteEventArgs e)
        {
            foreach (var item in PluginLoader.StageInstanceDeletedEvents)
            {
                Task.Factory.StartNew(() =>
                {
                    try
                    {
                        item.OnCalled(e);
                    }
                    catch (Exception e) { Logger.Error(e); }
                });
            }
            return null;
        }

        private static Task client_StageInstanceUpdated(DiscordClient sender, StageInstanceUpdateEventArgs e)
        {
            foreach (var item in PluginLoader.StageInstanceUpdatedEvents)
            {
                Task.Factory.StartNew(() =>
                {
                    try
                    {
                        item.OnCalled(e);
                    }
                    catch (Exception e) { Logger.Error(e); }
                });
            }
            return null;
        }

        private static Task client_StageInstanceCreated(DiscordClient sender, StageInstanceCreateEventArgs e)
        {
            foreach (var item in PluginLoader.StageInstanceCreatedEvents)
            {
                Task.Factory.StartNew(() =>
                {
                    try
                    {
                        item.OnCalled(e);
                    }
                    catch (Exception e) { Logger.Error(e); }
                });
            }
            return null;
        }

        private static Task client_IntegrationDeleted(DiscordClient sender, IntegrationDeleteEventArgs e)
        {
            foreach (var item in PluginLoader.IntegrationDeletedEvents)
            {
                Task.Factory.StartNew(() =>
                {
                    try
                    {
                        item.OnCalled(e);
                    }
                    catch (Exception e) { Logger.Error(e); }
                });
            }
            return null;
        }

        private static Task client_IntegrationUpdated(DiscordClient sender, IntegrationUpdateEventArgs e)
        {
            foreach (var item in PluginLoader.IntegrationUpdatedEvents)
            {
                Task.Factory.StartNew(() =>
                {
                    try
                    {
                        item.OnCalled(e);
                    }
                    catch (Exception e) { Logger.Error(e); }
                });
            }
            return null;
        }

        private static Task client_IntegrationCreated(DiscordClient sender, IntegrationCreateEventArgs e)
        {
            foreach (var item in PluginLoader.IntegrationCreatedEvents)
            {
                Task.Factory.StartNew(() =>
                {
                    try
                    {
                        item.OnCalled(e);
                    }
                    catch (Exception e) { Logger.Error(e); }
                });
            }
            return null;
        }

        private static Task client_ApplicationCommandDeleted(DiscordClient sender, ApplicationCommandEventArgs e)
        {
            foreach (var item in PluginLoader.ApplicationCommandDeletedEvents)
            {
                Task.Factory.StartNew(() =>
                {
                    try
                    {
                        item.OnCalled(e);
                    }
                    catch (Exception e) { Logger.Error(e); }
                });
            }
            return null;
        }

        private static Task client_ApplicationCommandUpdated(DiscordClient sender, ApplicationCommandEventArgs e)
        {
            foreach (var item in PluginLoader.ApplicationCommandUpdatedEvents)
            {
                Task.Factory.StartNew(() =>
                {
                    try
                    {
                        item.OnCalled(e);
                    }
                    catch (Exception e) { Logger.Error(e); }
                });
            }
            return null;
        }

        private static Task client_ApplicationCommandCreated(DiscordClient sender, ApplicationCommandEventArgs e)
        {
            foreach (var item in PluginLoader.ApplicationCommandCreatedEvents)
            {
                Task.Factory.StartNew(() =>
                {
                    try
                    {
                        item.OnCalled(e);
                    }
                    catch (Exception e) { Logger.Error(e); }
                });
            }
            return null;
        }

        private static Task client_ThreadMembersUpdated(DiscordClient sender, ThreadMembersUpdateEventArgs e)
        {
            foreach (var item in PluginLoader.ThreadMembersUpdatedEvents)
            {
                Task.Factory.StartNew(() =>
                {
                    try
                    {
                        item.OnCalled(e);
                    }
                    catch (Exception e) { Logger.Error(e); }
                });
            }
            return null;
        }

        private static Task client_ThreadMemberUpdated(DiscordClient sender, ThreadMemberUpdateEventArgs e)
        {
            foreach (var item in PluginLoader.ThreadMemberUpdatedEvents)
            {
                Task.Factory.StartNew(() =>
                {
                    try
                    {
                        item.OnCalled(e);
                    }
                    catch (Exception e) { Logger.Error(e); }
                });
            }
            return null;
        }

        private static Task client_ThreadListSynced(DiscordClient sender, ThreadListSyncEventArgs e)
        {
            foreach (var item in PluginLoader.ThreadListSyncedEvents)
            {
                Task.Factory.StartNew(() =>
                {
                    try
                    {
                        item.OnCalled(e);
                    }
                    catch (Exception e) { Logger.Error(e); }
                });
            }
            return null;
        }

        private static Task client_ThreadDeleted(DiscordClient sender, ThreadDeleteEventArgs e)
        {
            foreach (var item in PluginLoader.ThreadDeletedEvents)
            {
                Task.Factory.StartNew(() =>
                {
                    try
                    {
                        item.OnCalled(e);
                    }
                    catch (Exception e) { Logger.Error(e); }
                });
            }
            return null;
        }

        private static Task client_ThreadUpdated(DiscordClient sender, ThreadUpdateEventArgs e)
        {
            foreach (var item in PluginLoader.ThreadUpdatedEvents)
            {
                Task.Factory.StartNew(() =>
                {
                    try
                    {
                        item.OnCalled(e);
                    }
                    catch (Exception e) { Logger.Error(e); }
                });
            }
            return null;
        }

        private static Task client_ThreadCreated(DiscordClient sender, ThreadCreateEventArgs e)
        {
            foreach (var item in PluginLoader.ThreadCreatedEvents)
            {
                Task.Factory.StartNew(() =>
                {
                    try
                    {
                        item.OnCalled(e);
                    }
                    catch (Exception e) { Logger.Error(e); }
                });
            }
            return null;
        }

        private static Task client_VoiceServerUpdated(DiscordClient sender, VoiceServerUpdateEventArgs e)
        {
            foreach (var item in PluginLoader.VoiceServerUpdatedEvents)
            {
                Task.Factory.StartNew(() =>
                {
                    try
                    {
                        item.OnCalled(e);
                    }
                    catch (Exception e) { Logger.Error(e); }
                });
            }
            return null;
        }

        private static Task client_VoiceStateUpdated(DiscordClient sender, VoiceStateUpdateEventArgs e)
        {
            foreach (var item in PluginLoader.VoiceStateUpdatedEvents)
            {
                Task.Factory.StartNew(() =>
                {
                    try
                    {
                        item.OnCalled(e);
                    }
                    catch (Exception e) { Logger.Error(e); }
                });
            }
            return null;
        }

        private static Task client_UserUpdated(DiscordClient sender, UserUpdateEventArgs e)
        {
            foreach (var item in PluginLoader.UserUpdatedEvents)
            {
                Task.Factory.StartNew(() =>
                {
                    try
                    {
                        item.OnCalled(e);
                    }
                    catch (Exception e) { Logger.Error(e); }
                });
            }
            return null;
        }

        private static Task client_UserSettingsUpdated(DiscordClient sender, UserSettingsUpdateEventArgs e)
        {
            foreach (var item in PluginLoader.UserSettingsUpdatedEvents)
            {
                Task.Factory.StartNew(() =>
                {
                    try
                    {
                        item.OnCalled(e);
                    }
                    catch (Exception e) { Logger.Error(e); }
                });
            }
            return null;
        }

        private static Task client_PresenceUpdated(DiscordClient sender, PresenceUpdateEventArgs e)
        {
            foreach (var item in PluginLoader.PresenceUpdatedEvents)
            {
                Task.Factory.StartNew(() =>
                {
                    try
                    {
                        item.OnCalled(e);
                    }
                    catch (Exception e) { Logger.Error(e); }
                });
            }
            return null;
        }

        private static Task client_MessageReactionRemovedEmoji(DiscordClient sender, MessageReactionRemoveEmojiEventArgs e)
        {
            foreach (var item in PluginLoader.MessageReactionRemovedEmojiEvents)
            {
                Task.Factory.StartNew(() =>
                {
                    try
                    {
                        item.OnCalled(e);
                    }
                    catch (Exception e) { Logger.Error(e); }
                });
            }
            return null;
        }

        private static Task client_MessageReactionsCleared(DiscordClient sender, MessageReactionsClearEventArgs e)
        {
            foreach (var item in PluginLoader.MessageReactionsClearedEvents)
            {
                Task.Factory.StartNew(() =>
                {
                    try
                    {
                        item.OnCalled(e);
                    }
                    catch (Exception e) { Logger.Error(e); }
                });
            }
            return null;
        }

        private static Task client_MessageReactionRemoved(DiscordClient sender, MessageReactionRemoveEventArgs e)
        {
            foreach (var item in PluginLoader.MessageReactionRemovedEvents)
            {
                Task.Factory.StartNew(() =>
                {
                    try
                    {
                        item.OnCalled(e);
                    }
                    catch (Exception e) { Logger.Error(e); }
                });
            }
            return null;
        }

        private static Task client_MessageReactionAdded(DiscordClient sender, MessageReactionAddEventArgs e)
        {
            foreach (var item in PluginLoader.MessageReactionAddedEvents)
            {
                Task.Factory.StartNew(() =>
                {
                    try
                    {
                        item.OnCalled(e);
                    }
                    catch (Exception e) { Logger.Error(e); }
                });
            }
            return null;
        }

        private static Task client_MessagesBulkDeleted(DiscordClient sender, MessageBulkDeleteEventArgs e)
        {
            foreach (var item in PluginLoader.MessagesBulkDeletedEvents)
            {
                Task.Factory.StartNew(() =>
                {
                    try
                    {
                        item.OnCalled(e);
                    }
                    catch (Exception e) { Logger.Error(e); }
                });
            }
            return null;
        }

        private static Task client_MessageDeleted(DiscordClient sender, MessageDeleteEventArgs e)
        {
            foreach (var item in PluginLoader.MessageDeletedEvents)
            {
                Task.Factory.StartNew(() =>
                {
                    try
                    {
                        item.OnCalled(e);
                    }
                    catch (Exception e) { Logger.Error(e); }
                });
            }
            return null;
        }

        private static Task client_MessageUpdated(DiscordClient sender, MessageUpdateEventArgs e)
        {
            foreach (var item in PluginLoader.MessageUpdatedEvents)
            {
                Task.Factory.StartNew(() =>
                {
                    try
                    {
                        item.OnCalled(e);
                    }
                    catch (Exception e) { Logger.Error(e); }
                });
            }
            return null;
        }

        private static Task client_MessageAcknowledged(DiscordClient sender, MessageAcknowledgeEventArgs e)
        {
            foreach (var item in PluginLoader.MessageAcknowledgedEvents)
            {
                Task.Factory.StartNew(() =>
                {
                    try
                    {
                        item.OnCalled(e);
                    }
                    catch (Exception e) { Logger.Error(e); }
                });
            }
            return null;
        }

        private static Task client_MessageCreated(DiscordClient sender, MessageCreateEventArgs e)
        {
            try
            {
                string[] _args = e.Message.Content.Split(' ');
                string cmd = _args[0];
                _args = _args.Skip(1).ToArray();
                string prefix = "";
                try
                {
                    prefix = JsonDatabase.GetString("prefix." + (e.Guild != null ? e.Guild.Id : (ulong)0));
                    if (prefix == null)
                        prefix = JsonDatabase.GetString("prefix");
                    if (prefix == null)
                        prefix = "";
                }
                catch (Exception) { }

                foreach (var item in PluginLoader.MessageCreatedEvents)
                {
                    Task.Factory.StartNew(() =>
                    {
                        try
                        {
                            item.OnCalled(e);
                        }
                        catch (Exception ex) { Logger.Error(ex); }
                    });
                }

                if (cmd.ToLower().StartsWith(prefix.ToLower()) && !e.Author.IsBot)
                {
                    cmd = cmd.Substring(prefix.Length);
                    foreach (var command in PluginLoader.Commands)
                    {
                        try
                        {
                            if (command.IsEnabled())
                                foreach (var item in command.Command.Aliases)
                                {
                                    if (item.ToLower() == cmd.ToLower())
                                    {
                                        Task.Factory.StartNew(() =>
                                        {
                                            try
                                            {
                                                command.Command.OnCalled(_args, _client, e);
                                            }
                                            catch (Exception ex) { Logger.Error(ex); }
                                        });
                                    }
                                }
                        }
                        catch (NotImplementedException) { }
                        catch (Exception ex) { Logger.Error(ex); }
                    }
                }
            }
            catch (Exception ex) { Logger.Error(ex); }

            return null;
        }

        private static Task client_InviteDeleted(DiscordClient sender, InviteDeleteEventArgs e)
        {
            foreach (var item in PluginLoader.InviteDeletedEvents)
            {
                Task.Factory.StartNew(() =>
                {
                    try
                    {
                        item.OnCalled(e);
                    }
                    catch (Exception e) { Logger.Error(e); }
                });
            }
            return null;
        }

        private static Task client_InviteCreated(DiscordClient sender, InviteCreateEventArgs e)
        {
            foreach (var item in PluginLoader.InviteCreatedEvents)
            {
                Task.Factory.StartNew(() =>
                {
                    try
                    {
                        item.OnCalled(e);
                    }
                    catch (Exception e) { Logger.Error(e); }
                });
            }
            return null;
        }

        private static Task client_GuildRoleDeleted(DiscordClient sender, GuildRoleDeleteEventArgs e)
        {
            foreach (var item in PluginLoader.GuildRoleDeletedEvents)
            {
                Task.Factory.StartNew(() =>
                {
                    try
                    {
                        item.OnCalled(e);
                    }
                    catch (Exception e) { Logger.Error(e); }
                });
            }
            return null;
        }

        private static Task client_GuildRoleUpdated(DiscordClient sender, GuildRoleUpdateEventArgs e)
        {
            foreach (var item in PluginLoader.GuildRoleUpdatedEvents)
            {
                Task.Factory.StartNew(() =>
                {
                    try
                    {
                        item.OnCalled(e);
                    }
                    catch (Exception e) { Logger.Error(e); }
                });
            }
            return null;
        }

        private static Task client_GuildRoleCreated(DiscordClient sender, GuildRoleCreateEventArgs e)
        {
            foreach (var item in PluginLoader.GuildRoleCreatedEvents)
            {
                Task.Factory.StartNew(() =>
                {
                    try
                    {
                        item.OnCalled(e);
                    }
                    catch (Exception e) { Logger.Error(e); }
                });
            }
            return null;
        }

        private static Task client_GuildMembersChunked(DiscordClient sender, GuildMembersChunkEventArgs e)
        {
            foreach (var item in PluginLoader.GuildMembersChunkedEvents)
            {
                Task.Factory.StartNew(() =>
                {
                    try
                    {
                        item.OnCalled(e);
                    }
                    catch (Exception e) { Logger.Error(e); }
                });
            }
            return null;
        }

        private static Task client_GuildMemberUpdated(DiscordClient sender, GuildMemberUpdateEventArgs e)
        {
            foreach (var item in PluginLoader.GuildMemberUpdatedEvents)
            {
                Task.Factory.StartNew(() =>
                {
                    try
                    {
                        item.OnCalled(e);
                    }
                    catch (Exception e) { Logger.Error(e); }
                });
            }
            return null;
        }

        private static Task client_GuildMemberRemoved(DiscordClient sender, GuildMemberRemoveEventArgs e)
        {
            foreach (var item in PluginLoader.GuildMemberRemovedEvents)
            {
                Task.Factory.StartNew(() =>
                {
                    try
                    {
                        item.OnCalled(e);
                    }
                    catch (Exception e) { Logger.Error(e); }
                });
            }
            return null;
        }

        private static Task client_GuildMemberAdded(DiscordClient sender, GuildMemberAddEventArgs e)
        {
            foreach (var item in PluginLoader.GuildMemberAddedEvents)
            {
                Task.Factory.StartNew(() =>
                {
                    try
                    {
                        item.OnCalled(e);
                    }
                    catch (Exception e) { Logger.Error(e); }
                });
            }
            return null;
        }

        private static Task client_GuildBanRemoved(DiscordClient sender, GuildBanRemoveEventArgs e)
        {
            foreach (var item in PluginLoader.GuildBanRemovedEvents)
            {
                Task.Factory.StartNew(() =>
                {
                    try
                    {
                        item.OnCalled(e);
                    }
                    catch (Exception e) { Logger.Error(e); }
                });
            }
            return null;
        }

        private static Task client_GuildBanAdded(DiscordClient sender, GuildBanAddEventArgs e)
        {
            foreach (var item in PluginLoader.GuildBanAddedEvents)
            {
                Task.Factory.StartNew(() =>
                {
                    try
                    {
                        item.OnCalled(e);
                    }
                    catch (Exception e) { Logger.Error(e); }
                });
            }
            return null;
        }

        private static Task client_ScheduledGuildEventUserRemoved(DiscordClient sender, ScheduledGuildEventUserRemoveEventArgs e)
        {
            foreach (var item in PluginLoader.ScheduledGuildEventUserRemovedEvents)
            {
                Task.Factory.StartNew(() =>
                {
                    try
                    {
                        item.OnCalled(e);
                    }
                    catch (Exception e) { Logger.Error(e); }
                });
            }
            return null;
        }

        private static Task client_ScheduledGuildEventUserAdded(DiscordClient sender, ScheduledGuildEventUserAddEventArgs e)
        {
            foreach (var item in PluginLoader.ScheduledGuildEventUserAddedEvents)
            {
                Task.Factory.StartNew(() =>
                {
                    try
                    {
                        item.OnCalled(e);
                    }
                    catch (Exception e) { Logger.Error(e); }
                });
            }
            return null;
        }

        private static Task client_ScheduledGuildEventCompleted(DiscordClient sender, ScheduledGuildEventCompletedEventArgs e)
        {
            foreach (var item in PluginLoader.ScheduledGuildEventCompletedEvents)
            {
                Task.Factory.StartNew(() =>
                {
                    try
                    {
                        item.OnCalled(e);
                    }
                    catch (Exception e) { Logger.Error(e); }
                });
            }
            return null;
        }

        private static Task client_ScheduledGuildEventDeleted(DiscordClient sender, ScheduledGuildEventDeleteEventArgs e)
        {
            foreach (var item in PluginLoader.ScheduledGuildEventDeletedEvents)
            {
                Task.Factory.StartNew(() =>
                {
                    try
                    {
                        item.OnCalled(e);
                    }
                    catch (Exception e) { Logger.Error(e); }
                });
            }
            return null;
        }

        private static Task client_ScheduledGuildEventUpdated(DiscordClient sender, ScheduledGuildEventUpdateEventArgs e)
        {
            foreach (var item in PluginLoader.ScheduledGuildEventUpdatedEvents)
            {
                Task.Factory.StartNew(() =>
                {
                    try
                    {
                        item.OnCalled(e);
                    }
                    catch (Exception e) { Logger.Error(e); }
                });
            }
            return null;
        }

        private static Task client_ScheduledGuildEventCreated(DiscordClient sender, ScheduledGuildEventCreateEventArgs e)
        {
            foreach (var item in PluginLoader.ScheduledGuildEventCreatedEvents)
            {
                Task.Factory.StartNew(() =>
                {
                    try
                    {
                        item.OnCalled(e);
                    }
                    catch (Exception e) { Logger.Error(e); }
                });
            }
            return null;
        }

        private static Task client_GuildIntegrationsUpdated(DiscordClient sender, GuildIntegrationsUpdateEventArgs e)
        {
            foreach (var item in PluginLoader.GuildIntegrationsUpdatedEvents)
            {
                Task.Factory.StartNew(() =>
                {
                    try
                    {
                        item.OnCalled(e);
                    }
                    catch (Exception e) { Logger.Error(e); }
                });
            }
            return null;
        }

        private static Task client_GuildStickersUpdated(DiscordClient sender, GuildStickersUpdateEventArgs e)
        {
            foreach (var item in PluginLoader.GuildStickersUpdatedEvents)
            {
                Task.Factory.StartNew(() =>
                {
                    try
                    {
                        item.OnCalled(e);
                    }
                    catch (Exception e) { Logger.Error(e); }
                });
            }
            return null;
        }

        private static Task client_GuildEmojisUpdated(DiscordClient sender, GuildEmojisUpdateEventArgs e)
        {
            foreach (var item in PluginLoader.GuildEmojisUpdatedEvents)
            {
                Task.Factory.StartNew(() =>
                {
                    try
                    {
                        item.OnCalled(e);
                    }
                    catch (Exception e) { Logger.Error(e); }
                });
            }
            return null;
        }

        private static Task client_GuildDownloadCompleted(DiscordClient sender, GuildDownloadCompletedEventArgs e)
        {
            foreach (var item in PluginLoader.GuildDownloadCompletedEvents)
            {
                Task.Factory.StartNew(() =>
                {
                    try
                    {
                        item.OnCalled(e);
                    }
                    catch (Exception e) { Logger.Error(e); }
                });
            }
            return null;
        }

        private static Task client_GuildUnavailable(DiscordClient sender, GuildDeleteEventArgs e)
        {
            foreach (var item in PluginLoader.GuildUnavailableEvents)
            {
                Task.Factory.StartNew(() =>
                {
                    try
                    {
                        item.OnCalled(e);
                    }
                    catch (Exception e) { Logger.Error(e); }
                });
            }
            return null;
        }

        private static Task client_GuildDeleted(DiscordClient sender, GuildDeleteEventArgs e)
        {
            foreach (var item in PluginLoader.GuildDeletedEvents)
            {
                Task.Factory.StartNew(() =>
                {
                    try
                    {
                        item.OnCalled(e);
                    }
                    catch (Exception e) { Logger.Error(e); }
                });
            }
            return null;
        }

        private static Task client_GuildUpdated(DiscordClient sender, GuildUpdateEventArgs e)
        {
            foreach (var item in PluginLoader.GuildUpdatedEvents)
            {
                Task.Factory.StartNew(() =>
                {
                    try
                    {
                        item.OnCalled(e);
                    }
                    catch (Exception e) { Logger.Error(e); }
                });
            }
            return null;
        }

        private static Task client_GuildAvailable(DiscordClient sender, GuildCreateEventArgs e)
        {
            foreach (var item in PluginLoader.GuildAvailableEvents)
            {
                Task.Factory.StartNew(() =>
                {
                    try
                    {
                        item.OnCalled(e);
                    }
                    catch (Exception e) { Logger.Error(e); }
                });
            }
            return null;
        }

        private static Task client_GuildCreated(DiscordClient sender, GuildCreateEventArgs e)
        {
            foreach (var item in PluginLoader.GuildCreatedEvents)
            {
                Task.Factory.StartNew(() =>
                {
                    try
                    {
                        item.OnCalled(e);
                    }
                    catch (Exception e) { Logger.Error(e); }
                });
            }
            return null;
        }

        private static Task client_ChannelPinsUpdated(DiscordClient sender, ChannelPinsUpdateEventArgs e)
        {
            foreach (var item in PluginLoader.ChannelPinsUpdatedEvents)
            {
                Task.Factory.StartNew(() =>
                {
                    try
                    {
                        item.OnCalled(e);
                    }
                    catch (Exception e) { Logger.Error(e); }
                });
            }
            return null;
        }

        private static Task client_DmChannelDeleted(DiscordClient sender, DmChannelDeleteEventArgs e)
        {
            foreach (var item in PluginLoader.DmChannelDeletedEvents)
            {
                Task.Factory.StartNew(() =>
                {
                    try
                    {
                        item.OnCalled(e);
                    }
                    catch (Exception e) { Logger.Error(e); }
                });
            }
            return null;
        }

        private static Task client_ChannelDeleted(DiscordClient sender, ChannelDeleteEventArgs e)
        {
            foreach (var item in PluginLoader.ChannelDeletedEvents)
            {
                Task.Factory.StartNew(() =>
                {
                    try
                    {
                        item.OnCalled(e);
                    }
                    catch (Exception e) { Logger.Error(e); }
                });
            }
            return null;
        }

        private static Task client_ChannelUpdated(DiscordClient sender, ChannelUpdateEventArgs e)
        {
            foreach (var item in PluginLoader.ChannelUpdatedEvents)
            {
                Task.Factory.StartNew(() =>
                {
                    try
                    {
                        item.OnCalled(e);
                    }
                    catch (Exception e) { Logger.Error(e); }
                });
            }
            return null;
        }

        private static Task client_ChannelCreated(DiscordClient sender, ChannelCreateEventArgs e)
        {
            foreach (var item in PluginLoader.ChannelCreatedEvents)
            {
                Task.Factory.StartNew(() =>
                {
                    try
                    {
                        item.OnCalled(e);
                    }
                    catch (Exception e) { Logger.Error(e); }
                });
            }
            return null;
        }

        private static Task client_Zombied(DiscordClient sender, ZombiedEventArgs e)
        {
            foreach (var item in PluginLoader.ZombiedEvents)
            {
                Task.Factory.StartNew(() =>
                {
                    try
                    {
                        item.OnCalled(e);
                    }
                    catch (Exception e) { Logger.Error(e); }
                });
            }
            return null;
        }

        private static Task client_Heartbeated(DiscordClient sender, HeartbeatEventArgs e)
        {
            foreach (var item in PluginLoader.HeartbeatedEvents)
            {
                Task.Factory.StartNew(() =>
                {
                    try
                    {
                        item.OnCalled(e);
                    }
                    catch (Exception e) { Logger.Error(e); }
                });
            }
            return null;
        }

        private static Task client_Resumed(DiscordClient sender, ReadyEventArgs e)
        {
            foreach (var item in PluginLoader.ResumedEvents)
            {
                Task.Factory.StartNew(() =>
                {
                    try
                    {
                        item.OnCalled(e);
                    }
                    catch (Exception e) { Logger.Error(e); }
                });
            }
            return null;
        }

        private static Task client_Ready(DiscordClient sender, ReadyEventArgs e)
        {
            foreach (var item in PluginLoader.ReadyEvents)
            {
                Task.Factory.StartNew(() =>
                {
                    try
                    {
                        item.OnCalled(e);
                    }
                    catch (Exception e) { Logger.Error(e); }
                });
            }
            Logger.WriteLine($"{{green}}{(_client.CurrentUser != null ? _client.CurrentUser.Username + "#" + _client.CurrentUser.Discriminator : "{blue}Unknown{end}{red}")} is Ready{{end}}");
            return null;
        }

        private static Task client_SocketClosed(DiscordClient sender, SocketCloseEventArgs e)
        {
            foreach (var item in PluginLoader.SocketClosedEvents)
            {
                Task.Factory.StartNew(() =>
                {
                    try
                    {
                        item.OnCalled(e);
                    }
                    catch (Exception e) { Logger.Error(e); }
                });
            }
            return null;
        }

        private static Task client_SocketOpened(DiscordClient sender, SocketEventArgs e)
        {
            foreach (var item in PluginLoader.SocketOpenedEvents)
            {
                Task.Factory.StartNew(() =>
                {
                    try
                    {
                        item.OnCalled(e);
                    }
                    catch (Exception e) { Logger.Error(e); }
                });
            }
            return null;
        }

        private static Task client_SocketErrored(DiscordClient sender, SocketErrorEventArgs e)
        {
            foreach (var item in PluginLoader.SocketErroredEvents)
            {
                Task.Factory.StartNew(() =>
                {
                    try
                    {
                        item.OnCalled(e);
                    }
                    catch (Exception e) { Logger.Error(e); }
                });
            }
            return null;
        }
        #endregion
    }
}

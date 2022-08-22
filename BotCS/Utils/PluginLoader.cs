using PluginCS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using PluginCS.Plugins;
using PluginCS.Events;
using System.Reflection.Emit;
using System.Diagnostics;

namespace BotCS.Utils
{
    internal static class PluginLoader
    {
        #region Plugin Lists
        [DebuggerHidden]
        public static List<ClientPlugin> Plugins { get; } = new List<ClientPlugin>();

        [DebuggerHidden]
        public static List<IConsolePlugin> ConsolePlugins { get; } = new List<IConsolePlugin>();

        [DebuggerHidden]
        public static List<ClientCommand> Commands { get; } = new List<ClientCommand>();
        #endregion

        #region Event Lists
        public static List<IApplicationCommandCreated> ApplicationCommandCreatedEvents { get; } = new List<IApplicationCommandCreated>();
        public static List<IApplicationCommandDeleted> ApplicationCommandDeletedEvents { get; } = new List<IApplicationCommandDeleted>();
        public static List<IApplicationCommandUpdated> ApplicationCommandUpdatedEvents { get; } = new List<IApplicationCommandUpdated>();
        public static List<IChannelCreated> ChannelCreatedEvents { get; } = new List<IChannelCreated>();
        public static List<IChannelDeleted> ChannelDeletedEvents { get; } = new List<IChannelDeleted>();
        public static List<IChannelPinsUpdated> ChannelPinsUpdatedEvents { get; } = new List<IChannelPinsUpdated>();
        public static List<IChannelUpdated> ChannelUpdatedEvents { get; } = new List<IChannelUpdated>();
        public static List<IClientErrored> ClientErroredEvents { get; } = new List<IClientErrored>();
        public static List<IComponentInteractionCreated> ComponentInteractionCreatedEvents { get; } = new List<IComponentInteractionCreated>();
        public static List<IContextMenuInteractionCreated> ContextMenuInteractionCreatedEvents { get; } = new List<IContextMenuInteractionCreated>();
        public static List<IDmChannelDeleted> DmChannelDeletedEvents { get; } = new List<IDmChannelDeleted>();
        public static List<IGuildAvailable> GuildAvailableEvents { get; } = new List<IGuildAvailable>();
        public static List<IGuildBanAdded> GuildBanAddedEvents { get; } = new List<IGuildBanAdded>();
        public static List<IGuildBanRemoved> GuildBanRemovedEvents { get; } = new List<IGuildBanRemoved>();
        public static List<IGuildCreated> GuildCreatedEvents { get; } = new List<IGuildCreated>();
        public static List<IGuildDeleted> GuildDeletedEvents { get; } = new List<IGuildDeleted>();
        public static List<IGuildDownloadCompleted> GuildDownloadCompletedEvents { get; } = new List<IGuildDownloadCompleted>();
        public static List<IGuildEmojisUpdated> GuildEmojisUpdatedEvents { get; } = new List<IGuildEmojisUpdated>();
        public static List<IGuildIntegrationsUpdated> GuildIntegrationsUpdatedEvents { get; } = new List<IGuildIntegrationsUpdated>();
        public static List<IGuildMemberAdded> GuildMemberAddedEvents { get; } = new List<IGuildMemberAdded>();
        public static List<IGuildMemberRemoved> GuildMemberRemovedEvents { get; } = new List<IGuildMemberRemoved>();
        public static List<IGuildMembersChunked> GuildMembersChunkedEvents { get; } = new List<IGuildMembersChunked>();
        public static List<IGuildMemberUpdated> GuildMemberUpdatedEvents { get; } = new List<IGuildMemberUpdated>();
        public static List<IGuildRoleCreated> GuildRoleCreatedEvents { get; } = new List<IGuildRoleCreated>();
        public static List<IGuildRoleDeleted> GuildRoleDeletedEvents { get; } = new List<IGuildRoleDeleted>();
        public static List<IGuildRoleUpdated> GuildRoleUpdatedEvents { get; } = new List<IGuildRoleUpdated>();
        public static List<IGuildStickersUpdated> GuildStickersUpdatedEvents { get; } = new List<IGuildStickersUpdated>();
        public static List<IGuildUnavailable> GuildUnavailableEvents { get; } = new List<IGuildUnavailable>();
        public static List<IGuildUpdated> GuildUpdatedEvents { get; } = new List<IGuildUpdated>();
        public static List<IHeartbeated> HeartbeatedEvents { get; } = new List<IHeartbeated>();
        public static List<IIntegrationCreated> IntegrationCreatedEvents { get; } = new List<IIntegrationCreated>();
        public static List<IIntegrationDeleted> IntegrationDeletedEvents { get; } = new List<IIntegrationDeleted>();
        public static List<IIntegrationUpdated> IntegrationUpdatedEvents { get; } = new List<IIntegrationUpdated>();
        public static List<IInteractionCreated> InteractionCreatedEvents { get; } = new List<IInteractionCreated>();
        public static List<IInviteCreated> InviteCreatedEvents { get; } = new List<IInviteCreated>();
        public static List<IInviteDeleted> InviteDeletedEvents { get; } = new List<IInviteDeleted>();
        public static List<IMessageAcknowledged> MessageAcknowledgedEvents { get; } = new List<IMessageAcknowledged>();
        public static List<IMessageCreated> MessageCreatedEvents { get; } = new List<IMessageCreated>();
        public static List<IMessageDeleted> MessageDeletedEvents { get; } = new List<IMessageDeleted>();
        public static List<IMessageReactionAdded> MessageReactionAddedEvents { get; } = new List<IMessageReactionAdded>();
        public static List<IMessageReactionRemoved> MessageReactionRemovedEvents { get; } = new List<IMessageReactionRemoved>();
        public static List<IMessageReactionRemovedEmoji> MessageReactionRemovedEmojiEvents { get; } = new List<IMessageReactionRemovedEmoji>();
        public static List<IMessageReactionsCleared> MessageReactionsClearedEvents { get; } = new List<IMessageReactionsCleared>();
        public static List<IMessagesBulkDeleted> MessagesBulkDeletedEvents { get; } = new List<IMessagesBulkDeleted>();
        public static List<IMessageUpdated> MessageUpdatedEvents { get; } = new List<IMessageUpdated>();
        public static List<IModalSubmitted> ModalSubmittedEvents { get; } = new List<IModalSubmitted>();
        public static List<IPresenceUpdated> PresenceUpdatedEvents { get; } = new List<IPresenceUpdated>();
        public static List<IReady> ReadyEvents { get; } = new List<IReady>();
        public static List<IResumed> ResumedEvents { get; } = new List<IResumed>();
        public static List<IScheduledGuildEventCompleted> ScheduledGuildEventCompletedEvents { get; } = new List<IScheduledGuildEventCompleted>();
        public static List<IScheduledGuildEventCreated> ScheduledGuildEventCreatedEvents { get; } = new List<IScheduledGuildEventCreated>();
        public static List<IScheduledGuildEventDeleted> ScheduledGuildEventDeletedEvents { get; } = new List<IScheduledGuildEventDeleted>();
        public static List<IScheduledGuildEventUpdated> ScheduledGuildEventUpdatedEvents { get; } = new List<IScheduledGuildEventUpdated>();
        public static List<IScheduledGuildEventUserAdded> ScheduledGuildEventUserAddedEvents { get; } = new List<IScheduledGuildEventUserAdded>();
        public static List<IScheduledGuildEventUserRemoved> ScheduledGuildEventUserRemovedEvents { get; } = new List<IScheduledGuildEventUserRemoved>();
        public static List<ISocketClosed> SocketClosedEvents { get; } = new List<ISocketClosed>();
        public static List<ISocketErrored> SocketErroredEvents { get; } = new List<ISocketErrored>();
        public static List<ISocketOpened> SocketOpenedEvents { get; } = new List<ISocketOpened>();
        public static List<IStageInstanceCreated> StageInstanceCreatedEvents { get; } = new List<IStageInstanceCreated>();
        public static List<IStageInstanceDeleted> StageInstanceDeletedEvents { get; } = new List<IStageInstanceDeleted>();
        public static List<IStageInstanceUpdated> StageInstanceUpdatedEvents { get; } = new List<IStageInstanceUpdated>();
        public static List<IThreadCreated> ThreadCreatedEvents { get; } = new List<IThreadCreated>();
        public static List<IThreadDeleted> ThreadDeletedEvents { get; } = new List<IThreadDeleted>();
        public static List<IThreadListSynced> ThreadListSyncedEvents { get; } = new List<IThreadListSynced>();
        public static List<IThreadMembersUpdated> ThreadMembersUpdatedEvents { get; } = new List<IThreadMembersUpdated>();
        public static List<IThreadMemberUpdated> ThreadMemberUpdatedEvents { get; } = new List<IThreadMemberUpdated>();
        public static List<IThreadUpdated> ThreadUpdatedEvents { get; } = new List<IThreadUpdated>();
        public static List<ITypingStarted> TypingStartedEvents { get; } = new List<ITypingStarted>();
        public static List<IUnknownEvent> UnknownEventEvents { get; } = new List<IUnknownEvent>();
        public static List<IUserSettingsUpdated> UserSettingsUpdatedEvents { get; } = new List<IUserSettingsUpdated>();
        public static List<IUserUpdated> UserUpdatedEvents { get; } = new List<IUserUpdated>();
        public static List<IVoiceServerUpdated> VoiceServerUpdatedEvents { get; } = new List<IVoiceServerUpdated>();
        public static List<IVoiceStateUpdated> VoiceStateUpdatedEvents { get; } = new List<IVoiceStateUpdated>();
        public static List<IWebhooksUpdated> WebhooksUpdatedEvents { get; } = new List<IWebhooksUpdated>();
        public static List<IZombied> ZombiedEvents { get; } = new List<IZombied>();
        #endregion

        public static void Init()
        {
            #region Clears
            foreach (var prop in typeof(PluginLoader).GetProperties().Where(p => p.Name.StartsWith("List") && p.Name != "Commands"))
            {
                dynamic list = prop.GetValue(null);
                list.Clear();
            }
            #endregion

            hasFiles();
            Logger.WriteLine("{green}Plugin loads started");
            try
            {
                LoadDll(Assembly.GetAssembly(typeof(Program)), false);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }

            foreach (var dllPath in Directory.GetFiles(Environment.CurrentDirectory + ".\\Plugins", "*.dll"))
            {
                try
                {
                    Assembly dll = Assembly.LoadFile(dllPath);
                    if (dll != null)
                    {
                        LoadDll(dll);
                    }
                }
                catch (Exception e)
                {
                    Logger.Error(e);
                }
            }
            Logger.WriteLine("{green}Plugin pre-load finished");
            RefreshCommands();
        }

        public static void RefreshCommands()
        {
            Commands.Clear();

            hasFiles();
            try
            {
                privateRefreshCommands(Assembly.GetAssembly(typeof(Program)), false);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            foreach (var dllPath in Directory.GetFiles(Environment.CurrentDirectory + ".\\Plugins", "*.dll"))
            {
                try
                {
                    Assembly dll = Assembly.LoadFile(dllPath);
                    if (dll != null)
                    {
                        privateRefreshCommands(dll);
                    }
                }
                catch (Exception e)
                {
                    Logger.Error(e);
                }
            }
            Bot.SetCommands(Commands.Select(cmd =>cmd.Command).ToList());
        }

        private static void privateRefreshCommands(Assembly dll, bool security = true)
        {
            foreach (Type _class in dll.GetExportedTypes())
            {
                if (security)
                {
                    if (_class.Namespace.Split('.')[0].ToLower() == "botcs")
                    {
                        Logger.Error("Suspicious Command. File Name : " + dll.ManifestModule.Name);
                        return;
                    }
                }

                try
                {
                    if (_class.GetInterface("ICommand") != null)
                    {
                        ICommand tempICommand = (ICommand)Activator.CreateInstance(_class);

                        ClientCommand temp = new ClientCommand()
                        {
                            CommandID = _class.GUID.ToString(),
                            Command = tempICommand
                        };

                        Commands.Add(temp);
                        try
                        {
                            tempICommand.OnLoad();
                        }
                        catch (NotImplementedException) { }
                        string commandName = "{blue}Unknown{end}";
                        string authorName = "{blue}Unknown{end}";
                        try
                        {
                            commandName = tempICommand.Name;
                        }
                        catch (NotImplementedException) { }
                        try
                        {
                            authorName = tempICommand.AuthorName;
                        }
                        catch (NotImplementedException) { }
                        Logger.WriteLine($"{{cyan}}{commandName}{{green}} Client Command Loaded. {{yellow2}}Developed by {authorName}.");
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error(ex);
                }
            }
        }

        private static void LoadDll(Assembly dll, bool security = true)
        {
            foreach (Type _class in dll.GetExportedTypes())
            {
                if (security)
                {
                    if (_class.Namespace.Split('.')[0].ToLower() == "botcs")
                    {
                        Logger.Error("Suspicious Plugin. Plugin File Name : " + dll.ManifestModule.Name);
                        return;
                    }
                }

                try
                {
                    ClientPlugin plugin = new ClientPlugin();
                    if (_class.GetInterface("IPlugin") != null)
                    {
                        plugin = Plugins.Find(plu => plu.PluginDllName == dll.ManifestModule.Name);

                        if (plugin == null)
                        {
                            plugin = new ClientPlugin();
                            Plugins.Add(plugin);
                        }

                        plugin.PluginID = _class.GUID.ToString();
                        plugin.PluginDllName = dll.ManifestModule.Name;
                        plugin.Plugin = (IPlugin)Activator.CreateInstance(_class);

                    }
                    else if (_class.GetInterface("IConsolePlugin") != null)
                    {
                        IConsolePlugin temp = (IConsolePlugin)Activator.CreateInstance(_class);
                        if (!temp.ClientEnabled)
                        {
                            try
                            {
                                temp.OnLoad(null);
                            }
                            catch (NotImplementedException) { }
                            string pluginName = "{blue}Unknown{end}";
                            string authorName = "{blue}Unknown{end}";
                            try
                            {
                                pluginName = temp.Name;
                            }
                            catch (Exception) { }
                            try
                            {
                                authorName = temp.AuthorName;
                            }
                            catch (Exception) { }
                            Logger.WriteLine($"{{cyan}}{pluginName}{{green}} Plugin Loaded. {{yellow2}}Developed by {authorName}.");
                        }
                        ConsolePlugins.Add(temp);
                    }
                    else
                    {
                        plugin = Plugins.Find(plu => plu.PluginDllName == dll.ManifestModule.Name);
                        if (plugin == null)
                        {
                            plugin = new ClientPlugin();
                            plugin.PluginDllName = dll.ManifestModule.Name;
                            Plugins.Add(plugin);
                        }

                        if (_class.GetInterface("IApplicationCommandCreated") != null)
                        {
                            IApplicationCommandCreated temp = (IApplicationCommandCreated)Activator.CreateInstance(_class);
                            ApplicationCommandCreatedEvents.Add(temp);
                            plugin.Events.Add("IApplicationCommandCreated");
                        }
                        if (_class.GetInterface("IApplicationCommandDeleted") != null)
                        {
                            IApplicationCommandDeleted temp = (IApplicationCommandDeleted)Activator.CreateInstance(_class);
                            ApplicationCommandDeletedEvents.Add(temp);
                            plugin.Events.Add("IApplicationCommandDeleted");
                        }
                        if (_class.GetInterface("IApplicationCommandUpdated") != null)
                        {
                            IApplicationCommandUpdated temp = (IApplicationCommandUpdated)Activator.CreateInstance(_class);
                            ApplicationCommandUpdatedEvents.Add(temp);
                            plugin.Events.Add("IApplicationCommandUpdated");
                        }
                        if (_class.GetInterface("IChannelCreated") != null)
                        {
                            IChannelCreated temp = (IChannelCreated)Activator.CreateInstance(_class);
                            ChannelCreatedEvents.Add(temp);
                            plugin.Events.Add("IChannelCreated");
                        }
                        if (_class.GetInterface("IChannelDeleted") != null)
                        {
                            IChannelDeleted temp = (IChannelDeleted)Activator.CreateInstance(_class);
                            ChannelDeletedEvents.Add(temp);
                            plugin.Events.Add("IChannelDeleted");
                        }
                        if (_class.GetInterface("IChannelPinsUpdated") != null)
                        {
                            IChannelPinsUpdated temp = (IChannelPinsUpdated)Activator.CreateInstance(_class);
                            ChannelPinsUpdatedEvents.Add(temp);
                            plugin.Events.Add("IChannelPinsUpdated");
                        }
                        if (_class.GetInterface("IChannelUpdated") != null)
                        {
                            IChannelUpdated temp = (IChannelUpdated)Activator.CreateInstance(_class);
                            ChannelUpdatedEvents.Add(temp);
                            plugin.Events.Add("IChannelUpdated");
                        }
                        if (_class.GetInterface("IClientErrored") != null)
                        {
                            IClientErrored temp = (IClientErrored)Activator.CreateInstance(_class);
                            ClientErroredEvents.Add(temp);
                            plugin.Events.Add("IClientErrored");
                        }
                        if (_class.GetInterface("IComponentInteractionCreated") != null)
                        {
                            IComponentInteractionCreated temp = (IComponentInteractionCreated)Activator.CreateInstance(_class);
                            ComponentInteractionCreatedEvents.Add(temp);
                            plugin.Events.Add("IComponentInteractionCreated");
                        }
                        if (_class.GetInterface("IContextMenuInteractionCreated") != null)
                        {
                            IContextMenuInteractionCreated temp = (IContextMenuInteractionCreated)Activator.CreateInstance(_class);
                            ContextMenuInteractionCreatedEvents.Add(temp);
                            plugin.Events.Add("IContextMenuInteractionCreated");
                        }
                        if (_class.GetInterface("IDmChannelDeleted") != null)
                        {
                            IDmChannelDeleted temp = (IDmChannelDeleted)Activator.CreateInstance(_class);
                            DmChannelDeletedEvents.Add(temp);
                            plugin.Events.Add("IDmChannelDeleted");
                        }
                        if (_class.GetInterface("IGuildAvailable") != null)
                        {
                            IGuildAvailable temp = (IGuildAvailable)Activator.CreateInstance(_class);
                            GuildAvailableEvents.Add(temp);
                            plugin.Events.Add("IGuildAvailable");
                        }
                        if (_class.GetInterface("IGuildBanAdded") != null)
                        {
                            IGuildBanAdded temp = (IGuildBanAdded)Activator.CreateInstance(_class);
                            GuildBanAddedEvents.Add(temp);
                            plugin.Events.Add("IGuildBanAdded");
                        }
                        if (_class.GetInterface("IGuildBanRemoved") != null)
                        {
                            IGuildBanRemoved temp = (IGuildBanRemoved)Activator.CreateInstance(_class);
                            GuildBanRemovedEvents.Add(temp);
                            plugin.Events.Add("IGuildBanRemoved");
                        }
                        if (_class.GetInterface("IGuildCreated") != null)
                        {
                            IGuildCreated temp = (IGuildCreated)Activator.CreateInstance(_class);
                            GuildCreatedEvents.Add(temp);
                            plugin.Events.Add("IGuildCreated");
                        }
                        if (_class.GetInterface("IGuildDeleted") != null)
                        {
                            IGuildDeleted temp = (IGuildDeleted)Activator.CreateInstance(_class);
                            GuildDeletedEvents.Add(temp);
                            plugin.Events.Add("IGuildDeleted");
                        }
                        if (_class.GetInterface("IGuildDownloadCompleted") != null)
                        {
                            IGuildDownloadCompleted temp = (IGuildDownloadCompleted)Activator.CreateInstance(_class);
                            GuildDownloadCompletedEvents.Add(temp);
                            plugin.Events.Add("IGuildDownloadCompleted");
                        }
                        if (_class.GetInterface("IGuildEmojisUpdated") != null)
                        {
                            IGuildEmojisUpdated temp = (IGuildEmojisUpdated)Activator.CreateInstance(_class);
                            GuildEmojisUpdatedEvents.Add(temp);
                            plugin.Events.Add("IGuildEmojisUpdated");
                        }
                        if (_class.GetInterface("IGuildIntegrationsUpdated") != null)
                        {
                            IGuildIntegrationsUpdated temp = (IGuildIntegrationsUpdated)Activator.CreateInstance(_class);
                            GuildIntegrationsUpdatedEvents.Add(temp);
                            plugin.Events.Add("IGuildIntegrationsUpdated");
                        }
                        if (_class.GetInterface("IGuildMemberAdded") != null)
                        {
                            IGuildMemberAdded temp = (IGuildMemberAdded)Activator.CreateInstance(_class);
                            GuildMemberAddedEvents.Add(temp);
                            plugin.Events.Add("IGuildMemberAdded");
                        }
                        if (_class.GetInterface("IGuildMemberRemoved") != null)
                        {
                            IGuildMemberRemoved temp = (IGuildMemberRemoved)Activator.CreateInstance(_class);
                            GuildMemberRemovedEvents.Add(temp);
                            plugin.Events.Add("IGuildMemberRemoved");
                        }
                        if (_class.GetInterface("IGuildMembersChunked") != null)
                        {
                            IGuildMembersChunked temp = (IGuildMembersChunked)Activator.CreateInstance(_class);
                            GuildMembersChunkedEvents.Add(temp);
                            plugin.Events.Add("IGuildMembersChunked");
                        }
                        if (_class.GetInterface("IGuildMemberUpdated") != null)
                        {
                            IGuildMemberUpdated temp = (IGuildMemberUpdated)Activator.CreateInstance(_class);
                            GuildMemberUpdatedEvents.Add(temp);
                            plugin.Events.Add("IGuildMemberUpdated");
                        }
                        if (_class.GetInterface("IGuildRoleCreated") != null)
                        {
                            IGuildRoleCreated temp = (IGuildRoleCreated)Activator.CreateInstance(_class);
                            GuildRoleCreatedEvents.Add(temp);
                            plugin.Events.Add("IGuildRoleCreated");
                        }
                        if (_class.GetInterface("IGuildRoleDeleted") != null)
                        {
                            IGuildRoleDeleted temp = (IGuildRoleDeleted)Activator.CreateInstance(_class);
                            GuildRoleDeletedEvents.Add(temp);
                            plugin.Events.Add("IGuildRoleDeleted");
                        }
                        if (_class.GetInterface("IGuildRoleUpdated") != null)
                        {
                            IGuildRoleUpdated temp = (IGuildRoleUpdated)Activator.CreateInstance(_class);
                            GuildRoleUpdatedEvents.Add(temp);
                            plugin.Events.Add("IGuildRoleUpdated");
                        }
                        if (_class.GetInterface("IGuildStickersUpdated") != null)
                        {
                            IGuildStickersUpdated temp = (IGuildStickersUpdated)Activator.CreateInstance(_class);
                            GuildStickersUpdatedEvents.Add(temp);
                            plugin.Events.Add("IGuildStickersUpdated");
                        }
                        if (_class.GetInterface("IGuildUnavailable") != null)
                        {
                            IGuildUnavailable temp = (IGuildUnavailable)Activator.CreateInstance(_class);
                            GuildUnavailableEvents.Add(temp);
                            plugin.Events.Add("IGuildUnavailable");
                        }
                        if (_class.GetInterface("IGuildUpdated") != null)
                        {
                            IGuildUpdated temp = (IGuildUpdated)Activator.CreateInstance(_class);
                            GuildUpdatedEvents.Add(temp);
                            plugin.Events.Add("IGuildUpdated");
                        }
                        if (_class.GetInterface("IHeartbeated") != null)
                        {
                            IHeartbeated temp = (IHeartbeated)Activator.CreateInstance(_class);
                            HeartbeatedEvents.Add(temp);
                            plugin.Events.Add("IHeartbeated");
                        }
                        if (_class.GetInterface("IIntegrationCreated") != null)
                        {
                            IIntegrationCreated temp = (IIntegrationCreated)Activator.CreateInstance(_class);
                            IntegrationCreatedEvents.Add(temp);
                            plugin.Events.Add("IIntegrationCreated");
                        }
                        if (_class.GetInterface("IIntegrationDeleted") != null)
                        {
                            IIntegrationDeleted temp = (IIntegrationDeleted)Activator.CreateInstance(_class);
                            IntegrationDeletedEvents.Add(temp);
                            plugin.Events.Add("IIntegrationDeleted");
                        }
                        if (_class.GetInterface("IIntegrationUpdated") != null)
                        {
                            IIntegrationUpdated temp = (IIntegrationUpdated)Activator.CreateInstance(_class);
                            IntegrationUpdatedEvents.Add(temp);
                            plugin.Events.Add("IIntegrationUpdated");
                        }
                        if (_class.GetInterface("IInteractionCreated") != null)
                        {
                            IInteractionCreated temp = (IInteractionCreated)Activator.CreateInstance(_class);
                            InteractionCreatedEvents.Add(temp);
                            plugin.Events.Add("IInteractionCreated");
                        }
                        if (_class.GetInterface("IInviteCreated") != null)
                        {
                            IInviteCreated temp = (IInviteCreated)Activator.CreateInstance(_class);
                            InviteCreatedEvents.Add(temp);
                            plugin.Events.Add("IInviteCreated");
                        }
                        if (_class.GetInterface("IInviteDeleted") != null)
                        {
                            IInviteDeleted temp = (IInviteDeleted)Activator.CreateInstance(_class);
                            InviteDeletedEvents.Add(temp);
                            plugin.Events.Add("IInviteDeleted");
                        }
                        if (_class.GetInterface("IMessageAcknowledged") != null)
                        {
                            IMessageAcknowledged temp = (IMessageAcknowledged)Activator.CreateInstance(_class);
                            MessageAcknowledgedEvents.Add(temp);
                            plugin.Events.Add("IMessageAcknowledged");
                        }
                        if (_class.GetInterface("IMessageCreated") != null)
                        {
                            IMessageCreated temp = (IMessageCreated)Activator.CreateInstance(_class);
                            MessageCreatedEvents.Add(temp);
                            plugin.Events.Add("IMessageCreated");
                        }
                        if (_class.GetInterface("IMessageDeleted") != null)
                        {
                            IMessageDeleted temp = (IMessageDeleted)Activator.CreateInstance(_class);
                            MessageDeletedEvents.Add(temp);
                            plugin.Events.Add("IMessageDeleted");
                        }
                        if (_class.GetInterface("IMessageReactionAdded") != null)
                        {
                            IMessageReactionAdded temp = (IMessageReactionAdded)Activator.CreateInstance(_class);
                            MessageReactionAddedEvents.Add(temp);
                            plugin.Events.Add("IMessageReactionAdded");
                        }
                        if (_class.GetInterface("IMessageReactionRemoved") != null)
                        {
                            IMessageReactionRemoved temp = (IMessageReactionRemoved)Activator.CreateInstance(_class);
                            MessageReactionRemovedEvents.Add(temp);
                            plugin.Events.Add("IMessageReactionRemoved");
                        }
                        if (_class.GetInterface("IMessageReactionRemovedEmoji") != null)
                        {
                            IMessageReactionRemovedEmoji temp = (IMessageReactionRemovedEmoji)Activator.CreateInstance(_class);
                            MessageReactionRemovedEmojiEvents.Add(temp);
                            plugin.Events.Add("IMessageReactionRemovedEmoji");
                        }
                        if (_class.GetInterface("IMessageReactionsCleared") != null)
                        {
                            IMessageReactionsCleared temp = (IMessageReactionsCleared)Activator.CreateInstance(_class);
                            MessageReactionsClearedEvents.Add(temp);
                            plugin.Events.Add("IMessageReactionsCleared");
                        }
                        if (_class.GetInterface("IMessagesBulkDeleted") != null)
                        {
                            IMessagesBulkDeleted temp = (IMessagesBulkDeleted)Activator.CreateInstance(_class);
                            MessagesBulkDeletedEvents.Add(temp);
                            plugin.Events.Add("IMessagesBulkDeleted");
                        }
                        if (_class.GetInterface("IMessageUpdated") != null)
                        {
                            IMessageUpdated temp = (IMessageUpdated)Activator.CreateInstance(_class);
                            MessageUpdatedEvents.Add(temp);
                            plugin.Events.Add("IMessageUpdated");
                        }
                        if (_class.GetInterface("IModalSubmitted") != null)
                        {
                            IModalSubmitted temp = (IModalSubmitted)Activator.CreateInstance(_class);
                            ModalSubmittedEvents.Add(temp);
                            plugin.Events.Add("IModalSubmitted");
                        }
                        if (_class.GetInterface("IPresenceUpdated") != null)
                        {
                            IPresenceUpdated temp = (IPresenceUpdated)Activator.CreateInstance(_class);
                            PresenceUpdatedEvents.Add(temp);
                            plugin.Events.Add("IPresenceUpdated");
                        }
                        if (_class.GetInterface("IReady") != null)
                        {
                            IReady temp = (IReady)Activator.CreateInstance(_class);
                            ReadyEvents.Add(temp);
                            plugin.Events.Add("IReady");
                        }
                        if (_class.GetInterface("IResumed") != null)
                        {
                            IResumed temp = (IResumed)Activator.CreateInstance(_class);
                            ResumedEvents.Add(temp);
                            plugin.Events.Add("IResumed");
                        }
                        if (_class.GetInterface("IScheduledGuildEventCompleted") != null)
                        {
                            IScheduledGuildEventCompleted temp = (IScheduledGuildEventCompleted)Activator.CreateInstance(_class);
                            ScheduledGuildEventCompletedEvents.Add(temp);
                            plugin.Events.Add("IScheduledGuildEventCompleted");
                        }
                        if (_class.GetInterface("IScheduledGuildEventCreated") != null)
                        {
                            IScheduledGuildEventCreated temp = (IScheduledGuildEventCreated)Activator.CreateInstance(_class);
                            ScheduledGuildEventCreatedEvents.Add(temp);
                            plugin.Events.Add("IScheduledGuildEventCreated");
                        }
                        if (_class.GetInterface("IScheduledGuildEventDeleted") != null)
                        {
                            IScheduledGuildEventDeleted temp = (IScheduledGuildEventDeleted)Activator.CreateInstance(_class);
                            ScheduledGuildEventDeletedEvents.Add(temp);
                            plugin.Events.Add("IScheduledGuildEventDeleted");
                        }
                        if (_class.GetInterface("IScheduledGuildEventUpdated") != null)
                        {
                            IScheduledGuildEventUpdated temp = (IScheduledGuildEventUpdated)Activator.CreateInstance(_class);
                            ScheduledGuildEventUpdatedEvents.Add(temp);
                            plugin.Events.Add("IScheduledGuildEventUpdated");
                        }
                        if (_class.GetInterface("IScheduledGuildEventUserAdded") != null)
                        {
                            IScheduledGuildEventUserAdded temp = (IScheduledGuildEventUserAdded)Activator.CreateInstance(_class);
                            ScheduledGuildEventUserAddedEvents.Add(temp);
                            plugin.Events.Add("IScheduledGuildEventUserAdded");
                        }
                        if (_class.GetInterface("IScheduledGuildEventUserRemoved") != null)
                        {
                            IScheduledGuildEventUserRemoved temp = (IScheduledGuildEventUserRemoved)Activator.CreateInstance(_class);
                            ScheduledGuildEventUserRemovedEvents.Add(temp);
                            plugin.Events.Add("IScheduledGuildEventUserRemoved");
                        }
                        if (_class.GetInterface("ISocketClosed") != null)
                        {
                            ISocketClosed temp = (ISocketClosed)Activator.CreateInstance(_class);
                            SocketClosedEvents.Add(temp);
                            plugin.Events.Add("ISocketClosed");
                        }
                        if (_class.GetInterface("ISocketErrored") != null)
                        {
                            ISocketErrored temp = (ISocketErrored)Activator.CreateInstance(_class);
                            SocketErroredEvents.Add(temp);
                            plugin.Events.Add("ISocketErrored");
                        }
                        if (_class.GetInterface("ISocketOpened") != null)
                        {
                            ISocketOpened temp = (ISocketOpened)Activator.CreateInstance(_class);
                            SocketOpenedEvents.Add(temp);
                            plugin.Events.Add("ISocketOpened");
                        }
                        if (_class.GetInterface("IStageInstanceCreated") != null)
                        {
                            IStageInstanceCreated temp = (IStageInstanceCreated)Activator.CreateInstance(_class);
                            StageInstanceCreatedEvents.Add(temp);
                            plugin.Events.Add("IStageInstanceCreated");
                        }
                        if (_class.GetInterface("IStageInstanceDeleted") != null)
                        {
                            IStageInstanceDeleted temp = (IStageInstanceDeleted)Activator.CreateInstance(_class);
                            StageInstanceDeletedEvents.Add(temp);
                            plugin.Events.Add("IStageInstanceDeleted");
                        }
                        if (_class.GetInterface("IStageInstanceUpdated") != null)
                        {
                            IStageInstanceUpdated temp = (IStageInstanceUpdated)Activator.CreateInstance(_class);
                            StageInstanceUpdatedEvents.Add(temp);
                            plugin.Events.Add("IStageInstanceUpdated");
                        }
                        if (_class.GetInterface("IThreadCreated") != null)
                        {
                            IThreadCreated temp = (IThreadCreated)Activator.CreateInstance(_class);
                            ThreadCreatedEvents.Add(temp);
                            plugin.Events.Add("IThreadCreated");
                        }
                        if (_class.GetInterface("IThreadDeleted") != null)
                        {
                            IThreadDeleted temp = (IThreadDeleted)Activator.CreateInstance(_class);
                            ThreadDeletedEvents.Add(temp);
                            plugin.Events.Add("IThreadDeleted");
                        }
                        if (_class.GetInterface("IThreadListSynced") != null)
                        {
                            IThreadListSynced temp = (IThreadListSynced)Activator.CreateInstance(_class);
                            ThreadListSyncedEvents.Add(temp);
                            plugin.Events.Add("IThreadListSynced");
                        }
                        if (_class.GetInterface("IThreadMembersUpdated") != null)
                        {
                            IThreadMembersUpdated temp = (IThreadMembersUpdated)Activator.CreateInstance(_class);
                            ThreadMembersUpdatedEvents.Add(temp);
                            plugin.Events.Add("IThreadMembersUpdated");
                        }
                        if (_class.GetInterface("IThreadMemberUpdated") != null)
                        {
                            IThreadMemberUpdated temp = (IThreadMemberUpdated)Activator.CreateInstance(_class);
                            ThreadMemberUpdatedEvents.Add(temp);
                            plugin.Events.Add("IThreadMemberUpdated");
                        }
                        if (_class.GetInterface("IThreadUpdated") != null)
                        {
                            IThreadUpdated temp = (IThreadUpdated)Activator.CreateInstance(_class);
                            ThreadUpdatedEvents.Add(temp);
                            plugin.Events.Add("IThreadUpdated");
                        }
                        if (_class.GetInterface("ITypingStarted") != null)
                        {
                            ITypingStarted temp = (ITypingStarted)Activator.CreateInstance(_class);
                            TypingStartedEvents.Add(temp);
                            plugin.Events.Add("ITypingStarted");
                        }
                        if (_class.GetInterface("IUnknownEvent") != null)
                        {
                            IUnknownEvent temp = (IUnknownEvent)Activator.CreateInstance(_class);
                            UnknownEventEvents.Add(temp);
                            plugin.Events.Add("IUnknownEvent");
                        }
                        if (_class.GetInterface("IUserSettingsUpdated") != null)
                        {
                            IUserSettingsUpdated temp = (IUserSettingsUpdated)Activator.CreateInstance(_class);
                            UserSettingsUpdatedEvents.Add(temp);
                            plugin.Events.Add("IUserSettingsUpdated");
                        }
                        if (_class.GetInterface("IUserUpdated") != null)
                        {
                            IUserUpdated temp = (IUserUpdated)Activator.CreateInstance(_class);
                            UserUpdatedEvents.Add(temp);
                            plugin.Events.Add("IUserUpdated");
                        }
                        if (_class.GetInterface("IVoiceServerUpdated") != null)
                        {
                            IVoiceServerUpdated temp = (IVoiceServerUpdated)Activator.CreateInstance(_class);
                            VoiceServerUpdatedEvents.Add(temp);
                            plugin.Events.Add("IVoiceServerUpdated");
                        }
                        if (_class.GetInterface("IVoiceStateUpdated") != null)
                        {
                            IVoiceStateUpdated temp = (IVoiceStateUpdated)Activator.CreateInstance(_class);
                            VoiceStateUpdatedEvents.Add(temp);
                            plugin.Events.Add("IVoiceStateUpdated");
                        }
                        if (_class.GetInterface("IWebhooksUpdated") != null)
                        {
                            IWebhooksUpdated temp = (IWebhooksUpdated)Activator.CreateInstance(_class);
                            WebhooksUpdatedEvents.Add(temp);
                            plugin.Events.Add("IWebhooksUpdated");
                        }
                        if (_class.GetInterface("IZombied") != null)
                        {
                            IZombied temp = (IZombied)Activator.CreateInstance(_class);
                            ZombiedEvents.Add(temp);
                            plugin.Events.Add("IZombied");
                        }
                    }

                }
                catch (Exception e)
                {
                    Logger.Error(e);
                }
            }
        }

        private static void hasFiles()
        {
            if (!Directory.Exists(Environment.CurrentDirectory + ".\\Plugins"))
                Directory.CreateDirectory(Environment.CurrentDirectory + ".\\Plugins");
        }
    }
}

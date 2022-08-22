using BotCS.Utils;
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
    public class Help : IConsolePlugin
    {
        public string Name => "System Help Command";

        public string AuthorName => "BotCS";

        public double Version => 1.0;

        public bool ClientEnabled => false;

        public List<string> Aliases => new List<string>() { "help" };

        public string Description => "It gives information about the commands that can be used on the console.";

        public void OnCalled(string[] args)
        {
            if (args.Length == 0)
            {
                string Output = "{yellow2}For detailed information, you can write \"help -i [PluginName | PluginAlias]\".{end}\n\n";
                foreach (var item in PluginLoader.ConsolePlugins)
                {
                    string Name = "";
                    string Aliases = "";
                    try
                    {
                        Name = item.Name;
                    }
                    catch (Exception) { Name = "Unknown Plugin"; }
                    try
                    {
                        Aliases = string.Join(", ", item.Aliases);
                    }
                    catch (Exception) { Aliases = "Unknown Commands"; }

                    Output += $"{{yellow2}}{Name}{{end}} - Aliases : {{cyan}}{Aliases}{{end}}\n";
                }
                Logger.WriteLine(Output);
            }
            else if (args.Length > 1 && args[0].ToLower() == "-i")
            {
                string search = string.Join(" ", args.Skip(1));
                string Output = "";
                foreach (var plugin in PluginLoader.ConsolePlugins)
                {
                    if (plugin.Name.ToLower() == search.ToLower())
                    {
                        Output = $"(help -i {search.ToLower()}) command invoked\n";
                        Output += getPluginInfo(plugin);
                    }
                    else
                    {
                        try
                        {
                            foreach (var item in plugin.Aliases)
                            {
                                if (item.ToLower() == search.ToLower())
                                {
                                    Output = $"(help -i {search.ToLower()}) command invoked\n";
                                    Output += getPluginInfo(plugin);
                                    break;
                                }
                            }
                        }
                        catch (Exception) { }
                    }
                }

                if (string.IsNullOrEmpty(Output))
                    Logger.WriteLine("{red}Command not found.");
                else
                    Logger.WriteLine(Output);
            }
            else
            {
                Logger.WriteLine("{red}There is no such parameter.");
            }
        }

        private string getPluginInfo(IConsolePlugin plugin)
        {
            string Out = "";

            try
            {
                Out += $"{{yellow2}}Plugin Name {{end}}:{plugin.Name}";
            }
            catch (Exception) 
            {
                Out += $"{{yellow2}}Plugin Name {{end}}:{{blue}}Unknown{{end}}";
            }
            Out += "\n";
            try
            {
                Out += $"{{yellow2}}Plugin Aliases {{end}}:{string.Join(",",plugin.Aliases).Replace(" ","_").Replace(",", ", ")}";
            }
            catch (Exception)
            {
                Out += $"{{yellow2}}Plugin Aliases {{end}}:{{blue}}Unknown{{end}}";
            }
            Out += "\n";
            try
            {
                Out += $"{{yellow2}}Plugin Author Name {{end}}:{plugin.AuthorName}";
            }
            catch (Exception)
            {
                Out += $"{{yellow2}}Plugin Author Name {{end}}:{{blue}}Unknown{{end}}";
            }
            Out += "\n";
            try
            {
                Out += $"{{yellow2}}Plugin Version {{end}}:{plugin.Version:0.0000}";
            }
            catch (Exception)
            {
                Out += $"{{yellow2}}Plugin Version {{end}}:{{blue}}Unknown{{end}}";
            }
            Out += "\n";
            try
            {
                Out += $"{{yellow2}}Plugin Description {{end}}:{plugin.Description}";
            }
            catch (Exception)
            {
                Out += $"{{yellow2}}Plugin Description {{end}}:{{blue}}Unknown{{end}}";
            }

            return Out;
        }

        public void OnLoad(DiscordClient client) { }
    }
}

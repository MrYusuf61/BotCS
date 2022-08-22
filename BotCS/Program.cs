using PluginCS;
using PluginCS.Databases;
using DSharpPlus;
using System;
using System.Diagnostics;
using System.Threading;
using BotCS.Utils;
using System.Collections.Generic;
using System.Linq;
using BotCS.Discord;
using System.Reflection;
using PluginCS.Objects;

namespace BotCS
{
    public static class Program
    {
        internal const double Version = 0.1;

        internal static Thread? readThread;


        public static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            Console.Title = "BotCS V" + Version.ToString("0.0000");
            PluginLoader.Init();
            Client.Init();

            readThread = new Thread(() =>
            {
                try
                {
                    while (true)
                    {
                        string[] _args = (Console.ReadLine() ?? "").Trim().Split(' ');
                        if (_args.Length > 0)
                        {
                            string command = _args[0];
                            if (_args.Length > 1) _args = _args.Skip(1).ToArray();
                            else _args = new string[0];
                            foreach (var consolePlugin in PluginLoader.ConsolePlugins.ToList())
                            {
                                try
                                {
                                    foreach (var item in consolePlugin.Aliases.ToList())
                                        if (string.Join("_", item.ToLower().Split(' ')) == command.ToLower())
                                            consolePlugin.OnCalled(_args);

                                }
                                catch (NotImplementedException) { }
                                catch (Exception ex)
                                {
                                    Logger.Error(ex);
                                }
                            }
                        }
                    }
                }
                catch (Exception) { }
            });
            readThread.Start();
        }

        public static string GetCurrentRam()
        {
            var temp = ((Process.GetCurrentProcess().PrivateMemorySize64) / (float)1024) / (float)1024;
            return $"{temp:0.00MB}";
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
#pragma warning disable CS0618
            Logger.FatalError((Exception)e.ExceptionObject);
#pragma warning restore CS0618

            if (readThread != null)
                readThread.Interrupt();
            Console.ReadLine();
            Environment.Exit(1);
        }
    }
}

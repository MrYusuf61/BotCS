using PluginCS.Databases;
using PluginCS.Plugins;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginCS
{
    public static class Bot
    {
        private static IReadOnlyList<ICommand> botCommands = null;

        [DebuggerHidden]
        public static IReadOnlyList<ICommand> Commands { get => botCommands.ToList(); }

        [DebuggerHidden]
        public static void SetCommands(List<ICommand> commands)
        {
            var frames = new StackTrace(1, true).GetFrames();
            if (frames.FirstOrDefault() is StackFrame frame)
            {
                var t = frame.GetMethod();
                if (!frame.GetMethod().DeclaringType.Namespace.ToLower().StartsWith("botcs"))
                    throw new Exception("This method can only be run by the BotCS application.");
            }
            botCommands = commands;
        }

        public static List<string> Owners
        {
            get
            {
                List<object> list = JsonDatabase.GetList("owners");
                List<string> Out = new();
                
                if (list == null) return Out;

                foreach (var item in list)
                    Out.Add((item ?? "").ToString());

                return Out;
            }
        }
    }
}

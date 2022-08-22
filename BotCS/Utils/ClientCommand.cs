using PluginCS.Databases;
using PluginCS.Plugins;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace BotCS.Utils
{
    internal class ClientCommand
    {
        [DebuggerHidden]
        public string CommandID { get; set; }
        
        [DebuggerHidden]
        public ICommand Command { get; set; }
        
        [DebuggerHidden]
        public bool IsEnabled()
        {
            var id = "DISABLED";
            var disabledList = JsonDatabase.GetList(id);
            return disabledList == null ? true : (disabledList.Find(id => id.ToString() == Helper.GuidToID(CommandID)) == null);
        }
    }
}

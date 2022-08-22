using DSharpPlus;
using PluginCS;
using PluginCS.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BotCS.SystemPlugins
{
    public class TypeInfoCommand : IConsolePlugin
    {
        public string Name => "System Type Command";

        public string AuthorName => "BotCS";

        public double Version => 1.0;

        public string Description => "Returns the information of the given type name.";

        public bool ClientEnabled => false;

        public List<string> Aliases => new() { "type" };

        public void OnCalled(string[] args)
        {
            if (args.Length != 0)
            {
                var type = Type.GetType(args[0]);
                if (type == null)
                {
                    writeParamError();
                    return;
                }
                else
                {
                    if (type.Module.Name == "System.Private.CoreLib.dll")
                    {
                        try
                        {
                            PropertyInfo maxValue = null, minValue = null;
                            FieldInfo empty = null;
                            foreach (var item in ((dynamic)type).DeclaredProperties)
                            {
                                if (item.Name.EndsWith("MaxValue"))
                                    maxValue = item;
                                else if (item.Name.EndsWith("MinValue"))
                                    minValue = item;
                            }

                            foreach (var item in ((dynamic)type).DeclaredFields)
                            {
                                if (item.Name.EndsWith("Empty"))
                                    empty = item;
                            }

                            Logger.WriteLine($@"
{{cyan}}Name{{end}} : {{yellow2}}{args[0]}{{end}}
{{cyan}}Is Value Type{{end}} : {{yellow2}}{type.IsValueType}{{end}}
{{cyan}}Max Value : {{yellow2}}{(maxValue != null ? maxValue.GetValue(null) : "{blue}Unknown{end}")}{{end}}
{{cyan}}Min Value : {{yellow2}}{(minValue != null ? minValue.GetValue(null) : "{blue}Unknown{end}")}{{end}}
{{cyan}}Empty : {{yellow2}}""{(empty != null ? empty.GetValue(null) : "{blue}Unknown{end}")}""{{end}}");

                        }
                        catch (Exception ex)
                        {
                            Logger.Error(ex);
                            return;
                        }
                    }
                }
            }

        }

        private static void writeParamError() => Logger.WriteLine("{red}Invalid type{end}. {red}Please enter a valid type{end}.");

        public void OnLoad(DiscordClient client) { }
    }
}

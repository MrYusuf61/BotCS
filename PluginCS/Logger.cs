using PluginCS.Objects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace PluginCS
{

    public static class Logger
    {
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // The DebuggerHidden attribute prevents unintentional entry into the method. It can work without the attribute if desired.
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// It is used to output console in color.<br/>
        /// 
        /// <b>Color Codes</b><br/>
        ///     {blue} - DarkBlue<br/>
        ///     {red} - DarkRed<br/>
        ///     {green} - Green<br/>
        ///     {yellow} - Yellow<br/>
        ///     {cyan} - DarkCyan<br/>
        ///     {blue2} - Blue<br/>
        ///     {red2} - Red<br/>
        ///     {green2} - DarkGreen<br/>
        ///     {yellow2} - DarkYellow<br/>
        ///     {cyan2} - Cyan<br/>
        ///     {end} - ResetColor<br/><br/>
        /// 
        /// <b>Example</b><br/>
        /// <code>
        /// Logger.Write("{blue}Hello{end} {green}World{end}");
        /// Logger.Write("{red}Hello {cyan}World");
        /// Logger.Write($"{{red}}Hello {{cyan}}World");
        /// </code>
        /// </summary>
        /// <param name="text">Console Output</param>
        [DebuggerHidden]
        public static void WriteLine(string text = "")
        {
            if (text == (string)null)
                Write("{blue}null{end}\n");
            else
                Write(text + "\n");
        }

        /// <summary>
        /// It is used to output console in color.<br/>
        /// 
        /// <b>Color Codes</b><br/>
        ///     {blue} - DarkBlue<br/>
        ///     {red} - DarkRed<br/>
        ///     {green} - Green<br/>
        ///     {yellow} - Yellow<br/>
        ///     {cyan} - DarkCyan<br/>
        ///     {blue2} - Blue<br/>
        ///     {red2} - Red<br/>
        ///     {green2} - DarkGreen<br/>
        ///     {yellow2} - DarkYellow<br/>
        ///     {cyan2} - Cyan<br/>
        ///     {end} - ResetColor<br/><br/>
        /// 
        /// <b>Example</b><br/>
        /// <code>
        /// Logger.Write("{blue}Hello{end} {green}World{end}");
        /// Logger.Write("{red}Hello {cyan}World");
        /// Logger.Write($"{{red}}Hello {{cyan}}World");
        /// </code>
        /// </summary>
        /// <param name="text">Console Output</param>
        [DebuggerHidden]
        public static void WriteLine(object text)
        {
            if (text == null)
                Write("{blue}null{end}\n");
            else
                Write(text.ToString() + "\n");
        }

        /// <summary>
        /// It is used to output console in color.<br/>
        /// 
        /// <b>Color Codes</b><br/>
        ///     {blue} - DarkBlue<br/>
        ///     {red} - DarkRed<br/>
        ///     {green} - Green<br/>
        ///     {yellow} - Yellow<br/>
        ///     {cyan} - DarkCyan<br/>
        ///     {blue2} - Blue<br/>
        ///     {red2} - Red<br/>
        ///     {green2} - DarkGreen<br/>
        ///     {yellow2} - DarkYellow<br/>
        ///     {cyan2} - Cyan<br/>
        ///     {end} - ResetColor<br/><br/>
        /// 
        /// <b>Example</b><br/>
        /// <code>
        /// Logger.Write("{blue}Hello{end} {green}World{end}");
        /// Logger.Write("{red}Hello {cyan}World");
        /// Logger.Write($"{{red}}Hello {{cyan}}World");
        /// </code>
        /// </summary>
        /// <param name="text">Console Output</param>
        [DebuggerHidden]
        public static void Write(string text)
        {
            writePrefix(" LOG ", ConsoleColor.White, ConsoleColor.Blue);
            writeBase(text);
        }

        [DebuggerHidden]
        private static void writeBase(string text)
        {
            bool command = false;
            string commandString = "";
            foreach (var item in text)
            {
                if (item == '{')
                {
                    command = true;
                    continue;
                }
                else if (command && item == '}')
                {
                    command = false;
                    commandString = commandString.ToLower();
                    if (commandString == "end") Console.ResetColor();
                    else if (commandString == "blue") Console.ForegroundColor = ConsoleColor.DarkBlue;
                    else if (commandString == "red") Console.ForegroundColor = ConsoleColor.DarkRed;
                    else if (commandString == "green") Console.ForegroundColor = ConsoleColor.Green;
                    else if (commandString == "cyan") Console.ForegroundColor = ConsoleColor.DarkCyan;
                    else if (commandString == "blue2") Console.ForegroundColor = ConsoleColor.Blue;
                    else if (commandString == "red2") Console.ForegroundColor = ConsoleColor.Red;
                    else if (commandString == "green2") Console.ForegroundColor = ConsoleColor.DarkGreen;
                    else if (commandString == "cyan2") Console.ForegroundColor = ConsoleColor.Cyan;
                    else if (commandString == "yellow") Console.ForegroundColor = ConsoleColor.Yellow;
                    else if (commandString == "yellow2") Console.ForegroundColor = ConsoleColor.DarkYellow;
                    else Console.Write($"{{{commandString}}}");
                    commandString = "";
                    continue;
                }

                if (command) commandString += item;
                else Console.Write(item);

            }
            Console.ResetColor();
        }

        [DebuggerHidden]
        private static void writePrefix(string prefix, ConsoleColor fore, ConsoleColor back)
        {
            Console.ForegroundColor = fore;
            Console.BackgroundColor = back;
            Console.Write(prefix);
            Console.ResetColor();
            Console.Write(" :");
        }

        [DebuggerHidden]
        private static string getFrameString(StackFrame frame)
        {
            var method = frame.GetMethod();
            string methodParams = $"{string.Join(", ", method.GetParameters().Select((param_) => param_.ParameterType.IsGenericType && param_.ParameterType.GetGenericTypeDefinition() == typeof(Nullable<>) ? param_.ParameterType.GetGenericArguments()[0].Name + "?" : param_.ParameterType.Name))}";
            string file_name = frame.GetFileName();
            if (!string.IsNullOrEmpty(file_name) && file_name.Length > 10)
                file_name = file_name[file_name.LastIndexOf("BotCS\\")..];

            return $"{file_name}:{frame.GetFileLineNumber()} - {method.ReflectedType.Name}.{method.Name}({methodParams});";
        }

        /// <summary>
        /// Prints the error on the console with the file path visible.
        /// </summary>
        /// <param name="e">Error</param>
        [DebuggerHidden]
        public static void Error(Exception e)
        {
            if (e == null)
            {
                Error("Unknown Error", 2);
                return;
            }

            var frames = new StackTrace(e, true).GetFrames();
            string error = e != null ? e.Message : "";
            writePrefix("ERROR", ConsoleColor.White, ConsoleColor.DarkRed);

            foreach (var frame in frames)
                error += $"\n\t{{red}}{getFrameString(frame)}{{end}}";

            writeBase(error + "\n");
        }

        [DebuggerHidden]
        [Obsolete("It is not recommended to use")]
        public static void FatalError(Exception e)
        {
            Console.Title = "BotCS FATAL ERROR";
            if (e == null)
            {
                FatalError("Unknown Fatal Error", 2);
                return;
            }

            var frames = new StackTrace(e, true).GetFrames();
            string error = e != null ? e.Message : "";
            writePrefix("F_ERR", ConsoleColor.White, ConsoleColor.DarkRed);

            foreach (var frame in frames)
                error += $"\n\t{getFrameString(frame)}";

            Console.BackgroundColor = ConsoleColor.DarkRed;
            writeBase(error + "\n");
        }

        [DebuggerHidden]
        [Obsolete("It is not recommended to use")]
        public static void FatalError(string text = null, int skip = 1)
        {
            Console.Title = "BotCS FATAL ERROR";

            var frames = new StackTrace(skip, true).GetFrames();
            string error = text ?? "";
            writePrefix("F_ERR", ConsoleColor.White, ConsoleColor.DarkRed);

            foreach (var frame in frames)
                error += $"\n\t{getFrameString(frame)}";

            Console.BackgroundColor = ConsoleColor.DarkRed;
            writeBase(error + "\n");
        }

        /// <summary>
        /// It is used to write an error message to the console. It is written with the file path when typing the error message.
        /// </summary>
        /// <param name="text">Additional info.</param>
        /// <param name="skip">The number of file paths that will not be printed.</param>
        [DebuggerHidden]
        public static void Error(string text = null, int skip = 1)
        {
            var frames = new StackTrace(skip, true).GetFrames();
            string error = text ?? "";
            writePrefix("ERROR", ConsoleColor.White, ConsoleColor.DarkRed);

            foreach (var frame in frames)
                error += $"\n\t{{red}}{getFrameString(frame)}{{end}}";

            writeBase(error + "\n");
        }

        /// <summary>
        /// It is used to clear the text on the console.
        /// </summary>
        [DebuggerHidden]
        public static void Clear()
        {
            Console.Clear();
            Console.ResetColor();
        }

        /// <summary>
        /// It is used to read data from the console.
        /// </summary>
        /// <param name="prefix">The text to be written before getting the value through the console.</param>
        /// <returns>Value read from console.</returns>
        [DebuggerHidden]
        public static string ReadLine(string prefix = null)
        {
            if (!string.IsNullOrEmpty(prefix))
                writeBase(prefix);
            return Console.ReadLine();
        }

        /// <summary>
        /// It works the same as the Readline method, but insists that the user enter numbers.
        /// </summary>
        /// <param name="numberOfAttempts">indicates how many times to insist. Unlimited at <see langword="-1"/> or lower numbers.default <see langword="-1"/></param>
        /// <param name="prefix">The text to be written before getting the value through the console.</param>
        /// <returns>Value read from console.</returns>
        [DebuggerHidden]
        public static int? ReadInt(int numberOfAttempts = -1, string prefix = null)
        {
            int? Out = null;
            while (numberOfAttempts-- != 0)
            {
                try
                {
                    Out = int.Parse(ReadLine(prefix));
                    break;
                }
                catch (FormatException)
                {
                    goto WRITE_TRY_AGAIN;
                }
                catch (OverflowException)
                {
                    goto WRITE_TRY_AGAIN;
                }

                continue;
            WRITE_TRY_AGAIN:
                Logger.WriteLine("{blue}Please enter a number{end}.");
            }

            return Out;
        }

        /// <summary>
        /// It works the same as the Readline method, but insists the user enter a value based on the type you provide.
        /// </summary>
        /// <param name="type">Type to be specified</param>
        /// <param name="numberOfAttempts">indicates how many times to insist. Unlimited at <see langword="-1"/> or lower numbers.default <see langword="-1"/></param>
        /// <param name="prefix">The text to be written before getting the value through the console.</param>
        /// <returns>Value read from console.</returns>
        /// <exception cref="ArgumentException">The given type must contain the (static)Parse(String) method.</exception>
        [DebuggerHidden]
        public static object ReadType(Type type, int numberOfAttempts = -1, string prefix = null)
        {
            var parseMethod = type.GetMethod("Parse", 0, new Type[] { typeof(string) }) ??
                throw new ArgumentException("No method named (static)Parse was found in the given type or the parameters in the method do not match. The method should be Parse(String).", nameof(type));

            while (numberOfAttempts-- != 0)
            {
                string tempVar;
                try
                {
                    tempVar = ReadLine(prefix);
                }
                catch (Exception) { throw; }

                try
                {
                    return parseMethod.Invoke(null, new object[] { tempVar });
                }
                catch (Exception) { }

                Logger.WriteLine($"{{blue}}Please enter a value of the specified type.{{end}}({{yellow2}}{type.Name}{{end}}){{cyan}}For detailed information \"type {type.FullName}\"{{end}}.");
            }

            return null;
        }

        /// <summary>
        /// It works the same as the Readline method, but insists the user enter a value based on the type you provide.
        /// </summary>
        /// <typeparam name="T">Type to be specified</typeparam>
        /// <param name="numberOfAttempts">indicates how many times to insist. Unlimited at <see langword="-1"/> or lower numbers.default <see langword="-1"/></param>
        /// <param name="prefix">The text to be written before getting the value through the console.</param>
        /// <returns>Value read from console.</returns>
        /// <exception cref="ArgumentException"></exception>
        [DebuggerHidden]
        public static T? ReadType<T>(int numberOfAttempts = -1, string prefix = null) where T : struct
        {
            return (T?)ReadType(typeof(T), numberOfAttempts, prefix);
        }

        /// <summary>
        /// It works the same as the Readline method, but insists the user enter a value based on the type you provide.
        /// </summary>
        /// <typeparam name="T">Type to be specified</typeparam>
        /// <param name="numberOfAttempts">indicates how many times to insist. Unlimited at <see langword="-1"/> or lower numbers.default <see langword="-1"/></param>
        /// <param name="prefix">The text to be written before getting the value through the console.</param>
        /// <returns>Value read from console.</returns>
        /// <exception cref="ArgumentException"></exception>
        [DebuggerHidden]
        public static T ReadClassType<T>(int numberOfAttempts = -1, string prefix = null) where T : class
        {
            return (T)ReadType(typeof(T), numberOfAttempts, prefix);
        }

        /// <summary>
        /// It works the same as the ReadLine method but is used to read commands.
        /// </summary>
        /// <param name="prefix">The text to be written before getting the value through the console.</param>
        /// <returns>Command read from console.</returns>
        [DebuggerHidden]
        public static (string cmd, string[] args) ReadCommand(string prefix = ">>>")
        {
            string readedValue = ReadLine(prefix);
            if (!string.IsNullOrEmpty(readedValue))
            {
                var temp = readedValue.Split(' ');
                return (temp[0].ToLower(), temp.Skip(1).ToArray());
            }
            return (null, null);
        }


        /// <summary>
        /// It starts a menu according to the commands you have given.
        /// </summary>
        /// <param name="Aliases">commands. <br/><see langword="Key"/> = Command<br/><see langword="Value"/> = description.</param>
        /// <param name="OnCalled">If one of the commands specified in the menu is written, it is executed.<br/><br/><b><u>Information</u></b><br/>If the <see cref="MenuResult.Break"/> property is set to true, it terminates the menu.</param>
        /// <param name="startText">The text that will be written when the menu is started. <b>Supports colored text.</b></param>
        /// <param name="prefix">Prefix at the time of reading in the menu. <b>Supports colored text.</b></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="FormatException"></exception>
        public static void StartMenu(IDictionary<string, string> Aliases, Action<MenuResult> OnCalled, string startText = "{blue}You have entered the menu{end}. {yellow}You can type {yellow2}\"exit\"{yellow} to exit and {yellow2}\"help\"{yellow} to get help{end}.", string prefix = ">>>")
        {
            if (Aliases == null)
                throw new ArgumentNullException(nameof(Aliases));
            if (OnCalled == null)
                throw new ArgumentNullException(nameof(OnCalled));

            var tempKey = Aliases.Keys.ToList().Find((key) => key.IndexOf(" ") > 0);
            if (tempKey != null)
                throw new FormatException($"Space characters cannot be found in the given commands.Given value \"{tempKey}\"");


            var aliases = Aliases.Keys.Select(p1 => p1.ToLower()).ToList();


            if (!string.IsNullOrEmpty(startText))
                WriteLine(startText);

            while (true)
            {
                var readed = ReadCommand(prefix);
                if (aliases.Contains(readed.cmd))
                {
                    var OutObject = new MenuResult() { Command = readed.cmd, Args = readed.args.ToList(), Break = false };
                    OnCalled.Invoke(OutObject);
                    if (OutObject.Break)
                        break;
                }
                else
                {
                    if (readed.cmd == "exit")
                        break;
                    else if (readed.cmd == "help")
                    {
                        writeBase("{green}Commands{end}\n    " + string.Join("\n    ", Aliases.Select((p1) => $"{{cyan}}{p1.Key} {{end}}:{(!string.IsNullOrEmpty(p1.Value) ? $"{{yellow2}}{p1.Value}{{end}}" : "{blue}Unknown{end}")}")) + "\n\n");
                    }
                    else WriteLine("{red}Wrong command{end}. {blue}You can type {yellow2}\"help\"{blue} to get help{end}.");
                }
            }
        }
    }
}

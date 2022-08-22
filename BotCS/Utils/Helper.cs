using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace BotCS.Utils
{
    internal static class Helper
    {
        public static void RemoveCurrentConsoleLine()
        {
            int currentCursorLine = Console.CursorTop;
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, currentCursorLine);
        }

        public static string GuidToID(this Guid value)
        {
            return BitConverter.ToUInt64(value.ToByteArray(), 0).ToString();
        }

        public static string GuidToID(this string value)
        {
            return Guid.Parse(value).GuidToID();
        }

        public static string GetEnumValues(Type enumType ,string joinString = ".\n", string lastString = ".\n")
        {
            List<string> Out = new();
            try
            {
                foreach (int item in Enum.GetValues(enumType))
                {
                    Out.Add($"({item}){Enum.GetName(enumType, item)}");
                }
            }
            catch (Exception) 
            {
                foreach (byte item in Enum.GetValues(enumType))
                {
                    Out.Add($"({item}){Enum.GetName(enumType, item)}");
                }
            }
            

            return string.Join(joinString, Out) + lastString;
        }
    }
}

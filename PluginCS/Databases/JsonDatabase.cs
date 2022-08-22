using PluginCS.Databases.Content;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Diagnostics;

namespace PluginCS.Databases
{
    public static class JsonDatabase
    {
        private static JsonContent jsonContent;
        private static bool isWorking = false;
        private const string filePath = ".\\database.json";

        internal static void Read()
        {
            while (isWorking) { Thread.Sleep(10); }
            isWorking = true;
            if (!File.Exists(filePath)) safeSave();
            safeRead();
            isWorking = false;
        }

        internal static void Save()
        {
            while (isWorking) { Thread.Sleep(10); }
            isWorking = true;
            safeSave();
            isWorking = false;
        }

        private static void safeRead()
        {
            if (File.Exists(filePath))
                jsonContent = JsonSerializer.Deserialize<JsonContent>(File.ReadAllText(filePath));
        }

        private static void safeSave()
        {
            if (File.Exists(filePath)) File.Delete(filePath);
            if (jsonContent == null) jsonContent = new JsonContent();
            File.WriteAllText(filePath, JsonSerializer.Serialize(jsonContent));
        }





        public static bool Set(string key, object value, bool showError = true) => Set(new KeyValuePair<string, object>(key, value), showError);

        public static bool Set(KeyValuePair<string, object> _param, bool showError = true)
        {
            try
            {
                if (_param.Key.ToLower() == "token")
                {
                    var frames = new StackTrace(1, true).GetFrames();
                    if (frames.Length > 0)
                    {
                        foreach (var item in frames)
                        {
                            if (item.GetMethod().DeclaringType.FullName == "BotCS.SystemPlugins.Token" ||
                                item.GetMethod().DeclaringType.FullName == "BotCS.Discord.Client")
                            {
                                goto SAFE;
                            }
                        }
                    }
                    return false;
                }
            SAFE:
                Read();
                if (jsonContent.Content.TryGetValue(_param.Key, out var _value))
                {
                    jsonContent.Content.Remove(_param.Key);
                    jsonContent.Content.Add(_param.Key, _param.Value);
                }
                else
                {
                    jsonContent.Content.Add(_param.Key, _param.Value);
                }
                Save();

                return true;
            }
            catch (Exception ex)
            {
                if (showError)
                    Logger.Error(ex);
                return false;
            }
        }

        public static bool Remove(string key, bool showError = true)
        {
            try
            {
                if (key.ToLower() == "token")
                {
                    var frames = new StackTrace(1, true).GetFrames();
                    if (frames.Length > 0)
                    {
                        foreach (var item in frames)
                        {
                            if (item.GetMethod().DeclaringType.FullName == "BotCS.SystemPlugins.Token" ||
                                item.GetMethod().DeclaringType.FullName == "BotCS.Discord.Client")
                            {
                                goto SAFE;
                            }
                        }
                    }
                    return false;
                }
            SAFE:
                Read();
                if (jsonContent.Content.TryGetValue(key, out var value)) jsonContent.Content.Remove(key);
                else return false;
                Save();
                return true;
            }
            catch (Exception ex)
            {
                if (showError)
                    Logger.Error(ex);
                return false;
            }

        }

        public static bool Has(string key, bool showError = true)
        {
            try
            {
                Read();
                if (jsonContent.Content.TryGetValue(key, out var value)) return true;
                return false;
            }
            catch (Exception ex)
            {
                if (showError)
                    Logger.Error(ex);
                return false;
            }
        }

        public static object Get(string key, bool showError = true)
        {
            try
            {
                if (key.ToLower() == "token")
                {
                    var frames = new StackTrace(1, true).GetFrames();
                    if (frames.Length > 0)
                    {
                        foreach (var item in frames)
                        {
                            if (item.GetMethod().DeclaringType.FullName == "BotCS.SystemPlugins.Token" ||
                                item.GetMethod().DeclaringType.FullName == "BotCS.Discord.Client")
                            {
                                goto SAFE;
                            }
                        }
                    }
                    return false;
                }
            SAFE:
                Read();
                if (jsonContent.Content.TryGetValue(key, out var value))
                {
                    if (value is JsonElement element && element.ValueKind == JsonValueKind.Array) return JsonElementConvertToList(element);
                    else return value;
                }
                return null;
            }
            catch (Exception ex)
            {
                if (showError)
                    Logger.Error(ex);
                return null;
            }
        }

        public static List<object> GetList(string key, bool showError = true)
        {
            try
            {
                if (Get(key, showError) is List<object> list) return list;
                else return null;
            }
            catch (Exception ex)
            {
                if (showError)
                    Logger.Error(ex);
                return null;
            }
        }

        public static int? GetInt(string key, bool showError = true)
        {
            try
            {
                var temp = Get(key, showError);
                if (temp != null)
                    if (int.TryParse(temp.ToString(), out int value)) return value;
                    else return null;
            }
            catch (Exception ex)
            {
                if (showError)
                    Logger.Error(ex);
            }
            return null;
        }

        public static bool? GetBool(string key, bool showError = true)
        {
            try
            {
                var temp = Get(key, showError);
                if (temp != null)
                    if (bool.TryParse(temp.ToString(), out bool value)) return value;
                    else return null;
            }
            catch (Exception ex)
            {
                if (showError)
                    Logger.Error(ex);
            }
            return null;
        }

        public static string GetString(string key, bool showError = true)
        {
            try
            {
                object tempValue = Get(key, showError);
                if (tempValue != null) return tempValue.ToString();
                return (string)null;
            }
            catch (Exception ex)
            {
                if (showError)
                    Logger.Error(ex);
                return null;
            }
        }

        public static List<object> JsonElementConvertToList(JsonElement jsonElement)
        {
            List<object> Out = new List<object>();
            if (jsonElement.ValueKind == JsonValueKind.Array)
                foreach (var item in jsonElement.EnumerateArray())
                {
                    if (item.ValueKind == JsonValueKind.Array) Out.Add(JsonElementConvertToList(item));
                    else if (item.ValueKind == JsonValueKind.Number)
                    {
                        if (item.TryGetByte(out byte byte_value))
                        {
                            Out.Add(byte_value);
                        }
                        else if (item.TryGetSByte(out sbyte sbyte_value))
                        {
                            Out.Add(sbyte_value);
                        }
                        else if (item.TryGetDouble(out double double_value))
                        {
                            Out.Add(double_value);
                        }
                        else if (item.TryGetSingle(out float single_value))
                        {
                            Out.Add(single_value);
                        }
                        else if (item.TryGetInt16(out short short_value))
                        {
                            Out.Add(short_value);
                        }
                        else if (item.TryGetUInt16(out ushort ushort_value))
                        {
                            Out.Add(ushort_value);
                        }
                        else if (item.TryGetInt32(out int int_value))
                        {
                            Out.Add(int_value);
                        }
                        else if (item.TryGetUInt32(out uint uint_value))
                        {
                            Out.Add(uint_value);
                        }
                        else if (item.TryGetInt64(out long long_value))
                        {
                            Out.Add(long_value);
                        }
                        else if (item.TryGetUInt64(out ulong ulong_value))
                        {
                            Out.Add(ulong_value);
                        }
                        else if (item.TryGetDecimal(out decimal decimal_value))
                        {
                            Out.Add(decimal_value);
                        }
                    }
                    else if (item.ValueKind == JsonValueKind.Null)
                    {
                        Out.Add(null);
                    }
                    else if (item.ValueKind == JsonValueKind.Undefined)
                    {
                        Out.Add(null);
                    }
                    else if (item.ValueKind == JsonValueKind.True || item.ValueKind == JsonValueKind.False)
                    {
                        Out.Add(item.GetBoolean());
                    }
                    else if (item.ValueKind == JsonValueKind.String)
                    {
                        if (item.TryGetGuid(out Guid guid_value))
                        {
                            Out.Add(guid_value);
                        }
                        else if (item.TryGetDateTime(out DateTime dateTime_value))
                        {
                            Out.Add(dateTime_value);
                        }
                        else if (item.TryGetDateTimeOffset(out DateTimeOffset dateTimeOffset_value))
                        {
                            Out.Add(dateTimeOffset_value);
                        }
                        else
                        {
                            Out.Add(item.GetString());
                        }
                    }
                    else if (item.ValueKind == JsonValueKind.Object)
                    {
                        var tempDic = JsonSerializer.Deserialize<Dictionary<string, object>>(item.ToString());

                        foreach (var item2 in tempDic.Keys)
                        {
                            if (tempDic[item2] is JsonElement element && element.ValueKind == JsonValueKind.Array)
                            {
                                tempDic[item2] = JsonElementConvertToList(element);
                            }
                        }

                        Out.Add(tempDic);
                    }
                }
            return Out;
        }
    }
}

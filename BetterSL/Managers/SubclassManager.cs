using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BetterSL.Resources;
using PluginAPI.Core;
using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.Serialization;
using Utf8Json;
using PlayerRoles.SpawnData;
using BetterSL.Subclasses;

namespace BetterSL.Managers
{
    public class SubclassManager
    {
        private static string _path = Plugin.instance.SubclassPath;
        private static Serializer _serializer = (Serializer)new SerializerBuilder()
            .WithNamingConvention(UnderscoredNamingConvention.Instance)
            .Build();
        private static Deserializer _deserializer = (Deserializer)new DeserializerBuilder()
            .WithNamingConvention(UnderscoredNamingConvention.Instance)
            .Build();
        private static Dictionary<string, BaseSubclass> _loadedSubClasses = new Dictionary<string, BaseSubclass>();

        public static void Init()
        {
            if (!Directory.Exists(Plugin.instance.SubclassPath))
            {
                Log.Warning("Subclass folder does not exist. Generating.");
                Directory.CreateDirectory(Plugin.instance.SubclassPath);
            }
            SetSubclass(new MtfMarksmanSubclass());
            SetSubclass(new MtfVanguardSubclass());
            SetSubclass(new MtfShockTrooperSubclass());
        }

        public static void SetPlayerToSubclass(Player target, BaseSubclass targetSubclass)
        {
            target.SetRole(targetSubclass.BaseRole);
            target.ClearInventory();
            target.PlayerInfo.IsRoleHidden = targetSubclass.HideActualRole;
            target.CustomInfo = targetSubclass.DisplayName;
            foreach(ItemType item in targetSubclass.SpawnItems.Keys)
            {
                //Cheeky fix
                if (item.ToString().Contains("Ammo"))
                {
                    target.AddAmmo(item, (ushort)targetSubclass.SpawnItems[item]);
                }
                else
                {
                    for(int i = 0; i < targetSubclass.SpawnItems[item]; i++)
                    {
                        target.AddItem(item); 
                    }
                }
            }
            BetterSL.Resources.Extensions.ApplyAttachments(target);
            string text = Plugin.GetConfig().PlayerSubclassNotificationText;
            text = text.Replace("{subclass}", targetSubclass.DisplayName);
            text = text.Replace("{description}", targetSubclass.Description);
            text = text.Replace("{filename}", targetSubclass.FileName);
            target.SendBroadcast(text, Plugin.GetConfig().PlayerSubclassNotificationDuration, shouldClearPrevious: true);
        }


        public static BaseSubclass GetSubclass(string fileName, BaseSubclass defaultValue = null, bool writeDefaultValueToDisk = false)
        {
            if (_loadedSubClasses.ContainsKey(fileName))
            {
                return _loadedSubClasses[fileName];
            }
            else
            {
                //fileName += ".yml";
                BaseSubclass subclass = ReadFromDisk(fileName);
                if(subclass == null)
                {
                    Log.Warning("Failed to get subclass based on filename. Filename: " + fileName);
                    if(defaultValue != null)
                    {
                        if (writeDefaultValueToDisk)
                        {
                            WriteToDisk(defaultValue);
                        }
                        return defaultValue;
                    }
                    return null;
                }
                else
                {
                    _loadedSubClasses.Add(fileName, subclass);
                    return subclass;
                }
            }
        }

        public static void SetSubclass(BaseSubclass subClass, bool overwrite = false)
        {
            if (_loadedSubClasses.ContainsKey(subClass.FileName))
            {
                if (overwrite)
                {
                    _loadedSubClasses[subClass.FileName] = subClass;
                    WriteToDisk(subClass, overwrite: overwrite);
                }
            }
            if(!_loadedSubClasses.ContainsKey(subClass.FileName))
            {
                _loadedSubClasses.Add(subClass.FileName, subClass);
                WriteToDisk(subClass, overwrite: overwrite);
            }
        }

        public static void ClearCache(bool saveCacheToDisk, bool overwrite = false)
        {
            if (saveCacheToDisk)
            {
                foreach(string key in _loadedSubClasses.Keys)
                {
                    WriteToDisk(_loadedSubClasses[key], overwrite: overwrite);
                }
            }
            _loadedSubClasses.Clear();
        }

        private static void WriteToDisk(BaseSubclass subclass, bool overwrite = false)
        {
            string _filepath = Path.Combine(_path, subclass.FileName) + ".yml";
            if (!File.Exists(_filepath))
            {
                string output = _serializer.Serialize(subclass);
                File.WriteAllText(_filepath, output);
            }
            else
            {
                if (overwrite)
                {
                    File.Delete(_filepath);
                    string output = _serializer.Serialize(subclass);
                    File.WriteAllText(_filepath, output);
                }
            }
        }

        private static BaseSubclass ReadFromDisk(string filename)
        {
            string _filepath = Path.Combine(_path, filename) + ".yml";
            if (!File.Exists(_filepath))
            {
                Log.Debug(_filepath);
                Log.Warning("Failed to get subclass: No such file. Subclass filename: " + filename);
                return null;
            }
            else
            {
                string data = File.ReadAllText(_filepath);
                BaseSubclass subclass = _deserializer.Deserialize<BaseSubclass>(data);
                return subclass;
            }
        }
    }
}

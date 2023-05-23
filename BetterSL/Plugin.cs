using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PluginAPI;
using PluginAPI.Core.Attributes;
using PluginAPI.Core;
using PluginAPI.Helpers;

namespace BetterSL
{
    public class Plugin
    {
        public const string PluginName = "BetterSL";
        public const string PluginVersion = "0.1.0-alpha";
        public const string PluginDesc = "A plugin to make \'better\' balancing changes to SL.";

        public static Plugin instance;
        [PluginConfig(PluginName + ".yml")]
        public Config config = new Config();
        public EventHandler eventHandler;

        public string AudioPath = Path.Combine($"{Paths.Plugins}", "Sounds");

        [PluginEntryPoint(PluginName, PluginVersion, PluginDesc, "btelnyy#8395")]
        public void LoadPlugin()
        {
            if (!config.PluginEnabled)
            {
                Log.Debug("Plugin is disabled!");
                return;
            }
            instance = this;
            PluginAPI.Events.EventManager.RegisterEvents<EventHandler>(this);
            Log.Debug("BetterSL v" + PluginVersion + " loaded.");
        }

        [PluginUnload()]
        public void Unload()
        {
            config = null;
            eventHandler = null;
        }
    }
}

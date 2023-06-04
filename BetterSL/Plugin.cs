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
using HarmonyLib;
using BetterSL.EventHandlers;
using PlayerRoles.Ragdolls;
using BetterSL.EventHandlers.Generic;

namespace BetterSL
{
    public class Plugin
    {
        public const string PluginName = "BetterSL";
        public const string PluginVersion = "0.1.0-alpha";
        public const string PluginDesc = "A plugin to make \'better\' balancing changes to SL.";
        public Harmony harmony;
        public static Plugin instance;
        [PluginConfig(PluginName + ".yml")]
        public Config config = new Config();
        public static Config GetConfig()
        {
            return instance.config;
        }
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
            Log.Info("Registering events...");
            //uneeded
            //PluginAPI.Events.EventManager.RegisterEvents<Scp106Handler>(this);
            //Registers every event method possible
            PluginAPI.Events.EventManager.RegisterAllEvents(this);
            // harmony my beloved
            Log.Info("Patching...");
            harmony = new Harmony("com.lurkbois.shitbalanceplugin");
            harmony.PatchAll();
            Log.Info("BetterSL v" + PluginVersion + " loaded.");
            RagdollManager.OnRagdollSpawned += DimensionBodyHandler.OnRagdoll;
            StreamWriter streamWriter = new StreamWriter(Console.OpenStandardOutput());
            TextWriter error = new StreamWriter(Console.OpenStandardError());
            Console.SetOut(streamWriter);
            Console.SetError(error);
        }

        [PluginUnload()]
        public void Unload()
        {
            config = null;
            eventHandler = null;
            harmony.UnpatchAll("com.lurkbois.shitbalanceplugin"); // this needs to be the same as above or else we unpatch everyone's patches (other plugins) for some godforsaken reason
            RagdollManager.OnRagdollSpawned -= DimensionBodyHandler.OnRagdoll;
        }
    }
}

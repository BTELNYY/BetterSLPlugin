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
using PluginAPI.Events;
using InventorySystem.Items.Pickups;
using BetterSL.Managers;
using System.Reflection;
using BetterSL.Resources;

namespace BetterSL
{
    public class Plugin
    {
        public const string PluginName = "BetterSL";
        public const string PluginVersion = "0.6.0";
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
        public string SubclassPath = "";
        public string ConfigPath = "";
        public static TextConfig NpcNameConfig = null;
        public static TextConfig NpcDeathConfig = null;

        [PluginEntryPoint(PluginName, PluginVersion, PluginDesc, "btelnyy#8395")]
        public void LoadPlugin()
        {
            if (!config.PluginEnabled)
            {
                Log.Debug("Plugin is disabled!");
                return;
            }
            SubclassPath = Path.Combine($"{PluginHandler.Get(this).PluginDirectoryPath}", "subclasses");
            ConfigPath = PluginHandler.Get(this).PluginDirectoryPath;
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
            ItemSpawnHandler.Init();
            SubclassManager.Init();
            NpcDeathConfig = new TextConfig(ConfigPath, "npc_death_reasons", new List<string>()
            {
                "Mauled by Roxanne Wolf",
                "Was forced to play Changed",
                "Became discord kitten",
                "I don't know how they died",
                "Got [REDACTED] by SCP 939",
                "Drank the forbidden milk from under the sink",
                "Exploded",
                "Crushed by SCP 939's massive ass",
                "Forced to code Yandere Simulator",
                "Couldn't escape the van",
                "Imploded",
                "Tried to client mod Secret Lab",
                "Tried to visit the titanic",
                "[DATA LOST]",
                "Forced to install ARK:SE mods on Linux"
            });
            NpcNameConfig = new TextConfig(ConfigPath, "npc_names", new List<string>()
            {
                "Gregory",
                "Joe mama",
                "D-9341",
                "Joseph Stalin",
                "William Afton",
                "???",
                "Bob",
                "TV",
                "Adam Smith",
                "Johnny",
                "the pacer gram fitness test",
                "catcloner",
                "gonegooner",
                "Class D Puro",
                "hilts"
            });
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

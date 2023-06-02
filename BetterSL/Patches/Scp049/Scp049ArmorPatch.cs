using GameCore;
using HarmonyLib;
using PlayerRoles.PlayableScps.Scp049;
using PlayerRoles.PlayableScps.Scp096;
using ServerOutput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetterSL.Patches.Scp049
{
    [HarmonyPatch(typeof(Scp049Role), nameof(Scp049Role.GetArmorEfficacy))]
    public class Scp049ArmorPatch
    {
        static void Prefix(ref Scp049Role __instance)
        {
            var effecacy = AccessTools.Field(typeof(Scp049Role), "_armorEfficacy");
            var array = AccessTools.GetFieldNames(typeof(Scp049Role));
            foreach(var name in array )
            {
                PluginAPI.Core.Log.Debug(name);
            }
            effecacy.SetValue(__instance, Plugin.instance.config.Scp049Armour);
        }
    }
}

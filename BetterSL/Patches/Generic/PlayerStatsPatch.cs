using BetterSL.Resources.NewStatModules;
using HarmonyLib;
using PlayerStatsSystem;
using System;
using PluginAPI.Core;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetterSL.Patches.Generic
{
    //[HarmonyPatch(typeof(PlayerStats), "Start")]
    public class PlayerStatsPatch
    {
        public static void Postfix(PlayerStats __instance)
        {
            try
            {
                __instance.StatModules[0] = new NewHealthStat();
                __instance.StatModules[1] = new NewAhpStat();
                __instance.StatModules[4] = new NewHumeShieldStat();
                for (int i = 0; i < __instance.StatModules.Length; i++)
                {
                    ReferenceHub hub = (ReferenceHub)AccessTools.Field(typeof(PlayerStats), "_hub").GetValue(__instance);
                    object[] obj = { hub };
                    AccessTools.PropertySetter(typeof(StatBase), nameof(StatBase.Hub)).Invoke(__instance.StatModules[i], obj);
                }
            }
            catch(Exception ex)
            {
                Log.Error(ex.ToString());
            }
        }
    }
}

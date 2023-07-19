using CustomPlayerEffects;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BetterSL.EventHandlers.Generic;

namespace BetterSL.Patches.Generic
{
    [HarmonyPatch(typeof(StatusEffectBase), "Enabled")]
    public class CorrodingDisabledPatch
    {
        public static void Prefix(StatusEffectBase __instance)
        {
            if(__instance is Corroding)
            {
                CorrodingHandler.CorrodedPlayers.Add(__instance.Hub);
            }
        }
    }

    [HarmonyPatch(typeof(StatusEffectBase), "Disabled")]
    public class CorrodingEnabledPatch
    {
        public static void Prefix(StatusEffectBase __instance)
        {
            if (__instance is Corroding)
            {
                CorrodingHandler.CorrodedPlayers.Remove(__instance.Hub);
            }
        }
    }
}

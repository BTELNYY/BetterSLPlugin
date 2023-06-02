using HarmonyLib;
using PlayerRoles.PlayableScps;
using PlayerRoles.PlayableScps.Scp079;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace BetterSL.Patches.Scp079
{
    [HarmonyPatch(typeof(Scp079AuxManager), nameof(Scp079AuxManager.SpawnObject))]
    public class Scp079AuxRegenPatch
    {
        public static void Postfix(Scp079AuxManager __instance)
        {
            var regen = AccessTools.Field(typeof(Scp079AuxManager), "_regenerationPerTier");
            regen.SetValue(__instance, Plugin.GetConfig().Scp079RegenRate);
        }
    }
}

using HarmonyLib;
using PlayerRoles.PlayableScps.Scp173;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetterSL.Patches
{
    [HarmonyPatch(typeof(Scp173BreakneckSpeedsAbility), "Awake")]
    public class Scp173BreakneckSpeedsPatch
    {
        public static void Postfix(Scp173BreakneckSpeedsAbility __instance)
        {
            __instance.Cooldown.NextUse = (float)AccessTools.Field(typeof(Scp173TantrumAbility), "RechargeTime").GetValue(__instance);
        }
    }
}

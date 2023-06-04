using HarmonyLib;
using PlayerRoles.PlayableScps.Scp079.Pinging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetterSL.Patches.Scp079
{
    [HarmonyPatch(typeof(GeneratorPingProcessor), nameof(GeneratorPingProcessor.Range), MethodType.Getter)]
    public class Scp079GeneratorPingRangePatch
    {
        public static void Postfix(GeneratorPingProcessor __instance, ref float __result)
        {
            __result = Plugin.GetConfig().Scp079GeneratorPingRange;
        }
    }
}

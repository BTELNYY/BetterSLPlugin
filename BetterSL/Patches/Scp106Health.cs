using HarmonyLib;
using PlayerRoles.PlayableScps;
using PlayerRoles.PlayableScps.HumeShield;
using PlayerRoles.PlayableScps.Scp106;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetterSL.Patches
{
    [HarmonyPatch(typeof(FpcStandardScp), nameof(FpcStandardScp.MaxHealth), MethodType.Getter)]
    public class Scp106Health
    {
        static void Postfix(FpcStandardScp __instance, ref float __result)
        {
            if (__instance.RoleTypeId == PlayerRoles.RoleTypeId.Scp106)
            {
                __result = Plugin.instance.config.Scp106MaxHP;
            }
        }
    }
}

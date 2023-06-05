using HarmonyLib;
using PlayerRoles.PlayableScps;

namespace BetterSL.Patches.Scp106
{
    [HarmonyPatch(typeof(FpcStandardScp), nameof(FpcStandardScp.MaxHealth), MethodType.Getter)]
    public class Scp106HealthPatch
    {
        static void Postfix(FpcStandardScp __instance, ref float __result)
        {
            if (__instance.RoleTypeId == PlayerRoles.RoleTypeId.Scp106)
            {
                __result = Plugin.instance.config.Scp106MaxHp;
            }
        }
    }
}

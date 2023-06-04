using HarmonyLib;
using PlayerRoles;
using PlayerRoles.PlayableScps.HumeShield;
using PlayerRoles.PlayableScps.Scp106;

namespace BetterSL.Patches.Scp106
{
    [HarmonyPatch(typeof(DynamicHumeShieldController), nameof(DynamicHumeShieldController.HsMax), MethodType.Getter)]
    public class Scp106HumePatch
    {
        static void Postfix(DynamicHumeShieldController __instance, ref float __result) 
        {
            if (__instance.Role is Scp106Role)
            {
                __result = ((__result - 300) / 600) * 400 + 300;
            }
        }
    }
}
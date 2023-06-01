using HarmonyLib;
using PlayerRoles.PlayableScps.Scp079;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MEC;

namespace BetterSL.Patches
{
    [HarmonyPatch(typeof(Scp079LockdownRoomAbility), "RemainingCooldown", MethodType.Getter)]
    public class Scp079CooldownPatch
    {
        public static void Postfix(Scp079LockdownRoomAbility __instance, ref float __result)
        {
            if(__result > 20f)
            {
                __result = 20f;
            }
        }
    }
}

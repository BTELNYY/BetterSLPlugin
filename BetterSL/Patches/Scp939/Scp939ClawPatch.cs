using GameCore;
using HarmonyLib;
using PlayerRoles.PlayableScps.Scp939;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetterSL.Patches.Scp939
{
    [HarmonyPatch(typeof(Scp939ClawAbility), "BaseCooldown", MethodType.Getter)]
    public class Scp939ClawPatch
    {
        public static void Postfix(Scp939ClawAbility __instance, ref float __result) 
        {
            __result = Plugin.GetConfig().Scp939ClawCooldown;
        }
    }
}

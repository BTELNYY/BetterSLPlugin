using HarmonyLib;
using PlayerRoles.PlayableScps.Scp049;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetterSL.Patches
{
    [HarmonyPatch(typeof(Scp049ResurrectAbility), "MaxResurrections", MethodType.Getter)]
    public class Scp049ZombiePatch
    {
        public static void Prefix(Scp049ResurrectAbility __instance)
        {
            
        }
    }
}

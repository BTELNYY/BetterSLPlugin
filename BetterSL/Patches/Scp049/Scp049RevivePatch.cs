using HarmonyLib;
using PlayerRoles.PlayableScps.Scp049;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BetterSL.EventHandlers;
using PluginAPI.Core;

namespace BetterSL.Patches.Scp049
{
    [HarmonyPatch(typeof(Scp049ResurrectAbility), "ServerValidateBegin")]
    public class Scp049RevivePatch
    {
        public static bool Prefix(Scp049ResurrectAbility __instance, ref byte __result, BasicRagdoll ragdoll)
        {
            //temp fix
            //__result = 3;
            //return false;
            int resurrectionsNumber = Scp049ResurrectAbility.GetResurrectionsNumber(ragdoll.Info.OwnerHub);
            if (resurrectionsNumber < Plugin.GetConfig().Scp049Max0492Ressurection)
            {
                __result = 0;
                return false;
            }
            if (resurrectionsNumber <= Plugin.GetConfig().Scp049Max0492Ressurection)
            {
                __result = 3;
                return false;
            }
            __result = 4;
            return true;
        }
    }
}

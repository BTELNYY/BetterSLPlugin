using HarmonyLib;
using PlayerRoles.PlayableScps.Scp049;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BetterSL.EventHandlers;

namespace BetterSL.Patches.Scp049
{
    [HarmonyPatch(typeof(Scp049ResurrectAbility), "ServerValidateBegin")]
    public class Scp049RevivePatch
    {
        public static bool Prefix(Scp049ResurrectAbility __instance, ref byte __result, BasicRagdoll ragdoll)
        {
            if (Scp0492SpawnHandler.Player049Turns.ContainsKey(ragdoll.Info.OwnerHub.PlayerId)){
                if (Scp0492SpawnHandler.Player049Turns[ragdoll.Info.OwnerHub.PlayerId] >= Plugin.GetConfig().Scp049Max0492Ressurection)
                {
                    __result = 3;
                    return false;
                }
            }
            return true;
        }
    }
}

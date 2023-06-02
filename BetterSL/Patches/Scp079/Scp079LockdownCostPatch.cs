using GameCore;
using HarmonyLib;
using PlayerRoles.PlayableScps.Scp079;
using PluginAPI.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Log = PluginAPI.Core.Log;

namespace BetterSL.Patches.Scp079
{
    [HarmonyPatch(typeof(Scp079LockdownRoomAbility), "ServerProcessCmd")]
    public class Scp079LockdownCostPatch
    {
        public static void Prefix(Scp079LockdownRoomAbility __instance)
        {
            if(!__instance.ScpRole.SubroutineModule.TryGetSubroutine<Scp079AuxManager>(out var subroutine))
            {
                return;
            }
            else
            {
                var field = AccessTools.Field(typeof(Scp079LockdownRoomAbility), "_cost");
                int cost = (int)(subroutine.MaxAux * Plugin.GetConfig().Scp079LockdownCostPercent);
                field.SetValue(__instance, cost);
            }
        }
    }
}

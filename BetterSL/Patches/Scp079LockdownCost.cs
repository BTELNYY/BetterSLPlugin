using HarmonyLib;
using PlayerRoles.PlayableScps.Scp079;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetterSL.Patches
{
    [HarmonyPatch(typeof(Scp079LockdownRoomAbility), "ServerProcessCmd")]
    public class Scp079LockdownCost
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
                float cost = subroutine.MaxAux * (Plugin.GetConfig().Scp079LockdownCostPercent / 100);
                field.SetValue(__instance, cost);
            }

        }
    }
}

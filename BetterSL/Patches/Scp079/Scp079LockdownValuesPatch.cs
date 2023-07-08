using HarmonyLib;
using PlayerRoles.PlayableScps.Scp079;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetterSL.Patches.Scp079
{
    [HarmonyPatch(typeof(Scp079LockdownRoomAbility), "Start")]
    public class Scp079LockdownValuesPatch
    {
        public static void Postfix(Scp079LockdownRoomAbility __instance)
        {
            var regenarray = AccessTools.Field(typeof(Scp079LockdownRoomAbility), "_regenerationPerTier");
            regenarray.SetValue(__instance, Plugin.GetConfig().Scp079LockdownRegenMultiplier);
            var duration = AccessTools.Field(typeof(Scp079LockdownRoomAbility), "_lockdownDuration");
            duration.SetValue(__instance, Plugin.GetConfig().Scp079LockdownLength);
        }
    }
}

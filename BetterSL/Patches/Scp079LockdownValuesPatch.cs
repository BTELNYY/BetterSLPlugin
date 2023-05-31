using HarmonyLib;
using PlayerRoles.PlayableScps.Scp079;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetterSL.Patches
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
            //fix lockdown cooldown lasting forever
            var cooldown = AccessTools.Field(typeof(Scp079LockdownRoomAbility), "RemainingCooldown");
            PluginAPI.Core.Log.Debug(cooldown.GetValue(__instance).ToString());
            cooldown.SetValue(__instance, 20f);
            PluginAPI.Core.Log.Debug(cooldown.GetValue(__instance).ToString());
        }
    }
}

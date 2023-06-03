using HarmonyLib;
using PlayerRoles.PlayableScps.Scp079;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BetterSL.EventHandlers;
using BetterSL.EventHandlers.Scp079;

namespace BetterSL.Patches.Scp079
{
    [HarmonyPatch(typeof(Scp079DoorLockChanger), nameof(Scp079DoorLockChanger.ServerProcessCmd))]
    public class Scp079DoorLockPatch
    {
        public static bool Prefix(Scp079DoorLockChanger __instance)
        {
            if(Scp079DoorHandler.DoorsLocked >= Plugin.GetConfig().Scp079MaxLockedDoors)
            {
                return false;
            }
            return true;
        }
    }
}

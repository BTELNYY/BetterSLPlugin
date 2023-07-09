using HarmonyLib;
using Mirror;
using PlayerRoles.PlayableScps.Scp079;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BetterSL.Patches.Scp079
{
    [HarmonyPatch(typeof(Scp079BlackoutRoomAbility), "RemainingCooldown", MethodType.Getter)]
    public class Scp079BlackoutRoomCooldownPatch
    {
        public static bool Prefix(Scp079BlackoutRoomAbility __instance, ref float __result)
        {
            RoomLightController controller = (RoomLightController)AccessTools.Field(typeof(Scp079BlackoutRoomAbility), "_roomController").GetValue(__instance);
            var blackoutCooldownsField = AccessTools.Field(typeof(Scp079BlackoutRoomAbility), "_blackoutCooldowns");
            if(controller.Room.Zone != MapGeneration.FacilityZone.Surface)
            {
                return true;
            }
            double num2 = Plugin.GetConfig().Scp079SurfaceZoneBlackoutCooldown - NetworkTime.time;
            __result = Mathf.Max(0f, (float)num2);
            return false;
        }
    }
}

using HarmonyLib;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Mirror;
using PlayerRoles.PlayableScps.Scp079;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PluginAPI.Core;

namespace BetterSL.Patches.Scp079
{
    [HarmonyPatch(typeof(Scp079BlackoutRoomAbility), "ServerProcessCmd")]
    public class Scp079BlackoutRoomPatch
    {
        public static void Prefix(Scp079BlackoutRoomAbility __instance)
        {
            try
            {
                RoomLightController controller = (RoomLightController)AccessTools.Field(typeof(Scp079BlackoutRoomAbility), "_roomController").GetValue(__instance);
                var costField = AccessTools.Field(typeof(Scp079BlackoutRoomAbility), "_cost");
                costField.SetValue(__instance, (int)(Plugin.GetConfig().Scp079BlackoutCost * Plugin.GetConfig().Scp079BlackoutCostsPerZoneMultiplier[controller.Room.Zone]));
                
            }catch(Exception e)
            {
                Log.Debug("Error in Prefix");
                Log.Error(e.ToString());
            }
        }

        //public static void Postfix(Scp079BlackoutRoomAbility __instance)
        //{
        //    try
        //    {
        //        RoomLightController controller = (RoomLightController)AccessTools.Field(typeof(Scp079BlackoutRoomAbility), "_roomController").GetValue(__instance);
        //        var blackoutCooldownsField = AccessTools.Field(typeof(Scp079BlackoutRoomAbility), "_blackoutCooldowns");
        //        float cooldown = (float)AccessTools.Field(typeof(Scp079BlackoutRoomAbility), "_cooldown").GetValue(__instance);
        //        float blackoutDuration = (float)AccessTools.Field(typeof(Scp079BlackoutRoomAbility), "_blackoutDuration").GetValue(__instance);
        //        Dictionary<uint, double> blackoutCooldowns = (Dictionary<uint, double>)blackoutCooldownsField.GetValue(__instance);
        //        if (controller.Room.Zone == MapGeneration.FacilityZone.Surface)
        //        {
        //            blackoutCooldowns[controller.netId] = NetworkTime.time + blackoutDuration + Plugin.GetConfig().Scp079SurfaceZoneBlackoutCooldown;
        //        }
        //        blackoutCooldownsField.SetValue(__instance, blackoutCooldowns);
        //    }catch(Exception ex) 
        //    {
        //        Log.Debug("Error in Postfix");
        //        Log.Error(ex.ToString());
        //    }
        //}
    }
}

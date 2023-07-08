using BetterSL.EventHandlers;
using GameCore;
using HarmonyLib;
using Interactables.Interobjects.DoorUtils;
using MEC;
using PlayerRoles.PlayableScps.Scp079;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using BetterSL.EventHandlers.Scp079;

namespace BetterSL.Patches.Scp079
{
    [HarmonyPatch(typeof(Scp079DoorLockChanger), nameof(Scp079DoorLockChanger.LockClosedDoorCost), MethodType.Getter)]
    public class Scp079DoorLockCostPatch
    {
        public static void Prefix(Scp079DoorLockChanger __instance, ref int __result)
        {
            bool trygetauxmanager = __instance.ScpRole.SubroutineModule.TryGetSubroutine<Scp079AuxManager>(out var auxmanager);
            bool trygettiermanager = __instance.ScpRole.SubroutineModule.TryGetSubroutine<Scp079TierManager>(out var tiermanager);
            if (!trygettiermanager || !trygetauxmanager)
            {
                PluginAPI.Core.Log.Error("Tier or Aux managers are null!");
                return;
            }
            float cost = auxmanager.MaxAux * Plugin.GetConfig().Scp079DoorLockCostPercent[tiermanager.AccessTierIndex];
            var lockCostField = AccessTools.Field(typeof(Scp079DoorLockChanger), "_lockCostPerSec");
            lockCostField.SetValue(__instance, cost);
            return;
        }
    }
}

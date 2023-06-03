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
    [HarmonyPatch(typeof(Scp079DoorLockChanger), "SetDoorLock")]
    public class Scp079DoorLockCostPatch
    {
        public static bool CoroutineRunning = false;
        public static CoroutineHandle Handle;

        public static void Postfix(Scp079DoorLockChanger __instance, ref bool __result)
        {
            if(__result && !CoroutineRunning && !Handle.IsRunning)
            {
                Handle = Timing.CallPeriodically(float.PositiveInfinity, 1f, () =>
                {
                    CoroutineRunning = true;
                    if(Scp079DoorHandler.DoorsLocked == 0)
                    {
                        return;
                    }
                    bool trygetauxmanager = __instance.ScpRole.SubroutineModule.TryGetSubroutine<Scp079AuxManager>(out var auxmanager);
                    bool trygettiermanager = __instance.ScpRole.SubroutineModule.TryGetSubroutine<Scp079TierManager>(out var tiermanager);
                    if (!trygettiermanager || !trygetauxmanager)
                    {
                        PluginAPI.Core.Log.Error("Tier or Aux managers are null!");
                        return;
                    }
                    int cost = (int)(auxmanager.MaxAux * Plugin.GetConfig().Scp079DoorLockCostPercent[tiermanager.AccessTierIndex]);
                    auxmanager.CurrentAux -= cost;
                });
            }
            return;
        }
    }
}

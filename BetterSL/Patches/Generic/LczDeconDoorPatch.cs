using BetterSL.Resources;
using HarmonyLib;
using Interactables.Interobjects;
using Interactables.Interobjects.DoorUtils;
using LightContainmentZoneDecontamination;
using MapGeneration;
using Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetterSL.Patches.Generic
{
    [HarmonyPatch(typeof(DecontaminationController), "UpdateTime")]
    public class LczDeconDoorPatch
    {
        public static void Prefix(DecontaminationController __instance)
        {
            int nextPhase = (int)AccessTools.Field(typeof(DecontaminationController), "_nextPhase").GetValue(__instance);
            if (NetworkServer.active && __instance.DecontaminationPhases[nextPhase].Function == DecontaminationController.DecontaminationPhase.PhaseFunction.OpenCheckpoints)
            {
                List<DoorVariant> lczDoors = Resources.Extensions.GetDoorsByZone(FacilityZone.LightContainment);
                foreach(DoorVariant door in lczDoors)
                {
                    if(door is CheckpointDoor)
                    {
                        continue;
                    }
                    door.ServerChangeLock(DoorLockReason.DecontLockdown, true);
                    door.NetworkTargetState = true;
                }
            }
        }
    }
}

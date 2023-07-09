using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PluginAPI.Events;
using PluginAPI.Core;
using PluginAPI.Core.Attributes;
using PluginAPI.Enums;
using Interactables.Interobjects.DoorUtils;
using MapGeneration;
using Interactables.Interobjects;
using MEC;
using HarmonyLib;

namespace BetterSL.EventHandlers.Generic
{
    public class Hcz049Handler
    {
        private static int _generatorsActivated = 0;
        private static PryableDoor _targetDoor = null;

        [PluginEvent(ServerEventType.RoundStart)]
        public void OnRoundStart(RoundStartEvent ev)
        {
            _generatorsActivated = 0;
            _targetDoor = null;
            if(!Plugin.GetConfig().Lock049GateUntilGeneratorsAreActive)
            {
                return;
            }
            List<DoorVariant> alldoors = DoorVariant.AllDoors.Where(x => RoomIdUtils.RoomAtPositionRaycasts(x.transform.position).Name == RoomName.Hcz049).ToList();
            if (Plugin.GetConfig().Hcz049ArmoryDoesNotNeedKeycard)
            {
                foreach (DoorVariant door in DoorVariant.AllDoors)
                {
                    if (door.TryGetComponent<DoorNametagExtension>(out DoorNametagExtension dne) && dne.GetName == "049_ARMORY")
                    {
                        door.RequiredPermissions = new DoorPermissions() { RequiredPermissions = KeycardPermissions.None };
                    }
                }
            }
            List<DoorVariant> pryableDoors = alldoors.Where(x => x is PryableDoor).ToList();
            float lowesty = float.MaxValue;
            PryableDoor targetDoor = null;
            foreach(PryableDoor door in pryableDoors)
            {
                if(door.transform.position.y < lowesty)
                {
                    lowesty = door.transform.position.y;
                    targetDoor = door;
                }
            }
            targetDoor.ServerChangeLock(DoorLockReason.AdminCommand, true);
            targetDoor.NetworkTargetState = false;
            _targetDoor = targetDoor;
        }

        [PluginEvent(ServerEventType.GeneratorActivated)]
        public void OnGeneratorActivated(GeneratorActivatedEvent ev)
        {
            Timing.CallDelayed(1.5f, () =>
            {
                if (ev.Generator.Engaged)
                {
                    _generatorsActivated++;
                }
                if (_generatorsActivated >= Plugin.GetConfig().Unlock049GateGeneratorEngagedAmount)
                {
                    _targetDoor.ServerChangeLock(DoorLockReason.AdminCommand, false);
                    _targetDoor.NetworkTargetState = true;
                }
            });
        }
    }
}

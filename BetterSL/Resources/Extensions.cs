using Interactables.Interobjects;
using Interactables.Interobjects.DoorUtils;
using MapGeneration;
using PluginAPI.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BetterSL.Resources
{
    public class Extensions
    {
        public static bool InElevator(Vector3 position)
        {
            List<ElevatorDoor> AllElevs = new List<ElevatorDoor>();
            if (ElevatorDoor.AllElevatorDoors.TryGetValue(ElevatorManager.ElevatorGroup.LczB01, out var list))
            {
                AllElevs.AddRange(list);
            }
            if (ElevatorDoor.AllElevatorDoors.TryGetValue(ElevatorManager.ElevatorGroup.LczA01, out var list1))
            {
                AllElevs.AddRange(list1);
            }
            if (ElevatorDoor.AllElevatorDoors.TryGetValue(ElevatorManager.ElevatorGroup.LczA02, out var list2))
            {
                AllElevs.AddRange(list2);
            }
            if (ElevatorDoor.AllElevatorDoors.TryGetValue(ElevatorManager.ElevatorGroup.LczB02, out var list3))
            {
                AllElevs.AddRange(list3);
            }
            if (ElevatorDoor.AllElevatorDoors.TryGetValue(ElevatorManager.ElevatorGroup.Scp049, out var list4))
            {
                AllElevs.AddRange(list4);
            }
            if (ElevatorDoor.AllElevatorDoors.TryGetValue(ElevatorManager.ElevatorGroup.Nuke, out var list5))
            {
                AllElevs.AddRange(list5);
            }
            if (ElevatorDoor.AllElevatorDoors.TryGetValue(ElevatorManager.ElevatorGroup.GateA, out var list6))
            {
                AllElevs.AddRange(list6);
            }
            if (ElevatorDoor.AllElevatorDoors.TryGetValue(ElevatorManager.ElevatorGroup.GateB, out var list7))
            {
                AllElevs.AddRange(list7);
            }
            foreach (var elevdoors in AllElevs)
            {
                ElevatorChamber chamber = elevdoors.TargetPanel.AssignedChamber;
                if (chamber.WorldspaceBounds.Contains(position))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool InDoor(Vector3 position)
        {
            RoomIdentifier room  = RoomIdUtils.RoomAtPositionRaycasts(position);
            if(room == null)
            {
                Log.Warning("Room is null!");
            }
            foreach(DoorVariant var in DoorVariant.DoorsByRoom[room])
            {
                Transform transform = var.transform;
                BoxCollider collider;
                while (!transform.TryGetComponent<BoxCollider>(out collider))
                {
                    transform = var.transform.parent;
                    if (collider != null)
                    {
                        Log.Debug("Got Collider!");
                        return collider.bounds.Contains(position);
                    }
                }
            }
            return false;
        }
    }
}

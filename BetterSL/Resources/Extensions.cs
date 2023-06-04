﻿using HarmonyLib;
using Interactables.Interobjects;
using Interactables.Interobjects.DoorUtils;
using MapGeneration;
using PlayerRoles.PlayableScps.Scp106;
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

        public static bool Scp106InDoor(Vector3 position, Scp106MovementModule reference)
        {
            HashSet<Collider> colliders = (HashSet<Collider>)AccessTools.Field(typeof(Scp106MovementModule), "EnabledColliders").GetValue(reference);
            foreach (var collider in colliders)
            {
                if (collider.bounds.Contains(position))
                {
                    Log.Debug("Found collider with position in it!");
                    return true;
                }
                Log.Debug("Position in collider does not exist!");
            }
            Log.Debug("Did not find collider with position in it.");
            return false;
        }
    }
}

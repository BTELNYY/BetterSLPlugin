using HarmonyLib;
using Interactables.Interobjects;
using Interactables.Interobjects.DoorUtils;
using InventorySystem;
using InventorySystem.Items.Firearms;
using InventorySystem.Items.Pickups;
using InventorySystem.Items;
using MapGeneration;
using PlayerRoles;
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

        public static List<ReferenceHub> GetScpTeam()
        {
            return ReferenceHub.AllHubs.Where(x => x.IsSCP()).ToList();
        }

        public static List<ReferenceHub> GetByTeam(Team team)
        {
            return ReferenceHub.AllHubs.Where(x => x.roleManager.CurrentRole.Team == team).ToList();
        }

        public static int GetAlivePlayers()
        {
            return ReferenceHub.AllHubs.Where(x => x.IsAlive()).Count();
        }

        public static void BroadcastToTeam(Team team, string message)
        {
            List<ReferenceHub> hubs = GetByTeam(team);
            foreach (var hub in hubs)
            {
                Server.Broadcast.TargetAddElement(hub.connectionToClient, message, 5, Broadcast.BroadcastFlags.Normal);
            }
        }

        /// <summary>
        /// Removes ONLY ONE item of the type from the player
        /// </summary>
        /// <param name="target">
        /// Player to target
        /// </param>
        /// <param name="type">
        /// ItemType to remove
        /// </param>
        /// <returns>
        /// Wether or not the item was removed
        /// </returns>
        public static bool RemoveItemFromPlayer(Player target, ItemType type)
        {
            foreach (var item in target.ReferenceHub.inventory.UserInventory.Items.Keys)
            {
                if (target.ReferenceHub.inventory.UserInventory.Items[item].ItemTypeId == type)
                {
                    target.ReferenceHub.inventory.ServerRemoveItem(item, target.ReferenceHub.inventory.UserInventory.Items[item].PickupDropModel);
                    return true;
                }
            }
            return false;
        }

        public static List<DoorVariant> GetDoorsByZone(FacilityZone zone)
        {
            List<DoorVariant> doors = DoorVariant.AllDoors.Where(x => x.IsInZone(zone)).ToList();
            return doors;
        }

        public static ItemPickupBase CreateItemPickup(ItemType type, Vector3 Position)
        {
            ItemBase itemBase = ReferenceHub.HostHub.inventory.ServerAddItem(type);
            ItemPickupBase itemPickup = itemBase.ServerDropItem();
            itemPickup.transform.position = Position;
            itemPickup.transform.rotation = Quaternion.Euler(Vector3.up);
            return itemPickup;
        }

        public static ItemPickupBase CreateFirearmPickup(ItemType type, Vector3 Position, byte ammo)
        {
            ItemBase itemBase = ReferenceHub.HostHub.inventory.ServerAddItem(type);
            if (itemBase is Firearm)
            {
                Firearm firearm = (Firearm)itemBase;
                FirearmStatus status = new FirearmStatus(ammo, firearm.Status.Flags, firearm.Status.Attachments);
                firearm.Status = status;
                ItemPickupBase firearmitem = firearm.ServerDropItem();
                firearmitem.transform.position = Position;
                firearmitem.transform.rotation = Quaternion.Euler(Vector3.up);
                return firearmitem;
            }
            else
            {
                Log.Warning("Item ID is not a firearm!", nameof(Extensions.CreateFirearmPickup));
            }
            ItemPickupBase itemPickup = itemBase.ServerDropItem();
            itemPickup.transform.position = Position;
            itemPickup.transform.rotation = Quaternion.Euler(Vector3.up);
            return itemPickup;
        }

        public static ItemPickupBase CreateFirearmPickup(ItemType type, Vector3 Position, byte ammo, uint attachments)
        {
            ItemBase itemBase = ReferenceHub.HostHub.inventory.ServerAddItem(type);
            if (itemBase is Firearm)
            {
                Firearm firearm = (Firearm)itemBase;
                FirearmStatus status = new FirearmStatus(ammo, firearm.Status.Flags, attachments);
                firearm.Status = status;
                ItemPickupBase firearmitem = firearm.ServerDropItem();
                firearmitem.transform.position = Position;
                firearmitem.transform.rotation = Quaternion.Euler(Vector3.up);
                return firearmitem;
            }
            else
            {
                Log.Warning("Item ID is not a firearm!", nameof(Extensions.CreateFirearmPickup));
            }
            ItemPickupBase itemPickup = itemBase.ServerDropItem();
            itemPickup.transform.position = Position;
            itemPickup.transform.rotation = Quaternion.Euler(Vector3.up);
            return itemPickup;
        }
    }
}

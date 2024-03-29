﻿using HarmonyLib;
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
using InventorySystem.Items.Firearms.Attachments;
using InventorySystem.Items.Firearms.Ammo;
using InventorySystem.Disarming;

namespace BetterSL.Resources
{
    public static class Extensions
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
        public static bool RemoveItemFromPlayer(this Player target, ItemType type)
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

        public static List<ReferenceHub> GetPlayersByZone(this FacilityZone zone)
        {
            return ReferenceHub.AllHubs.Where(x => Player.Get(x).Zone == zone).ToList();
        }

        public static List<ReferenceHub> GetAllDisarmedPlayersByDisarmer(this ReferenceHub disarmer)
        {
            List<DisarmedPlayers.DisarmedEntry> disarmedEntries = DisarmedPlayers.Entries.Where(x => x.Disarmer == disarmer.networkIdentity.netId).ToList();
            List<uint> netIds = disarmedEntries.Select(x => x.DisarmedPlayer).ToList();
            List<ReferenceHub> hubs = ReferenceHub.AllHubs.Where(x => netIds.Contains(x.networkIdentity.netId)).ToList();
            return hubs;
        }

        public static List<DoorVariant> GetDoorsByZone(this FacilityZone zone)
        {
            List<DoorVariant> doors = DoorVariant.AllDoors.Where(x => x.IsInZone(zone)).ToList();
            return doors;
        }

        public static ItemPickupBase CreateItemPickup(this ItemType type, Vector3 Position)
        {
            ItemBase itemBase = ReferenceHub.HostHub.inventory.ServerAddItem(type);
            ItemPickupBase itemPickup = itemBase.ServerDropItem();
            itemPickup.transform.position = Position;
            itemPickup.transform.rotation = Quaternion.Euler(Vector3.up);
            return itemPickup;
        }

        public static ItemPickupBase CreateFirearmPickup(this ItemType type, Vector3 Position, byte ammo)
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

        public static ItemPickupBase CreateFirearmPickup(this ItemType type, Vector3 Position, byte ammo, uint attachments)
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

        public static ItemPickupBase CreateAmmoPickup(this ItemType type, Vector3 Position, int ammo)
        {
            ItemBase itemBase = ReferenceHub.HostHub.inventory.ServerAddItem(type);
            if (itemBase is AmmoItem)
            {
                AmmoItem ammoItem = (AmmoItem)itemBase;
                ammoItem.UnitPrice = ammo;
                ItemPickupBase ammoItemPickup = ammoItem.ServerDropItem();
                ammoItemPickup.transform.position = Position;
                ammoItemPickup.transform.rotation = Quaternion.Euler(Vector3.up);
                return ammoItemPickup;
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

        public static DoorVariant GetDoorBasedOnName(string name)
        {
            foreach (DoorVariant door in DoorVariant.AllDoors)
            {
                if (door.TryGetComponent<DoorNametagExtension>(out DoorNametagExtension dne) && dne.GetName == name)
                {
                    return door;
                }
            }
            return null;
        }

        public static void ApplyAttachments(this Player ply)
        {
            var item = ply.Items.Where(i => i is Firearm);

            foreach (var fire in item)
            {
                if (fire is Firearm fireArm)
                {
                    if (AttachmentsServerHandler.PlayerPreferences.TryGetValue(ply.ReferenceHub, out var value) && value.TryGetValue(fireArm.ItemTypeId, out var value2))
                        fireArm.ApplyAttachmentsCode(value2, reValidate: true);
                    var firearmStatusFlags = FirearmStatusFlags.MagazineInserted;
                    if (fireArm.HasAdvantageFlag(AttachmentDescriptiveAdvantages.Flashlight))
                        firearmStatusFlags |= FirearmStatusFlags.FlashlightEnabled;

                    fireArm.Status = new FirearmStatus(fireArm.AmmoManagerModule.MaxAmmo, firearmStatusFlags, fireArm.GetCurrentAttachmentsCode());
                }
            }
        }

        public static T GetRandomElementFromList<T>(this List<T> list)
        {
            System.Random random = new System.Random();
            int index = random.Next(list.Count);
            return list[index];
        }
    }
}

using InventorySystem.Items.Pickups;
using InventorySystem.Items;
using InventorySystem;
using PluginAPI.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using InventorySystem.Items.Firearms;

namespace BetterSL
{
    public class Utility
    {
        public static void CreateItemPickup(ItemType type, Vector3 Position)
        {
            ItemBase itemBase = ReferenceHub.HostHub.inventory.ServerAddItem(type);
            ItemPickupBase itemPickup = itemBase.ServerDropItem();
            itemPickup.transform.position = Position;
            itemPickup.transform.rotation = Quaternion.Euler(Vector3.up);
        }

        public static void CreateFirearmPickup(ItemType type, Vector3 Position, byte ammo) 
        {
            ItemBase itemBase = ReferenceHub.HostHub.inventory.ServerAddItem(type);
            if(itemBase is Firearm)
            {
                Firearm firearm = (Firearm)itemBase;
                FirearmStatus status = new FirearmStatus(ammo, firearm.Status.Flags, firearm.Status.Attachments);
                firearm.Status = status;
                ItemPickupBase firearmitem = firearm.ServerDropItem();
                firearmitem.transform.position = Position;
                firearmitem.transform.rotation = Quaternion.Euler(Vector3.up);
                return;
            }
            else
            {
                Log.Warning("Item ID is not a firearm!", nameof(Utility.CreateFirearmPickup));
            }
            ItemPickupBase itemPickup = itemBase.ServerDropItem();
            itemPickup.transform.position = Position;
            itemPickup.transform.rotation = Quaternion.Euler(Vector3.up);
        }

        public static void CreateFirearmPickup(ItemType type, Vector3 Position, byte ammo, uint attachments)
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
                return;
            }
            else
            {
                Log.Warning("Item ID is not a firearm!", nameof(Utility.CreateFirearmPickup));
            }
            ItemPickupBase itemPickup = itemBase.ServerDropItem();
            itemPickup.transform.position = Position;
            itemPickup.transform.rotation = Quaternion.Euler(Vector3.up);
        }
    }
}

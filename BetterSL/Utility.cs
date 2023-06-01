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
    }
}

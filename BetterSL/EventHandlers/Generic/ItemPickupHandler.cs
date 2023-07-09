using InventorySystem.Items.Pickups;
using MapGeneration;
using PluginAPI.Core;
using PluginAPI.Core.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BetterSL.EventHandlers.Generic
{
    public class ItemPickupHandler
    {
        public static void Init()
        {
            ItemPickupBase.OnPickupAdded += LczArmorySwapItems;
        }

        static Dictionary<Vector3, ItemPickupBase> ModifiedItems = new Dictionary<Vector3, ItemPickupBase>();

        public static void LczArmorySwapItems(ItemPickupBase item)
        {
            if(ModifiedItems.ContainsKey(item.Position))
            { return; }
            if (RoomIdUtils.RoomAtPositionRaycasts(item.Position).Name != RoomName.LczArmory)
            {
                return;
            }
            if (item.NetworkInfo.ItemId == ItemType.GunCrossvec)
            {
                if (!Round.IsRoundStarted && Plugin.GetConfig().LczArmoryReplaceCrossvecWithFSP9)
                {
                    ReplaceItem(item, ItemType.GunFSP9, 30);
                }
            }
            if(item.NetworkInfo.ItemId == ItemType.ArmorHeavy)
            {
                if(!Round.IsRoundStarted && Plugin.GetConfig().LczArmoryDowngradeArmor)
                {
                    ReplaceItem(item, ItemType.ArmorCombat);
                }
            }
            if (item.NetworkInfo.ItemId == ItemType.ArmorCombat)
            {
                if (!Round.IsRoundStarted && Plugin.GetConfig().LczArmoryDowngradeArmor)
                {
                    ReplaceItem(item, ItemType.ArmorLight);
                }
            }
        }

        private static void ReplaceItem(ItemPickupBase original, ItemType newType)
        {
            ItemPickupBase newItem = Utility.CreateItemPickup(newType, original.Position);
            original.DestroySelf();
            ModifiedItems.Add(newItem.Position, newItem);
        }

        private static void ReplaceItem(ItemPickupBase original, ItemType newType, byte ammo)
        {
            ItemPickupBase newItem = Utility.CreateFirearmPickup(newType, original.Position, ammo);
            original.DestroySelf();
            ModifiedItems.Add(newItem.Position, newItem);
        }
    }
}

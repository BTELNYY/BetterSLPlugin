using InventorySystem.Items.Pickups;
using MapGeneration;
using PluginAPI.Core;
using PluginAPI.Core.Items;
using PluginAPI.Events;
using PluginAPI.Core.Attributes;
using PluginAPI.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using InventorySystem.Items.Firearms.Attachments;

namespace BetterSL.EventHandlers.Generic
{
    public class ItemSpawnHandler
    {
        public static void Init()
        {
            ItemPickupBase.OnPickupAdded += LczArmorySwapItems;
            ItemPickupBase.OnPickupAdded += Hcz049ReplaceGuardKeycard;
        }

        static Dictionary<Vector3, ItemPickupBase> ModifiedItems = new Dictionary<Vector3, ItemPickupBase>();

        private static void LczArmorySwapItems(ItemPickupBase item)
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

        private static void Hcz049ReplaceGuardKeycard(ItemPickupBase item)
        {
            if (ModifiedItems.ContainsKey(item.Position)) { return; }
            if (RoomIdUtils.RoomAtPositionRaycasts(item.Position).Name != RoomName.Hcz049) { return; }
            if(item.NetworkInfo.ItemId != ItemType.KeycardGuard) { return; }
            if(Plugin.GetConfig().FacilityManagerReplacesGuardCardIn049 != true) { return; }
            ReplaceItem(item, ItemType.KeycardFacilityManager);
        }


        private static void ReplaceItem(ItemPickupBase original, ItemType newType)
        {
            ItemPickupBase newItem = Resources.Extensions.CreateItemPickup(newType, original.Position);
            original.DestroySelf();
            ModifiedItems.Add(newItem.Position, newItem);
        }

        private static void ReplaceItem(ItemPickupBase original, ItemType newType, byte ammo)
        {
            ItemPickupBase newItem = Resources.Extensions.CreateFirearmPickup(newType, original.Position, ammo);
            original.DestroySelf();
            ModifiedItems.Add(newItem.Position, newItem);
        }

        [PluginEvent(ServerEventType.RoundStart)]
        public void OnRoundStart(RoundStartEvent ev)
        {
            List<WorkstationController> HczArmoryController = WorkstationController.AllWorkstations.Where(x  => (RoomIdUtils.RoomAtPositionRaycasts(x.transform.position).Name == RoomName.HczArmory)).ToList();
            if(!Plugin.GetConfig().HczArmorySpawnCombatArmor)
            {
                return;
            }
            Vector3 NewPos = HczArmoryController.FirstOrDefault().transform.position;
            NewPos.y += 2.5f;
            for (int i = 0; i < Plugin.GetConfig().HczArmoryCombatArmorAmount; i++) 
            {
                Resources.Extensions.CreateItemPickup(ItemType.ArmorCombat, NewPos);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InventorySystem.Items;
using InventorySystem.Items.Usables;
using PlayerStatsSystem;
using PluginAPI.Core;
using PluginAPI.Events;

namespace BetterSL.EventHandlers.Items
{
    public class Scp500UseHandler
    {
        [PluginAPI.Core.Attributes.PluginEvent(PluginAPI.Enums.ServerEventType.PlayerUsedItem)]
        public void OnUseItem(PlayerUsedItemEvent ev)
        {
            ItemBase item = ev.Item;
            Player player = ev.Player;
            if (!Plugin.GetConfig().Scp500GivesStamina)
            {
                return;
            }
            if(item.ItemTypeId != ItemType.SCP500)
            {
                return;
            }
            player.ReferenceHub.playerStats.GetModule<StaminaStat>().ModifyAmount(1f);
        }
    }
}

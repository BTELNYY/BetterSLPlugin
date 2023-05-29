using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InventorySystem.Items;
using InventorySystem.Items.Usables;
using PlayerStatsSystem;
using PluginAPI.Core;

namespace BetterSL.EventHandlers
{
    public class Scp500UseHandler
    {
        [PluginAPI.Core.Attributes.PluginEvent(PluginAPI.Enums.ServerEventType.PlayerUsedItem)]
        public void OnUseItem(Player player, ItemBase item)
        {
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

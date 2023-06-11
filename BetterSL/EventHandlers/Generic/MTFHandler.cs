using PlayerRoles;
using PluginAPI.Core.Attributes;
using PluginAPI.Core;
using PluginAPI.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InventorySystem;

namespace BetterSL.EventHandlers.Generic
{
    public class MTFHandler
    {
        [PluginEvent(ServerEventType.PlayerSpawn)]
        public void OnPlayerSpawned(Player player, RoleTypeId role)
        {
            if(role == RoleTypeId.NtfPrivate)
            {
                
                //player.ReferenceHub.inventory.ServerRemoveItem();
            }
        }
    }
}

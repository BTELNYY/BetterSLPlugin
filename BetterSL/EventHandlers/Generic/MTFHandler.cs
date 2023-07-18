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
using PluginAPI.Events;
using BetterSL.Resources;
using MEC;

namespace BetterSL.EventHandlers.Generic
{
    public class MTFHandler
    {
        [PluginEvent(ServerEventType.PlayerSpawn)]
        public void OnPlayerSpawned(PlayerSpawnEvent ev)
        {
            Timing.CallDelayed(0.5f, () => 
            {
                Player player = ev.Player;
                RoleTypeId role = ev.Role;
                if (role == RoleTypeId.NtfPrivate)
                {
                    if(Extensions.RemoveItemFromPlayer(player, ItemType.KeycardNTFOfficer))
                    {
                        player.AddItem(ItemType.KeycardNTFLieutenant);
                    }
                }
            });
        }
    }
}

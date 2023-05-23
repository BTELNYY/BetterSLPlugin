using PluginAPI.Core;
using PluginAPI;
using PluginAPI.Enums;
using PluginAPI.Core.Attributes;
using PlayerStatsSystem;
using PlayerRoles;
using Scp914;
using PlayerRoles.FirstPersonControl;
using InventorySystem;
using InventorySystem.Items;
using InventorySystem.Items.Pickups;
using InventorySystem.Items.Usables.Scp330;
using InventorySystem.Items.Usables;
using System.Collections.Generic;
using CustomPlayerEffects;
using InventorySystem.Disarming;
using System;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace BetterSL.EventHandlers
{
    public class Scp106Handler
    {
        [PluginEvent(ServerEventType.PlayerChangeRole)]
        public void OnSpawn(Player player, PlayerRoleBase oldRole, RoleTypeId newRole, RoleChangeReason changeReason)
        {
            if(newRole == RoleTypeId.Scp106)
            {
                float damage = player.Health - Plugin.instance.config.Scp106StarterHealth;
                player.Damage(damage, "Auto balance damage");
            }
        }
    }
}

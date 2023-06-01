using PlayerRoles.PlayableScps.Scp079;
using PlayerRoles;
using PluginAPI.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandSystem.Commands.RemoteAdmin.Doors;
using Interactables.Interobjects.DoorUtils;
using UnityEngine;
using PlayerRoles.FirstPersonControl;
using MapGeneration;
using MEC;
using Interactables.Interobjects;
using InventorySystem;
using InventorySystem.Items.Keycards;
using InventorySystem.Items;
using Mirror;
using InventorySystem.Items.Pickups;

namespace BetterSL.EventHandlers
{
    public class Scp049SpawnHandler
    {
        [PluginAPI.Core.Attributes.PluginEvent(PluginAPI.Enums.ServerEventType.PlayerSpawn)]
        public void OnSpawn(Player player, RoleTypeId role)
        {
            if(role != RoleTypeId.Scp049) { return; }
            Timing.CallDelayed(0.5f, () =>
            {
                if (Plugin.GetConfig().FacilityManagerSpawnsIn049)
                {
                    Utility.CreateItemPickup(ItemType.KeycardFacilityManager, player.Position);
                }
            });
            Timing.CallDelayed(1f, () =>
            {
                foreach (var room in RoomIdentifier.AllRoomIdentifiers)
                {
                    if (room.name == "HCZ_049")
                    {
                        Vector3 pos = room.gameObject.transform.position;
                        pos.y += 2f;
                        player.Position = pos;
                        if (!player.ReferenceHub.TryOverridePosition(pos, Vector3.forward))
                        {
                            Log.Warning("Failed to override position.");
                        }
                    }
                }
            });
        }
    }
}

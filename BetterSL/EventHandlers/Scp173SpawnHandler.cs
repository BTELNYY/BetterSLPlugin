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

namespace BetterSL.EventHandlers
{
    public class Scp173SpawnHandler
    {
        //TODO: Fix this, while the code can find the room, it refuses to teleport the player.
        [PluginAPI.Core.Attributes.PluginEvent(PluginAPI.Enums.ServerEventType.PlayerSpawn)]
        public void OnSpawn(Player player, RoleTypeId role)
        {
            if (role != RoleTypeId.Scp173) { return; }
            ////give server instance requiered permission(depends the command)
            //Server.Instance.ReferenceHub.serverRoles.Permissions = (ulong)PlayerPermissions.PlayersManagement;
            //DoorTPCommand cmd = new DoorTPCommand(); //cleanup command for example
            //string response = "";
            //string[] empty = { player.PlayerId.ToString(), "HCZ_ARMORY"};
            //cmd.Execute(new ArraySegment<string>(empty), new RemoteAdmin.PlayerCommandSender(Server.Instance.ReferenceHub), out response);
            //Log.Info(response);
            Timing.CallDelayed(1f, () =>
            {
                foreach (var room in RoomIdentifier.AllRoomIdentifiers)
                {
                    if (room.name == "HCZ_Nuke")
                    {
                        var door = DoorVariant.DoorsByRoom[room].FirstOrDefault();
                        foreach (var thing in DoorVariant.DoorsByRoom[room])
                        {
                            if(thing.name == "LCZ PortallessBreakableDoor")
                            {
                                door = thing;
                            }
                        }
                        Vector3 pos = door.gameObject.transform.position;
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

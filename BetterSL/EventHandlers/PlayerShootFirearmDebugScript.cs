using InventorySystem.Items.Firearms;
using MapGeneration;
using PluginAPI.Core;
using UnityEngine;

namespace BetterSL.EventHandlers
{
    public class PlayerShootFirearmDebugScript
    {
        //debug event, uncomment if needed for specific room stuff
        //[PluginAPI.Core.Attributes.PluginEvent(PluginAPI.Enums.ServerEventType.PlayerShotWeapon)]
        public void OnFire(Player player, Firearm firearm)
        {
            RoomIdentifier roomIdentifier = RoomIdUtils.RoomAtPositionRaycasts(player.Position, true);
            if (roomIdentifier == null)
            {
                Log.Debug("Null room.");
            }
            else
            {
                Log.Debug(roomIdentifier.name);
            }
            // Does the ray intersect any objects excluding the player layer
            if (Physics.Raycast(player.GameObject.transform.position, player.GameObject.transform.TransformDirection(Vector3.forward), out RaycastHit hit, Mathf.Infinity))
            {
                Log.Debug("Did Hit");
                Log.Debug(hit.transform.gameObject.name);
            }
            else
            {
                Log.Debug("Did not Hit");
            }
        }
    }
}

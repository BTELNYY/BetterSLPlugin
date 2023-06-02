using PlayerRoles.FirstPersonControl;
using PlayerStatsSystem;
using PluginAPI.Core;
using PluginAPI.Core.Attributes;
using PluginAPI.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MEC;
using UnityEngine;
using BetterSL.Resources;
using Mirror;
using MapGeneration;
using PlayerRoles;
using BetterSL.Managers;

namespace BetterSL.EventHandlers
{
    public class DimensionBodyHandler
    {
        public static void OnRagdoll(BasicRagdoll ragdoll)
        {
            BasicRagdoll rag = ragdoll;
            DamageHandlerBase damageHandler = ragdoll.Info.Handler;
            UniversalDamageHandler un = damageHandler as UniversalDamageHandler;
            if(un.TranslationId != 7)
            {
                return;
            }
            int time = Plugin.GetConfig().Scp106BodyDropDelay;
            Timing.CallDelayed(time, () =>
            {
                List<Player> players = Player.GetPlayers();
                Log.Debug(players.Count.ToString());
                List<Player> checkedPlayers = players.Where(p => !p.IsSCP && p.IsAlive && !BetterSL.Resources.Extensions.InElevator(p.Position) && RoomIdUtils.RoomAtPositionRaycasts(p.Position) != null).ToList();
                Log.Debug(checkedPlayers.Count.ToString());
                Player chosenPlayer = checkedPlayers.RandomItem();
                Log.Debug(chosenPlayer.Nickname);
                Vector3 raycastPos = chosenPlayer.Camera.position;
                raycastPos.y += 0.5f;
                Vector3 teleportPos = Vector3.zero;
                if (!Physics.Raycast(raycastPos, Vector3.up, out RaycastHit hit, float.PositiveInfinity))
                {
                    Log.Warning("Failed to find object in checked player!");
                    teleportPos = chosenPlayer.Camera.position;
                }
                else
                {
                    teleportPos = hit.transform.position;
                    teleportPos.y -= 0.3f;
                }
                DummyManager.CloneRagdoll(ragdoll, teleportPos);
            });
        }
    }
}

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

namespace BetterSL.EventHandlers.Generic
{
    public class DimensionBodyHandler
    {
        public static bool ThreadStarted = false;

        public static CoroutineHandle Handler;

        public static List<BasicRagdoll> RagdollQueue = new List<BasicRagdoll>();

        public static void OnRagdoll(BasicRagdoll ragdoll)
        {
            //Log.Debug("Ragdoll handler called!");
            BasicRagdoll rag = ragdoll;
            DamageHandlerBase damageHandler = ragdoll.Info.Handler;
            UniversalDamageHandler un = damageHandler as UniversalDamageHandler;
            //Log.Debug("Ready to call delayed body handler!");
            if (un.TranslationId != 7)
            {
                //Log.Debug("Invalid translation ID!");
                return;
            }
            //Log.Debug("Adding to ragdoll queue!");
            RagdollQueue.Add(rag);
            if (!ThreadStarted || Handler == null || !Handler.IsRunning)
            {
                ThreadStarted = true;
                //Log.Debug("Starting ragdoll thread..");
                int time = Plugin.GetConfig().Scp106BodyDropDelay;
                Handler = Timing.CallPeriodically(float.PositiveInfinity, time, () =>
                {
                    //Log.Debug("Calling Timing!");
                    if(RagdollQueue.Count == 0)
                    {
                        //Log.Debug("Empty list");
                        return;
                    }
                    BasicRagdoll queueragdoll = RagdollQueue.RandomItem();
                    //Log.Debug("Calling delayed body drop!");
                    List<Player> players = Player.GetPlayers();
                    //Log.Debug(players.Count.ToString());
                    List<Player> checkedPlayers = players.Where(p => !p.IsSCP && p.IsAlive && !BetterSL.Resources.Extensions.InElevator(p.Position) && RoomIdUtils.RoomAtPositionRaycasts(p.Position) != null).ToList();
                    //Log.Debug(checkedPlayers.Count.ToString());
                    if(checkedPlayers.Count <= 0) 
                    {
                        //No valid players, try again next time.
                        return;
                    }
                    Player chosenPlayer = checkedPlayers.RandomItem();
                    //Log.Debug(chosenPlayer.Nickname);
                    Vector3 raycastPos = chosenPlayer.Camera.position;
                    raycastPos.y += 0.5f;
                    Vector3 teleportPos;
                    if (!Physics.Raycast(raycastPos, Vector3.up, out RaycastHit hit, float.PositiveInfinity))
                    {
                        Log.Warning("Failed to find object in checked player!");
                        teleportPos = chosenPlayer.Camera.position;
                    }
                    else
                    {
                        teleportPos = hit.point;
                        teleportPos.y -= 1f;
                    }
                    DummyManager.CloneRagdoll(queueragdoll, teleportPos);
                    RagdollQueue.Remove(queueragdoll);
                    Timing.CallDelayed(1f, () =>
                    {
                        NetworkServer.Destroy(queueragdoll.gameObject);
                    });
                });
            }
        }
    }
}

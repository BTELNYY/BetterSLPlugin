using System;
using System.Collections.Generic;
using PluginAPI.Core.Attributes;
using PluginAPI.Core;
using PluginAPI.Enums;
using Respawning;
using PlayerStatsSystem;
using PlayerRoles;
using MEC;
using System.Linq;
using PluginAPI.Events;

namespace BetterSL.EventHandlers.Generic
{
    public class ChaosTargetHandler
    {
        public static int ChaosTotal = 0;
        public static int ChaosAlive = 0;

        //public static bool DummySpawned = false;
        //public static ReferenceHub Dummy;

        //[PluginEvent(ServerEventType.RoundEndConditionsCheck)]
        //public static void CheckRoundEnd(bool baseConditionsSatisfied)
        //{
        //    //if there are less chaos alive then what we need
        //    if((ChaosTotal * Plugin.GetConfig().ChaosDeadPercent) > ChaosAlive)
        //    {
        //        //Log.Debug("Amount of chaos alive is less then needed to end round");
        //        if(DummySpawned && Dummy != null) 
        //        {
        //            DummySpawned = false;
        //            Dummy.characterClassManager.GodMode = false;
        //            Dummy.playerStats.DealDamage(new CustomReasonDamageHandler("Auto kill", -1f));
        //            NetworkServer.Destroy(Dummy.gameObject);
        //        }
        //        return;
        //    }
        //    else
        //    {
        //        DummySpawned = true;
        //    }
        //}

        [PluginEvent(ServerEventType.RoundEndConditionsCheck)]
        public RoundEndConditionsCheckCancellationData OnRoundEndConditionsCheck(RoundEndConditionsCheckEvent ev)
        {
            if (ChaosAlive > (ChaosTotal * Plugin.GetConfig().ChaosDeadPercent))
            {
                if (Resources.Extensions.GetScpTeam().Count != 0)
                {
                    return RoundEndConditionsCheckCancellationData.Override(false);
                }
            }
            return RoundEndConditionsCheckCancellationData.Override(ev.BaseGameConditionsSatisfied);
        }

        [PluginEvent(ServerEventType.TeamRespawn)]
        public void ChaosWaveSpawned(TeamRespawnEvent ev)
        {
            SpawnableTeamType team = ev.Team;
            Timing.CallDelayed(1f, () =>
            {
                //Log.Debug("Team respawned!");
                if (team != SpawnableTeamType.ChaosInsurgency)
                {
                    return;
                }
                //Log.Debug("Chaos spawned!");
                List<ReferenceHub> chaos = BetterSL.Resources.Extensions.GetByTeam(Team.ChaosInsurgency);
                ChaosTotal = chaos.Count;
                ChaosAlive = ChaosTotal;
                //Log.Debug($"Alive: {ChaosAlive} Total: {ChaosTotal}");
                if (ChaosAlive > (ChaosTotal * Plugin.GetConfig().ChaosDeadPercent))
                {
                    if (BetterSL.Resources.Extensions.GetScpTeam().Count != 0)
                    {
                        //RoundSummary.RoundLock = true;
                        BetterSL.Resources.Extensions.BroadcastToTeam(Team.SCPs, Plugin.GetConfig().ChaosTargetsBroadcast);
                    }
                }
                else
                {
                    //RoundSummary.RoundLock = false;
                }
            });
            int dclassCount = BetterSL.Resources.Extensions.GetByTeam(Team.ClassD).Count;
            int chaosCount = BetterSL.Resources.Extensions.GetByTeam(Team.ChaosInsurgency).Count;
            int livingPlayers = BetterSL.Resources.Extensions.GetAlivePlayers();
            if ((dclassCount + chaosCount) == livingPlayers)
            {
                //RoundSummary.RoundLock = false;
            }
        }

        [PluginEvent(ServerEventType.PlayerDying)]
        public void OnPlayerDeath(PlayerDyingEvent ev)
        {
            Player player = ev.Player;
            int dclassCount = BetterSL.Resources.Extensions.GetByTeam(Team.ClassD).Count;
            int chaosCount = BetterSL.Resources.Extensions.GetByTeam(Team.ChaosInsurgency).Count;
            int livingPlayers = BetterSL.Resources.Extensions.GetAlivePlayers();
            if((dclassCount + chaosCount) == livingPlayers)
            {
                //RoundSummary.RoundLock = false;
            }
            //Log.Debug("Player dying!");
            if (player.Role.GetTeam() != Team.ChaosInsurgency)
            {
                return;
            }
            ChaosAlive--;
            //Log.Debug($"Alive: {ChaosAlive} Total: {ChaosTotal}");
            if (ChaosAlive > (ChaosTotal * Plugin.GetConfig().ChaosDeadPercent))
            {
                if (BetterSL.Resources.Extensions.GetScpTeam().Count != 0)
                {
                    //RoundSummary.RoundLock = true;
                    BetterSL.Resources.Extensions.BroadcastToTeam(Team.SCPs, Plugin.GetConfig().ChaosTargetsBroadcast);
                }
            }
            else
            {
                //RoundSummary.RoundLock = false;
            }
            if(ChaosAlive == 0)
            {
                //RoundSummary.RoundLock = false;
            }
        }

        [PluginEvent(ServerEventType.PlayerSpawn)]
        public void OnPlayerSpawned(PlayerSpawnEvent ev)
        {
            RoleTypeId role = ev.Role;
            //Log.Debug("Player spawned!");
            if(role != RoleTypeId.ChaosConscript)
            {
                return;
            }
            ChaosAlive++;
            ChaosTotal++;
            //Log.Debug($"Alive: {ChaosAlive} Total: {ChaosTotal}");
            if (ChaosAlive > (ChaosTotal * Plugin.GetConfig().ChaosDeadPercent) || ChaosTotal == 1)
            {
                //Log.Debug($"More alive chaos then aloud! Alive: {ChaosAlive} Total: {ChaosTotal}");
                if(BetterSL.Resources.Extensions.GetScpTeam().Count != 0)
                {
                    //RoundSummary.RoundLock = true;
                    BetterSL.Resources.Extensions.BroadcastToTeam(Team.SCPs, Plugin.GetConfig().ChaosTargetsBroadcast);
                }
            }
            else
            {
                //RoundSummary.RoundLock = false;
            }
            int dclassCount = BetterSL.Resources.Extensions.GetByTeam(Team.ClassD).Count;
            int chaosCount = BetterSL.Resources.Extensions.GetByTeam(Team.ChaosInsurgency).Count;
            int livingPlayers = BetterSL.Resources.Extensions.GetAlivePlayers();
            if ((dclassCount + chaosCount) == livingPlayers)
            {
                //RoundSummary.RoundLock = false;
            }
        }
    }
}

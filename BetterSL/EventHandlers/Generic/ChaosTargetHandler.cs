using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PluginAPI.Core.Attributes;
using PluginAPI.Core;
using PluginAPI.Enums;
using Respawning;
using Mono.Cecil;
using PlayerStatsSystem;
using PlayerRoles;
using BetterSL.Resources;
using Interactables.Interobjects;
using BetterSL.Managers;
using UnityEngine;
using Mirror;

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
        //        Log.Debug("Amount of chaos alive is less then needed to end round");
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

        [PluginEvent(ServerEventType.TeamRespawn)]
        public static void ChaosWaveSpawned(SpawnableTeamType team)
        {
            Log.Debug("Team respawned!");
            if(team != SpawnableTeamType.ChaosInsurgency) 
            {
                return;
            }
            List<ReferenceHub> chaos = BetterSL.Resources.Extensions.GetByTeam(Team.ChaosInsurgency);
            ChaosTotal = chaos.Count;
            ChaosAlive = ChaosTotal;
        }

        [PluginEvent(ServerEventType.PlayerDying)]
        public static void OnPlayerDeath(Player player, Player attacker, DamageHandlerBase damageHandler)
        {
            if(player.Role.GetTeam() != Team.ChaosInsurgency)
            {
                return;
            }
            ChaosAlive--;
        }

        [PluginEvent(ServerEventType.PlayerSpawn)]
        public static void OnPlayerSpawned(Player player, RoleTypeId role)
        {
            if(role != RoleTypeId.ChaosConscript)
            {
                return;
            }
            ChaosAlive++;
            ChaosTotal++;
        }
    }
}

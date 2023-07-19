using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BetterSL.Resources;
using Discord;
using Interactables.Interobjects.DoorUtils;
using MapGeneration;
using MEC;
using PluginAPI.Core;
using PluginAPI.Core.Attributes;
using PluginAPI.Enums;
using PluginAPI.Events;
using Respawning.NamingRules;
using Respawning;
using UnityEngine;

namespace BetterSL.EventHandlers.Generic
{
    public class FacilityGuardSpawnHandler
    {
        [PluginEvent(ServerEventType.RoundStart)]
        public void HandleGuardTimer(RoundStartEvent ev)
        {
            float spawnTime = UnityEngine.Random.Range(Plugin.GetConfig().GuardSpawnMinTime, Plugin.GetConfig().GuardSpawnMaxTime);
            //Log.Debug("Spawn in: " + spawnTime);
            Timing.CallDelayed(spawnTime, () =>
            {
                SpawnGuards();
            });
        }

        public static void SpawnGuards()
        {
            //Log.Debug("Spawning");
            List<DoorVariant> validDoors = new List<DoorVariant>();
            foreach(DoorVariant door in DoorVariant.AllDoors)
            {
                if (door.Rooms.Any(x => Plugin.GetConfig().GuardSpawnableRooms.Contains(x.Name)))
                {
                    validDoors.Add(door);
                }
            }
            //Log.Debug("Set all valid doors");
            List<ReferenceHub> spectators = Extensions.GetByTeam(PlayerRoles.Team.Dead);
            List<ReferenceHub> chosenPlayers = new List<ReferenceHub>();
            List<Player> spawnedPlayers = new List<Player>();
            for(int i = 0; i < Plugin.GetConfig().MaxGuardSpawns; i++)
            {
                //Log.Debug("Choosing player!");
                ReferenceHub chosenHub = Extensions.GetRandomElementFromList(spectators);
                spectators.Remove(chosenHub);
                if(chosenHub == null)
                {
                    //Log.Warning("ReferenceHub is null!");
                    continue;
                }
                Player chosenPlayer = Player.Get(chosenHub);
                chosenPlayer.SetRole(PlayerRoles.RoleTypeId.FacilityGuard);
                spawnedPlayers.Add(chosenPlayer);
                Timing.CallDelayed(0.2f, () => 
                {
                    //Log.Debug("Teleporting!");
                    DoorVariant chosenDoor = Extensions.GetRandomElementFromList(validDoors);
                    Vector3 targetVector = chosenDoor.transform.position;
                    targetVector.y += 1;
                    chosenPlayer.Position = targetVector;
                });
            }
            if (!UnitNamingRule.TryGetNamingRule(SpawnableTeamType.NineTailedFox, out var rule))
            {
                Log.Warning("Failed to get unit name.");
            }
            //Log.Debug("Saying cassie!");
            string cassieUnitName = rule.GetCassieUnitName(spawnedPlayers.First().UnitName);
            string cassieMessage = $"Security team {cassieUnitName} has entered Light Containment Zone.";
            Cassie.Message(cassieMessage, isSubtitles: true);
        }

        [PluginEvent(ServerEventType.RoundStart)]
        public void SpawnRagdollsEvent(RoundStartEvent ev)
        {
            Timing.CallDelayed(1f, () => 
            {
                SpawnRagdolls();
            });
        }


        public static void SpawnRagdolls()
        {

        }
    }
}

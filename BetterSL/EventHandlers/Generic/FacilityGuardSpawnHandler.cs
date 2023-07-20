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
using BetterSL.Managers;
using PlayerRoles;
using InventorySystem;
using InventorySystem.Items;
using InventorySystem.Items.Firearms;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using PlayerStatsSystem;
using Mirror;

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
            List<ReferenceHub> spectators = Resources.Extensions.GetByTeam(PlayerRoles.Team.Dead);
            List<ReferenceHub> chosenPlayers = new List<ReferenceHub>();
            List<Player> spawnedPlayers = new List<Player>();
            for(int i = 0; i < Plugin.GetConfig().MaxGuardSpawns; i++)
            {
                //Log.Debug("Choosing player!");
                ReferenceHub chosenHub = Resources.Extensions.GetRandomElementFromList(spectators);
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
                    DoorVariant chosenDoor = Resources.Extensions.GetRandomElementFromList(validDoors);
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
            if (!Plugin.GetConfig().ShouldGuardCorpsesSpawn)
            {
                return;
            }
            List<RoomName> allowedRooms = Plugin.GetConfig().GuardCorpseSpawnableRooms;
            List<string> npcNames = Plugin.NpcNameConfig.CurrentText;
            List<string> npcDeathReasons = Plugin.NpcDeathConfig.CurrentText;
            for(int i = 0; i < Plugin.GetConfig().GuardCorpseAmount; i++)
            {
                RoomName chosenRoom = Resources.Extensions.GetRandomElementFromList(allowedRooms);
                if(allowedRooms.Count == 0)
                {
                    continue;
                }
                allowedRooms.Remove(chosenRoom);
                HashSet<RoomIdentifier> rooms = RoomIdUtils.FindRooms(chosenRoom, FacilityZone.None, RoomShape.Undefined);
                if (rooms != null)
                {
                    RoomIdentifier room = Resources.Extensions.GetRandomElementFromList(rooms.ToList());
                    HashSet<DoorVariant> doors = DoorVariant.DoorsByRoom[room];
                    DoorVariant door = Resources.Extensions.GetRandomElementFromList(doors.ToList());
                    Vector3 doorPosition = door.transform.position;
                    doorPosition.y += 1f;
                    string name = Resources.Extensions.GetRandomElementFromList(npcNames);
                    if(npcNames.Count == 0) 
                    {
                        name = "Ran out of names!";
                    }
                    else
                    {
                        npcNames.Remove(name);
                    }
                    name += " (NPC)";
                    ReferenceHub dummyHub = DummyManager.SpawnDummy(doorPosition, false, name, RoleTypeId.FacilityGuard);
                    dummyHub.inventory.UserInventory.ReserveAmmo.Clear();
                    InventoryInfo userInventory = dummyHub.inventory.UserInventory;
                    while (userInventory.Items.Count > 0)
                    {
                        dummyHub.inventory.ServerRemoveItem(userInventory.Items.ElementAt(0).Key, null);
                    }
                    if (room.Zone == FacilityZone.HeavyContainment)
                    {
                        foreach(ItemType item in Plugin.GetConfig().HczGuardCorpseContents.Keys)
                        {
                            int Amount = Plugin.GetConfig().HczGuardCorpseContents[item];
                            if (item.ToString().Contains("Gun"))
                            {
                                ItemBase itemBase = dummyHub.inventory.ServerAddItem(item);
                                Firearm firearm = (Firearm)itemBase;
                                FirearmStatus status = new FirearmStatus((byte)Amount, firearm.Status.Flags, firearm.Status.Attachments);
                                firearm.Status = status;
                                continue;
                            }
                            if (item.ToString().Contains("Ammo"))
                            {
                                dummyHub.inventory.ServerAddAmmo(item, Amount);
                                continue;
                            }
                            for(int y = 0; y < Amount; i++)
                            {
                                dummyHub.inventory.ServerAddItem(item);
                            }
                        }
                    }
                    else
                    {
                        foreach (ItemType item in Plugin.GetConfig().EzGuardCorpseContents.Keys)
                        {
                            int Amount = Plugin.GetConfig().EzGuardCorpseContents[item];
                            if (item.ToString().Contains("Gun"))
                            {
                                ItemBase itemBase = dummyHub.inventory.ServerAddItem(item);
                                Firearm firearm = (Firearm)itemBase;
                                FirearmStatus status = new FirearmStatus((byte)Amount, firearm.Status.Flags, firearm.Status.Attachments);
                                firearm.Status = status;
                                continue;
                            }
                            if (item.ToString().Contains("Ammo"))
                            {
                                dummyHub.inventory.ServerAddAmmo(item, Amount);
                                continue;
                            }
                            for (int y = 0; y < Amount; i++)
                            {
                                dummyHub.inventory.ServerAddItem(item);
                            }
                        }
                    }
                    string deathReason = Resources.Extensions.GetRandomElementFromList(npcDeathReasons);
                    if(npcDeathReasons.Count == 0)
                    {
                        deathReason = "Ran out of death reasons.";
                    }
                    else
                    {
                        npcDeathReasons.Remove(deathReason);
                    }
                    Player.Get(dummyHub).Kill(deathReason);
                    NetworkServer.Destroy(dummyHub.gameObject);
                }
                else
                {
                    Log.Warning("Can't find room! Room: " + chosenRoom.ToString());
                }
            }
        }
    }
}

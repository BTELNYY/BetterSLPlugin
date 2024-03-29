﻿using System;
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
using PlayerRoles.FirstPersonControl;

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
            if (!Round.IsRoundStarted)
            {
                Log.Warning("Tried to spawn guards when round did not start yet.");
                return;
            }
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
                if(chosenHub.nicknameSync.DisplayName == "Dedicated Server")
                {
                    continue;
                }
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
            Timing.CallDelayed(0.5f, () => 
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
            RoundSummary.RoundLock = true;
            List<RoomName> allowedRooms = Plugin.GetConfig().GuardCorpseSpawnableRooms;
            List<string> npcNames = Plugin.NpcNameConfig.CurrentText;
            List<string> npcDeathReasons = Plugin.NpcDeathConfig.CurrentText;
            for(int i = 0; i < Plugin.GetConfig().GuardCorpseAmount; i++)
            {
                try
                {
                    RoomName chosenRoom = Resources.Extensions.GetRandomElementFromList(allowedRooms);
                    if (allowedRooms.Count == 0)
                    {
                        continue;
                    }
                    allowedRooms.Remove(chosenRoom);
                    HashSet<RoomIdentifier> rooms = RoomIdUtils.FindRooms(chosenRoom, FacilityZone.None, RoomShape.Undefined);
                    if (rooms != null)
                    {
                        RoomIdentifier room = Resources.Extensions.GetRandomElementFromList(rooms.ToList());
                        //Log.Debug("Chosen room: " + room.Name.ToString());
                        HashSet<DoorVariant> doors = DoorVariant.DoorsByRoom[room];
                        DoorVariant door = Resources.Extensions.GetRandomElementFromList(doors.ToList());
                        //Log.Debug("Chosen door: " + door.name);
                        Vector3 doorPosition = door.transform.position;
                        doorPosition.y += 1f;
                        string name = Resources.Extensions.GetRandomElementFromList(npcNames);
                        if (npcNames.Count == 0)
                        {
                            Log.Warning("Ran out of names for NPCs!");
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
                        //Log.Debug("Dummy Spawned");
                        if (room.Zone == FacilityZone.HeavyContainment)
                        {
                            //Log.Debug("Adding Items! (HCZ)");
                            foreach (ItemType item in Plugin.GetConfig().HczGuardCorpseContents.Keys)
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
                                for (int y = 0; y < Amount; y++)
                                {
                                    dummyHub.inventory.ServerAddItem(item);
                                }
                            }
                        }
                        else
                        {
                            //Log.Debug("Adding items! (EZ)");
                            foreach (ItemType item in Plugin.GetConfig().EzGuardCorpseContents.Keys)
                            {
                                int Amount = Plugin.GetConfig().EzGuardCorpseContents[item];
                                if (item.ToString().Contains("Gun"))
                                {
                                    ItemBase itemBase = dummyHub.inventory.ServerAddItem(item);
                                    Firearm firearm = (Firearm)itemBase;
                                    FirearmStatus status = new FirearmStatus((byte)Amount, firearm.Status.Flags, firearm.Status.Attachments);
                                    firearm.Status = status;
                                    //Log.Debug("Added firearm");
                                    continue;
                                }
                                if (item.ToString().Contains("Ammo"))
                                {
                                    //Log.Debug("Added Ammo");
                                    dummyHub.inventory.ServerAddAmmo(item, Amount);
                                    continue;
                                }
                                for (int y = 0; y < Amount; y++)
                                {
                                    //Log.Debug("Added item");
                                    dummyHub.inventory.ServerAddItem(item);
                                }
                            }
                        }
                        string deathReason = Resources.Extensions.GetRandomElementFromList(npcDeathReasons);
                        if (npcDeathReasons.Count == 0)
                        {
                            Log.Warning("Ran out of NPC death reasons!");
                            deathReason = "Ran out of death reasons.";
                        }
                        else
                        {
                            npcDeathReasons.Remove(deathReason);
                        }
                        dummyHub.TryOverridePosition(doorPosition, Vector3.up);
                        //Log.Debug("Killing NPC!");
                        Player npc = Player.Get(dummyHub);
                        try
                        {
                            npc.Kill(deathReason);
                        }
                        catch (Exception ex)
                        {
                            Log.Error(ex.ToString());
                        }
                        //Log.Debug("Despawning NPC!");
                        //npc.Kick(null, "Despawn");
                        NetworkServer.Destroy(dummyHub.gameObject);
                        NetworkServer.Destroy(npc.GameObject);
                    }
                    else
                    {
                        Log.Warning("Can't find room! Room: " + chosenRoom.ToString());
                    }
                }
                catch(Exception ex)
                {
                    Log.Error(ex.ToString());
                    RoundSummary.RoundLock = false;
                }
            }
            RoundSummary.RoundLock = false;
        }
    }
}

using PlayerRoles;
using PluginAPI.Core.Attributes;
using PluginAPI.Core;
using PluginAPI.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InventorySystem;
using PluginAPI.Events;
using BetterSL.Resources;
using MEC;
using BetterSL.Managers;
using static UnityEngine.GraphicsBuffer;

namespace BetterSL.EventHandlers.Generic
{
    public class MTFHandler
    {
        [PluginEvent(ServerEventType.PlayerSpawn)]
        public void OnPlayerSpawned(PlayerSpawnEvent ev)
        {
            Timing.CallDelayed(0.5f, () => 
            {
                Player player = ev.Player;
                RoleTypeId role = ev.Role;
                if (role == RoleTypeId.NtfPrivate)
                {
                    if(Extensions.RemoveItemFromPlayer(player, ItemType.KeycardNTFOfficer))
                    {
                        player.AddItem(ItemType.KeycardNTFLieutenant);
                    }
                }
            });
        }

        [PluginEvent(ServerEventType.PlayerDeath)]
        public void OnPlayerDeath(PlayerDeathEvent ev)
        {
            ev.Player.PlayerInfo.IsRoleHidden = false;
            ev.Player.CustomInfo = null;
        }


        [PluginEvent(ServerEventType.TeamRespawn)]
        public void OnMTFSpawnAssignSubclasses(TeamRespawnEvent ev)
        {
            Timing.CallDelayed(0.5f, () => 
            {
                if (!Plugin.GetConfig().MTFAssignSubclasses)
                {
                    return;
                }
                if(ev.Team != Respawning.SpawnableTeamType.NineTailedFox)
                {
                    return;
                }
                BaseSubclass mtfVanguard = SubclassManager.GetSubclass("mtf_vanguard");
                BaseSubclass mtfShockTrooper = SubclassManager.GetSubclass("mtf_shock_trooper");
                BaseSubclass mtfMarksman = SubclassManager.GetSubclass("mtf_marksman");
                List<Player> respawnedPlayers = ev.Players;
                while (respawnedPlayers.Count > 0)
                {
                    Player player = BetterSL.Resources.Extensions.GetRandomElementFromList(respawnedPlayers);
                    respawnedPlayers.Remove(player);
                    SubclassManager.SetPlayerToSubclass(player, mtfMarksman);
                    if (respawnedPlayers.Count == 0)
                    {
                        break;
                    }
                    Player player1 = BetterSL.Resources.Extensions.GetRandomElementFromList(respawnedPlayers);
                    respawnedPlayers.Remove(player1);
                    SubclassManager.SetPlayerToSubclass(player1, mtfVanguard);
                    if (respawnedPlayers.Count == 0)
                    {
                        break;
                    }
                    Player player2 = BetterSL.Resources.Extensions.GetRandomElementFromList(respawnedPlayers);
                    Player player3 = BetterSL.Resources.Extensions.GetRandomElementFromList(respawnedPlayers);
                    respawnedPlayers.Remove(player2);
                    respawnedPlayers.Remove(player3);
                    SubclassManager.SetPlayerToSubclass(player2, mtfShockTrooper);
                    SubclassManager.SetPlayerToSubclass(player3, mtfShockTrooper);
                }
            });
        }
    }
}

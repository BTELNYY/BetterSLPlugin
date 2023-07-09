using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using PlayerRoles;
using PlayerRoles.PlayableScps.Scp079;
using PluginAPI;
using PluginAPI.Core;
using PluginAPI.Events;

namespace BetterSL.EventHandlers.Scp079
{
    public class Scp079Handler
    {
        [PluginAPI.Core.Attributes.PluginEvent(PluginAPI.Enums.ServerEventType.PlayerSpawn)]
        public void OnSpawn(PlayerSpawnEvent ev)
        {
            RoleTypeId role = ev.Role;
            Player player = ev.Player;
            var rolebase = player.RoleBase as Scp079Role;
            if (role != RoleTypeId.Scp079) { return; }
            if(player.RoleBase is null)
            {
                Log.Warning("RoleBase is null!");
                return;
            }
            if (player.RoleBase is Scp079Role)
            {
                
                if (!rolebase.SubroutineModule.TryGetSubroutine<Scp079AuxManager>(out var manager))
                {
                    Log.Error("Failed to get Scp079 Aux Manager!");
                    return;
                }
                manager.CurrentAux = Plugin.instance.config.Scp079SpawnAp;
            }
            else
            {
                Log.Debug(player.RoleBase.name);
            }
            rolebase.SubroutineModule.TryGetSubroutine(out Scp079LockdownRoomAbility ability);
            if(ability is null)
            {
                Log.Error("Failed to get LockdownRoomAbility!");
                return;
            }
            if (!ability.ScpRole.SubroutineModule.TryGetSubroutine<Scp079AuxManager>(out var subroutine))
            {
                Log.Error("Failed getting AuxManager!");
                return;
            }
            else
            {
                var field = AccessTools.Field(typeof(Scp079LockdownRoomAbility), "_cost");
                int cost = (int)(subroutine.MaxAux * Plugin.GetConfig().Scp079LockdownCostPercent);
                field.SetValue(ability, cost);
            }
        }
    }
}

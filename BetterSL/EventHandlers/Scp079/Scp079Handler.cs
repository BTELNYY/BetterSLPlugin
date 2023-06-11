using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlayerRoles;
using PlayerRoles.PlayableScps.Scp079;
using PluginAPI;
using PluginAPI.Core;

namespace BetterSL.EventHandlers.Scp079
{
    public class Scp079Handler
    {
        [PluginAPI.Core.Attributes.PluginEvent(PluginAPI.Enums.ServerEventType.PlayerSpawn)]
        public void OnSpawn(Player player, RoleTypeId role)
        {
            if(role != RoleTypeId.Scp079) { return; }
            if(player.RoleBase is null)
            {
                Log.Warning("RoleBase is null!");
                return;
            }
            if (player.RoleBase is Scp079Role)
            {
                var rolebase = player.RoleBase as Scp079Role;
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
            Scp079DoorHandler.DoorsLocked = 0;
        }

        [PluginAPI.Core.Attributes.PluginEvent(PluginAPI.Enums.ServerEventType.PlayerGameConsoleCommand)]
        public void ConsoleCommand(Player player, string command, string[] arguments)
        {
            if(command != "fixdoors")
            {
                return;
            }
            Scp079DoorHandler.DoorsLocked = 0;
        }
    }
}

using PlayerRoles.PlayableScps.Scp079;
using PlayerRoles;
using PluginAPI.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interactables.Interobjects;
using BetterSL.Managers;

namespace BetterSL.EventHandlers.Generic
{
    public class ConsoleCommandHandler
    {
        [PluginAPI.Core.Attributes.PluginEvent(PluginAPI.Enums.ServerEventType.PlayerGameConsoleCommand)]
        public void ConsoleCommand(Player player, string command, string[] arguments)
        {
            if (command != "spawndummy")
            {
                return;
            }
            if(Plugin.GetConfig().DummiesEnabled)
            {
                player.SendConsoleMessage("Spawning dummy...");
                DummyManager.SpawnDummy(player.Camera.position, true);
            }
            else
            {
                player.SendConsoleMessage("Nice try.", "red");
            }
        }
    }
}

using CommandSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PluginAPI.Core;
using BetterSL.Managers;

namespace BetterSL.Commands
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class SpawnDummyCommand : ICommand
    {
        public string Command => "spawndummy";

        public string[] Aliases => new string[0];

        public string Description => "Spawns a fake player";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player player = (Player)sender;
            if (Plugin.GetConfig().DummiesEnabled)
            {
                DummyManager.SpawnDummy(player.Camera.position, true);
                response = "Spawning dummy!";
                return true;
            }
            else
            {
                response = "Nice try...";
                return false;
            }
        }
    }
}

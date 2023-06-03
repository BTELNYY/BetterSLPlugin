using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PluginAPI;
using PluginAPI.Core.Attributes;
using PluginAPI.Core;
using PluginAPI.Helpers;

namespace BetterSL.EventHandlers.Generic
{
    public class PlayerJoinHandler
    {
        static Config config = Plugin.instance.config;

        [PluginAPI.Core.Attributes.PluginEvent(PluginAPI.Enums.ServerEventType.PlayerJoined)]
        public void OnPlayerJoined(Player player)
        {
            player.SendBroadcast(config.PlayerJoinBroadcastText, config.PlayerJoinBroadcastDuration, Broadcast.BroadcastFlags.Normal, true);
        }
    }
}

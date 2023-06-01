using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlayerRoles;
using PluginAPI.Core;
using PluginAPI.Core.Attributes;
using PluginAPI.Enums;

namespace BetterSL.EventHandlers
{
    public class Scp0492SpawnHandler
    {
        public static Dictionary<int, int> Player049Turns = new Dictionary<int, int>();

        [PluginEvent(ServerEventType.PlayerChangeRole)]
        public static void OnPlayerBecomeZombie(Player player, PlayerRoleBase oldRole, RoleTypeId newRole, RoleChangeReason changeReason)
        {
            if (!Player049Turns.ContainsKey(player.PlayerId) && newRole == RoleTypeId.Scp0492)
            {
                Player049Turns.Add(player.PlayerId, 0);
            }
            if (newRole == RoleTypeId.Scp0492 && changeReason == RoleChangeReason.Revived)
            {
                if (oldRole.RoleTypeId == RoleTypeId.Scp0492)
                {
                    Player049Turns[player.PlayerId]++;
                }
            }
            else
            {
                Player049Turns[player.PlayerId] = 0;
            }
        }
    }
}

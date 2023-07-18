using BetterSL.Resources;
using HarmonyLib;
using MapGeneration;
using PlayerRoles;
using PlayerRoles.PlayableScps.Scp079.Pinging;
using PluginAPI.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BetterSL.Patches.Scp079
{
    [HarmonyPatch(typeof(Scp079PingAbility), "ServerCheckReceiver")]
    public class Scp079PingGeneratorPatch
    {
        public static void Postfix(Scp079PingAbility __instance, ReferenceHub hub, Vector3 point, int processorIndex)
        {
            if(processorIndex == 0)
            {
                List<ReferenceHub> scpTeamMembers = Extensions.GetScpTeam();
                foreach(ReferenceHub scp in scpTeamMembers) 
                {
                    string roomname = RoomIdUtils.RoomAtPosition(point).name;
                    Player.Get(scp).SendBroadcast(Plugin.GetConfig().Scp079GeneratorAlertMessage.Replace("{room}", roomname), 10);
                }
            }
        }
    }
}

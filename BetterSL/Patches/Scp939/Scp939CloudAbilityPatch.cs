using HarmonyLib;
using Hints;
using Interactables.Interobjects;
using MapGeneration.Distributors;
using Mirror;
using PlayerRoles.PlayableScps.Scp939;
using PluginAPI.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BetterSL.Patches.Scp939
{
    [HarmonyPatch(typeof(Scp939AmnesticCloudAbility), "OnStateEnabled")]
    public class Scp939CloudAbilityPatch
    {
        public static bool Prefix(Scp939AmnesticCloudAbility __instance)
        {
            if (Plugin.GetConfig().Scp939CanUseCloudInElevator)
            {
                return true;
            }
            if (BetterSL.Resources.Extensions.InElevator(__instance.ScpRole.CameraPosition))
            {
                Player.Get(__instance.Owner).SendBroadcast(Plugin.GetConfig().Scp939GasInElevatorMessage, 2);
                TextHint hint = new TextHint(Plugin.GetConfig().Scp939GasInElevatorMessage);
                __instance.Owner.hints.Show(hint);
                __instance.ServerFailPlacement();
                return false;

            }
            return true;
        }           
    }
}

using HarmonyLib;
using MapGeneration.Distributors;
using PlayerRoles.PlayableScps.Scp939;
using PluginAPI.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BetterSL.Patches
{
    [HarmonyPatch(typeof(Scp939AmnesticCloudAbility), "OnStateEnabled")]
    public class Scp939CloudAbilityPatch
    {
        public static bool Prefix(Scp939AmnesticCloudAbility __instance)
        {
            if (Plugin.GetConfig().Scp939CanUseCloudInElevator)
            {
                return false;
            }
            Vector3 pos = __instance.transform.position;
            pos.y += 0.52f;
            var collider = __instance.GetComponent("Collider");;
            if (Physics.Raycast(pos, Vector3.up, out RaycastHit hit, 10f))
            {
                Log.Debug("Raycast hit!");
                if(hit.transform.gameObject.name == "Chamber")
                {
                    Log.Debug("Elevator found, skipping.");
                    return false;
                }
                else
                {
                    Log.Debug("Not an elevator. Object: " + hit.transform.gameObject.name);
                    Log.Debug(hit.transform.position.ToString());
                    Log.Debug(__instance.transform.position.ToString());
                    Log.Debug(pos.ToString());
                    return true;
                }
            }
            else
            {
                Log.Debug("didn't hit anything");
                return false;
            }
        }
    }
}

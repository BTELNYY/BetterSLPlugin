using HarmonyLib;
using PlayerRoles.PlayableScps.Scp939;
using PluginAPI.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using PluginAPI.Core;

namespace BetterSL.Patches
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
            Vector3 pos = __instance.transform.position + new Vector3(0, 3.2f, 0);
            if (Physics.Raycast(pos, __instance.transform.TransformDirection(Vector3.up), out RaycastHit hit, Mathf.Infinity))
            {
                if(hit.transform.gameObject.name == "Chamber")
                {
                    Log.Debug("Skipping check, in elevator!");
                    return false;
                }
                else
                {
                    Log.Debug("Not in elevator, continuing!");
                    Log.Debug(hit.transform.gameObject.name);
                    return true;
                }
            }
            else
            {
                Log.Debug("Didn't hit anything, allowing continue!");
                return true;
            }
        }
    }
}

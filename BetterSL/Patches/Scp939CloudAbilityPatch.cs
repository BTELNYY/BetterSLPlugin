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
            //false = not in elevator or check failed.
            //true in elevator
            //this is not a very clean way to do this. too bad
            bool[] counter = { false, false, false, false };
            counter[0] = Physics.Raycast(pos, __instance.transform.TransformDirection(Vector3.forward), out RaycastHit hit, Mathf.Infinity) && hit.transform.name == "Chamber";
            Log.Debug(hit.transform.gameObject.name);
            counter[1] = Physics.Raycast(pos, __instance.transform.TransformDirection(Vector3.right), out RaycastHit hit1, Mathf.Infinity) && hit1.transform.name == "Chamber";
            Log.Debug(hit1.transform.gameObject.name);
            counter[2] = Physics.Raycast(pos, __instance.transform.TransformDirection(Vector3.left), out RaycastHit hit2, Mathf.Infinity) && hit2.transform.name == "Chamber";
            Log.Debug(hit2.transform.gameObject.name);
            counter[3] = Physics.Raycast(pos, __instance.transform.TransformDirection(Vector3.back), out RaycastHit hit3, Mathf.Infinity) && hit3.transform.name == "Chamber";
            Log.Debug(hit3.transform.gameObject.name);
            List<bool> b = counter.Where(x => x = true).ToList();
            foreach(bool flag in b)
            {
                Log.Debug(flag.ToString());
            }
            return false;
        }
    }
}

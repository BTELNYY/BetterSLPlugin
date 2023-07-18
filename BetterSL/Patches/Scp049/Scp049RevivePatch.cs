using HarmonyLib;
using PlayerRoles.PlayableScps.Scp049;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BetterSL.EventHandlers;
using PluginAPI.Core;

namespace BetterSL.Patches.Scp049
{
    [HarmonyPatch(typeof(Scp049ResurrectAbility), "ServerValidateBegin")]
    public class Scp049RevivePatch
    {
        public static void Postfix(Scp049ResurrectAbility __instance, ref byte __result, BasicRagdoll ragdoll)
        {
            try
            {
                int resurrectionsNumber = Scp049ResurrectAbility.GetResurrectionsNumber(ragdoll.Info.OwnerHub);
                if(__result == 2)
                {
                    return;
                }
                if (resurrectionsNumber >= Plugin.GetConfig().Scp049Max0492Ressurection)
                {
                    __result = 3;
                    return;
                }
                __result = 0;
                return;
            }catch(Exception e)
            {
                Log.Error(e.ToString());
            }
            __result = 0;
        }
    }
}
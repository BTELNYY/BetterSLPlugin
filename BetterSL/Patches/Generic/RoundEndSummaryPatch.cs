using BetterSL.EventHandlers.Generic;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetterSL.Patches.Generic
{
    //[HarmonyPatch(typeof(RoundSummary), "_ProcessServerSideCode")]
    public class RoundEndSummaryPatch
    {
        public static bool Prefix(RoundSummary __instance) 
        {
            if(ChaosTargetHandler.ChaosAlive > ChaosTargetHandler.ChaosTotal * Plugin.GetConfig().ChaosDeadPercent)
            {
                return true;
            }
            return false;
        }
    }
}

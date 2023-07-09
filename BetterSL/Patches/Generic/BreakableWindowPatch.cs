using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetterSL.Patches.Generic
{
    [HarmonyPatch(typeof(BreakableWindow), "Awake")]
    public class BreakableWindowPatch
    {
        public static List<BreakableWindow> BreakableWindows = new List<BreakableWindow>();

        public static void Postfix(BreakableWindow __instance)
        {
            BreakableWindows.Add(__instance);
        }
    }
}

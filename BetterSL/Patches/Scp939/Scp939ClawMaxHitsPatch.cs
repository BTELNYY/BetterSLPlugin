using HarmonyLib;
using PlayerRoles.PlayableScps;
using PlayerRoles.PlayableScps.Scp939;
using PlayerStatsSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using PluginAPI.Core;
using PlayerRoles.PlayableScps.Subroutines;
using System.Runtime.CompilerServices;

namespace BetterSL.Patches.Scp939
{
    [HarmonyPatch(typeof(ScpAttackAbilityBase<Scp939Role>), "ServerPerformAttack")]
    public class Scp939ClawMaxHitsPatch
    {
        public static bool Prefix(ScpAttackAbilityBase<Scp939Role> __instance)
        {
            HashSet<ReferenceHub> targettedPlayers = (HashSet<ReferenceHub>)AccessTools.Field(typeof(ScpAttackAbilityBase<Scp939Role>), "TargettedPlayers").GetValue(__instance);
            while(targettedPlayers.Count > Plugin.GetConfig().Scp939MaxAoePlayerHits)
            {
                //System.Random rand = new System.Random();
                //int randomNumber = rand.Next(0, targettedPlayers.Count);
                //ReferenceHub hubtoremove = targettedPlayers.ToList()[randomNumber];
                //targettedPlayers.Remove(hubtoremove);
                //uncomment this and remove above to use Last() method! - btelnyy
                //Done, onion man suggested.
                targettedPlayers.Remove(targettedPlayers.Last());
            }
            return true;
        }
    }
}

﻿using HarmonyLib;
using PlayerRoles.PlayableScps.Scp049.Zombies;

namespace BetterSL.Patches
{ 
    [HarmonyPatch(typeof(ZombieMovementModule), nameof(ZombieMovementModule.BloodlustSpeed), MethodType.Getter)]
    public class Scp0492BloodlustPatch
    {
        public static void Postfix(ZombieMovementModule __instance, ref float __result)
        {
            __result = Plugin.GetConfig().LobotomizedBloodlustMaxSpeed;
        }
    }
}
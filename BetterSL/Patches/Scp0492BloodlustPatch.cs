using HarmonyLib;
using PlayerRoles.PlayableScps.Scp049.Zombies;

namespace BetterSL.Patches
{ 
    [HarmonyPatch(typeof(ZombieMovementModule), "UpdateSpeed")]
    public class Scp0492BloodlustPatch
    {
        static void Prefix(ZombieMovementModule __instance)
        {
            var field = AccessTools.Field(typeof(ZombieMovementModule), "BloodlustSpeed");
            field.SetValue(__instance, Plugin.instance.config.LobotomizedBloodlustMaxSpeed); // wow guys
        }
    }
}
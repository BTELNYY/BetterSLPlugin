using HarmonyLib;
using PlayerRoles.PlayableScps.Scp049.Zombies;

namespace BetterSL.Patches
{ 
    [HarmonyPatch(typeof(ZombieMovementModule), "UpdateSpeed")]
    public class SpookyZombieStareSpeedPatch
    {
        static void Prefix(ref float ___BloodlustSpeed)
        {
            ___BloodlustSpeed = Plugin.instance.config.LobotomizedBloodlustMaxSpeed; // wow guys
        }
    }
}
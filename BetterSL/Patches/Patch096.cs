
using HarmonyLib;
using PlayerRoles.PlayableScps.Scp096;

namespace BetterSL.Patches
{
    [HarmonyPatch(typeof(Scp096AttackAbility), nameof(Scp096AttackAbility.SpawnObject))]
    public class Patch096 {
        static void Postfix(ref Scp096HitHandler ____leftHitHandler, ref Scp096HitHandler ____rightHitHandler)
        {
            // get the cool field info
            var damage_field_info = AccessTools.Field(typeof(Scp096HitHandler), "_humanTargetDamage");
            damage_field_info.SetValue(____rightHitHandler, 65f);
            damage_field_info.SetValue(____leftHitHandler, 65f);
        }
    }
}
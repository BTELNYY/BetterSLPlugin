
using HarmonyLib;
using PlayerRoles.PlayableScps.Scp096;

namespace BetterSL.Patches
{
    [HarmonyPatch(typeof(Scp096AttackAbility), nameof(Scp096AttackAbility.SpawnObject))]
    public class Scp096MouseOneAttack {
        static void Postfix(ref Scp096HitHandler ____leftHitHandler, ref Scp096HitHandler ____rightHitHandler)
        {
            // get the cool field info
            var damage_field_info = AccessTools.Field(typeof(Scp096HitHandler), "_humanTargetDamage");
            damage_field_info.SetValue(____rightHitHandler, Plugin.instance.config.Scp096SwingDamage);
            damage_field_info.SetValue(____leftHitHandler, Plugin.instance.config.Scp096SwingDamage);
        }
    }
}
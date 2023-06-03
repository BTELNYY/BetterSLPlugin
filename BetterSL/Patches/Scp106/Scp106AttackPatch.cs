using CustomPlayerEffects;
using HarmonyLib;
using InventorySystem.Items.MicroHID;
using PlayerRoles;
using PlayerRoles.FirstPersonControl;
using PlayerRoles.PlayableScps.Scp106;
using PlayerStatsSystem;
using PluginAPI.Enums;
using MEC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using PluginAPI.Core;
using BetterSL.Resources;

namespace BetterSL.Patches.Scp106
{
    [HarmonyPatch(typeof(Scp106Attack), "ServerShoot")]
    public class Scp106AttackPatch
    {
        public static bool Prefix(Scp106Attack __instance)
        {
            ReferenceHub target = (ReferenceHub)AccessTools.Field(typeof(Scp106Attack), "_targetHub").GetValue(__instance);
            var targetRole = target.roleManager.CurrentRole as HumanRole;
            Vector3 targetPosition = targetRole.FpcModule.Position;
            Vector3 ownerPosition = __instance.ScpRole.FpcModule.Position;
            var sendcooldownmethod = AccessTools.Method(typeof(Scp106Attack), "SendCooldown");
            using (new FpcBacktracker(target, targetPosition, 0.35f))
            {
                Vector3 vector = targetPosition - ownerPosition;
                float sqrMagnitude = vector.sqrMagnitude;
                float maxRangeSqr = (float)AccessTools.Field(typeof(Scp106Attack), "_maxRangeSqr").GetValue(__instance);
                if (sqrMagnitude > maxRangeSqr)
                {
                    return false;
                }
                //Log.Debug("passed range check!");
                Transform ownerCam = __instance.Owner.PlayerCameraReference;
                Vector3 forward = ownerCam.forward;
                //Log.Debug("got camera!");
                forward.y = 0f;
                vector.y = 0f;
                if (Physics.Linecast(ownerPosition, targetPosition, MicroHIDItem.WallMask))
                {
                    return false;
                }
                //Log.Debug("Passed LineCast check!");
                AnimationCurve dotOverDistance = AccessTools.Field(typeof(Scp106Attack), "_dotOverDistance").GetValue(__instance) as AnimationCurve;
                if (dotOverDistance.Evaluate(sqrMagnitude) > Vector3.Dot(vector.normalized, forward.normalized))
                {
                    object[] missCooldown = { AccessTools.Field(typeof(Scp106Attack), "_missCooldown").GetValue(__instance) };
                    sendcooldownmethod.Invoke(__instance, missCooldown);
                    return false;
                }
                //if (Extensions.InDoor(__instance.Owner.PlayerCameraReference.position))
                //{
                //    object[] missCooldown = { AccessTools.Field(typeof(Scp106Attack), "_missCooldown").GetValue(__instance) };
                //    sendcooldownmethod.Invoke(__instance, missCooldown);
                //    return false;
                //}
                //Log.Debug("Passed dot over check!");
            }
            DamageHandlerBase handler = new ScpDamageHandler(__instance.Owner, Plugin.GetConfig().Scp106AttackDamage, DeathTranslations.PocketDecay);
            float hitCooldown = (float)AccessTools.Field(typeof(Scp106Attack), "_hitCooldown").GetValue(__instance);
            object[] cooldown = { hitCooldown };
            sendcooldownmethod.Invoke(__instance, cooldown);
            __instance.ScpRole.SubroutineModule.TryGetSubroutine<Scp106Vigor>(out var vigor);
            //vigor.VigorAmount += Plugin.GetConfig().Scp106OnKillVigor;
            AccessTools.Method(typeof(Scp106Attack), "ReduceSinkholeCooldown").Invoke(__instance, null);
            Hitmarker.SendHitmarker(__instance.Owner, 1f);
            if(target is null)
            {
                Log.Warning("Target is null!");
            }
            PlayerEffectsController playerEffectsController = target.playerEffectsController;
            if ((target.playerStats.StatModules[0].CurValue - Plugin.GetConfig().Scp106AttackDamage) <= 0)
            {
                if (target.characterClassManager.GodMode)
                {
                    //Log.Debug("Target in god mode!");
                    return false;
                }
                //Log.Debug("Target will die, sent to pocket.");
                playerEffectsController.EnableEffect<Corroding>(0f, false);
                target.playerStats.DealDamage(handler);
                vigor.VigorAmount += Plugin.GetConfig().Scp106OnKillVigor;
            }
            else
            {
                //Log.Debug("Applying damage over time!");
                vigor.VigorAmount += Plugin.GetConfig().Scp106OnAttackVigor;
                playerEffectsController.EnableEffect<Traumatized>(0f, false);
                float damage = Plugin.GetConfig().Scp106AttackDamageOverTimeDamage;
                target.playerStats.DealDamage(handler);
                DamageHandlerBase damageovertime = new ScpDamageHandler(__instance.Owner, damage, DeathTranslations.PocketDecay);
                Timing.CallPeriodically(Plugin.GetConfig().Scp106AttackDamageOverTimeDuration, Plugin.GetConfig().Scp106TickEvery, () => 
                {
                    if ((target.playerStats.StatModules[0].CurValue - damage) <= 0)
                    {
                        if (target.characterClassManager.GodMode)
                        {
                            return;
                        }
                        playerEffectsController.EnableEffect<Corroding>(0f, false);
                        vigor.VigorAmount += Plugin.GetConfig().Scp106OnKillVigor;
                    }
                    else
                    {
                        playerEffectsController.EnableEffect<Blinded>(2f, false);
                        playerEffectsController.EnableEffect<Traumatized>(0f, false);
                        playerEffectsController.EnableEffect<Concussed>(7f, false);
                        target.playerStats.DealDamage(damageovertime);
                    }
                });
            }
            return false;
        }
    }
}

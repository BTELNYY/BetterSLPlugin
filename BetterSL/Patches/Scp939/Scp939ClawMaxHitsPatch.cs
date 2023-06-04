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

namespace BetterSL.Patches.Scp939
{
    [HarmonyPatch(typeof(ScpAttackAbilityBase<Scp939Role>), "ServerPerformAttack")]
    public class Scp939ClawMaxHitsPatch
    {
        public static bool Prefix(ScpAttackAbilityBase<Scp939Role> __instance)
        {
            //Log.Debug("Getting vars!");
            float detectionRadius = (float)AccessTools.Field(typeof(ScpAttackAbilityBase<Scp939Role>), "_detectionRadius").GetValue(__instance);
            //Log.Debug("Getting detections non alloc!");
            Collider[] detectionsNonAlloc = (Collider[])AccessTools.Field(typeof(ScpAttackAbilityBase<Scp939Role>), "DetectionsNonAlloc").GetValue(__instance);
            //Log.Debug("Getting detectionMask!");
            CachedLayerMask detectionMask = (CachedLayerMask)AccessTools.Field(typeof(ScpAttackAbilityBase<Scp939Role>), "DetectionMask").GetValue(__instance);
            //Log.Debug("Getting blockerMask!");
            CachedLayerMask blockerMask = (CachedLayerMask)AccessTools.Field(typeof(ScpAttackAbilityBase<Scp939Role>), "BlockerMask").GetValue(__instance);
            //Log.Debug("Getting syncAttack!");
            AttackResult syncAttack = (AttackResult)AccessTools.Field(typeof(ScpAttackAbilityBase<Scp939Role>), "_syncAttack").GetValue(__instance);
            //Log.Debug("Getting targettedPlayers!");
            HashSet<ReferenceHub> targettedPlayers = (HashSet<ReferenceHub>)AccessTools.Field(typeof(ScpAttackAbilityBase<Scp939Role>), "TargettedPlayers").GetValue(__instance);
            //Log.Debug("Getting damageHandler");
            Scp939DamageHandler damageHandler = new Scp939DamageHandler(__instance.ScpRole, Scp939DamageType.Claw);
            //Log.Debug("Getting plyCam!");
            Transform plyCam = __instance.Owner.PlayerCameraReference;
            //Log.Debug("Getting detection offset!");
            float detectionOffset = (float)AccessTools.Field(typeof(ScpAttackAbilityBase<Scp939Role>), "_detectionOffset").GetValue(__instance);
            Vector3 overlapSphereOrigin = plyCam.position + plyCam.forward * detectionOffset;
            //Log.Debug("Getting methods!");
            MethodInfo onDestructibleDamaged = AccessTools.Method(typeof(ScpAttackAbilityBase<Scp939Role>), "OnDestructibleDamaged");
            Type[] types = { typeof(bool) };
            MethodInfo serverSendRpc = AccessTools.Method(typeof(ScpAttackAbilityBase<Scp939Role>), "ServerSendRpc", types);
            //Log.Debug("Got vars!");
            int num = Physics.OverlapSphereNonAlloc(overlapSphereOrigin, detectionRadius, detectionsNonAlloc, detectionMask);
            syncAttack = AttackResult.None;
            //Log.Debug("Got intersects!");
            int counter = 0;
            for (int i = 0; i < num; i++)
            {
                //Log.Debug("iterating!");
                IDestructible destructible;
                if (detectionsNonAlloc[i].TryGetComponent<IDestructible>(out destructible) && !Physics.Linecast(plyCam.position, destructible.CenterOfMass, blockerMask))
                {
                    HitboxIdentity hitboxIdentity = destructible as HitboxIdentity;
                    if ((hitboxIdentity == null || targettedPlayers.Remove(hitboxIdentity.TargetHub)) && destructible.Damage(__instance.DamageAmount, damageHandler, destructible.CenterOfMass))
                    {
                        object[] args = { destructible };
                        onDestructibleDamaged.Invoke(__instance, args);
                        syncAttack |= AttackResult.AttackedObject;
                        HitboxIdentity hitboxIdentity2 = destructible as HitboxIdentity;
                        if (hitboxIdentity2 != null)
                        {
                            if(counter >= Plugin.GetConfig().Scp939MaxAoePlayerHits)
                            {
                                break;
                            }
                            counter++;
                            syncAttack |= AttackResult.AttackedHuman;
                            if (hitboxIdentity2.TargetHub.playerStats.GetModule<HealthStat>().CurValue <= 0f)
                            {
                                syncAttack |= AttackResult.KilledHuman;
                            }
                        }
                    }
                }
            }
            object[] args1 = { true };
            serverSendRpc.Invoke(__instance, args1);
            return false;
        }
    }
}

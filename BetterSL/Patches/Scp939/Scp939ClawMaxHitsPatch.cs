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
using PlayerRoles;
using Mono.Cecil;
using PlayerRoles.PlayableScps.Scp049.Zombies;

namespace BetterSL.Patches.Scp939
{
    [HarmonyPatch(typeof(ScpAttackAbilityBase<Scp939Role>), "ServerPerformAttack")]
    public class Scp939ClawMaxHitsPatch
    {
        public static void Prefix(ScpAttackAbilityBase<Scp939Role> __instance)
        {
            if(__instance.Owner.roleManager.CurrentRole.RoleTypeId != RoleTypeId.Scp939)
            {
                //Log.Debug("Getting vars!");
                float detectionRadius = (float)AccessTools.Field(typeof(ScpAttackAbilityBase<ZombieRole>), "_detectionRadius").GetValue(__instance);
                //Log.Debug("Getting detections non alloc!");
                Collider[] detectionsNonAlloc = (Collider[])AccessTools.Field(typeof(ScpAttackAbilityBase<ZombieRole>), "DetectionsNonAlloc").GetValue(__instance);
                //Log.Debug("Getting detectionMask!");
                CachedLayerMask detectionMask = (CachedLayerMask)AccessTools.Field(typeof(ScpAttackAbilityBase<ZombieRole>), "DetectionMask").GetValue(__instance);
                //Log.Debug("Getting blockerMask!");
                CachedLayerMask blockerMask = (CachedLayerMask)AccessTools.Field(typeof(ScpAttackAbilityBase<ZombieRole>), "BlockerMask").GetValue(__instance);
                //Log.Debug("Getting syncAttack!");
                AttackResult syncAttack = (AttackResult)AccessTools.Field(typeof(ScpAttackAbilityBase<ZombieRole>), "_syncAttack").GetValue(__instance);
                //Log.Debug("Getting targettedPlayers!");
                HashSet<ReferenceHub> targettedPlayers = (HashSet<ReferenceHub>)AccessTools.Field(typeof(ScpAttackAbilityBase<ZombieRole>), "TargettedPlayers").GetValue(__instance);
                //Log.Debug("Getting damageHandler");
                ScpDamageHandler damageHandler = new ScpDamageHandler(__instance.Owner, __instance.DamageAmount, DeathTranslations.Zombie);
                //Log.Debug("Getting plyCam!");
                Transform plyCam = __instance.Owner.PlayerCameraReference;
                //Log.Debug("Getting detection offset!");
                float detectionOffset = (float)AccessTools.Field(typeof(ScpAttackAbilityBase<ZombieRole>), "_detectionOffset").GetValue(__instance);
                Vector3 overlapSphereOrigin = plyCam.position + plyCam.forward * detectionOffset;
                //Log.Debug("Getting methods!");
                MethodInfo onDestructibleDamaged = AccessTools.Method(typeof(ScpAttackAbilityBase<ZombieRole>), "OnDestructibleDamaged");
                Type[] types = { typeof(bool) };
                MethodInfo serverSendRpc = AccessTools.Method(typeof(ScpSubroutineBase), "ServerSendRpc", types);
                //Log.Debug("Got vars!");
                int num = Physics.OverlapSphereNonAlloc(overlapSphereOrigin, detectionRadius, detectionsNonAlloc, detectionMask);
                syncAttack = AttackResult.None;
                //Log.Debug("Got intersects!");
                for (int i = 0; i < num; i++)
                {
                    ////Log.Debug("iterating!");
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
                                Hitmarker.SendHitmarker(__instance.Owner.connectionToClient, 1.2f);
                                ////Log.Debug("Attacked human!");
                                syncAttack |= AttackResult.AttackedHuman;
                                if (hitboxIdentity2.TargetHub.playerStats.GetModule<HealthStat>().CurValue <= 0f)
                                {
                                    ////Log.Debug("Killed human!");
                                    syncAttack |= AttackResult.KilledHuman;
                                }
                            }
                        }
                    }
                }
                object[] args1 = { true };
                serverSendRpc.Invoke(__instance, args1);
                return;
            }
            else
            {
                //Log.Debug("Running 939 attack code...");
                HashSet<ReferenceHub> targettedPlayers = (HashSet<ReferenceHub>)AccessTools.Field(typeof(ScpAttackAbilityBase<Scp939Role>), "TargettedPlayers").GetValue(__instance);
                while (targettedPlayers.Count > Plugin.GetConfig().Scp939MaxAoePlayerHits)
                {
                    //System.Random rand = new System.Random();
                    //int randomNumber = rand.Next(0, targettedPlayers.Count);
                    //ReferenceHub hubtoremove = targettedPlayers.ToList()[randomNumber];
                    //targettedPlayers.Remove(hubtoremove);
                    //uncomment this and remove above to use Last() method! - btelnyy
                    //Done, onion man suggested.
                    //Log.Debug("removing target!");
                    targettedPlayers.Remove(targettedPlayers.Last());
                }
                return;
            }
        }
    }

    //[HarmonyPatch(typeof(ScpAttackAbilityBase<ZombieRole>), "ServerPerformAttack")]
    public class Scp049TestPatch
    {
        public static void Prefix(ScpAttackAbilityBase<ZombieRole> __instance)
        {

        }
    }
}

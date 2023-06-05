using HarmonyLib;
using PlayerRoles.PlayableScps.Scp079;
using PlayerRoles;
using PlayerRoles.PlayableScps.Scp079.Rewards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetterSL.Patches.Scp079
{
    [HarmonyPatch(typeof(TerminationRewards), nameof(TerminationRewards.TryGetReward))]
    public class Scp079TerminationRewardPatch
    {
        public static void Postfix(RoleTypeId rt, out Scp079HudTranslation gainReason, out int amount)
        {
            switch (rt.GetTeam())
            {
                case Team.FoundationForces:
                    if (rt == RoleTypeId.FacilityGuard)
                    {
                        amount = (int)(25 * Plugin.GetConfig().Scp079TerminationRewardMultipliers[rt.GetTeam()]);
                        gainReason = Scp079HudTranslation.ExpGainTerminationGuard;
                    }
                    else
                    {
                        amount = (int)(30 * Plugin.GetConfig().Scp079TerminationRewardMultipliers[rt.GetTeam()]);
                        gainReason = Scp079HudTranslation.ExpGainTerminationNtf;
                    }
                    return;
                case Team.ChaosInsurgency:
                    gainReason = Scp079HudTranslation.ExpGainTerminationChaos;
                    amount = (int)(30 * Plugin.GetConfig().Scp079TerminationRewardMultipliers[rt.GetTeam()]);
                    return;
                case Team.Scientists:
                    gainReason = Scp079HudTranslation.ExpGainTerminationScientist;
                    amount = (int)(50 * Plugin.GetConfig().Scp079TerminationRewardMultipliers[rt.GetTeam()]);
                    return;
                case Team.ClassD:
                    gainReason = Scp079HudTranslation.ExpGainTerminationClassD;
                    amount = (int)(40 * Plugin.GetConfig().Scp079TerminationRewardMultipliers[rt.GetTeam()]);
                    return;
                case Team.OtherAlive:
                    gainReason = Scp079HudTranslation.ExpGainTerminationOther;
                    amount = (int)(30 * Plugin.GetConfig().Scp079TerminationRewardMultipliers[rt.GetTeam()]);
                    return;
            }   
            amount = 1;
            gainReason = Scp079HudTranslation.Zoom;
            return;
        }
    }
}

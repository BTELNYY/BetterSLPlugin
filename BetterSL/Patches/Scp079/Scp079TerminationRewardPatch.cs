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
    [HarmonyPatch(typeof(TerminationRewards), nameof(TerminationRewards.TryGetBaseReward))]
    public class Scp079TerminationRewardPatch
    {
        public static void Postfix(RoleTypeId rt, out int amount)
        {
            switch (rt.GetTeam())
            {
                case Team.FoundationForces:
                    if (rt == RoleTypeId.FacilityGuard)
                    {
                        amount = (int)(30 * Plugin.GetConfig().Scp079TerminationRewardMultipliers[rt.GetTeam()]);
                    }
                    else
                    {
                        amount = (int)(50 * Plugin.GetConfig().Scp079TerminationRewardMultipliers[rt.GetTeam()]);
                    }
                    return;
                case Team.ChaosInsurgency:
                    amount = (int)(50 * Plugin.GetConfig().Scp079TerminationRewardMultipliers[rt.GetTeam()]);
                    return;
                case Team.Scientists:
                    amount = (int)(40 * Plugin.GetConfig().Scp079TerminationRewardMultipliers[rt.GetTeam()]);
                    return;
                case Team.ClassD:
                    amount = (int)(30 * Plugin.GetConfig().Scp079TerminationRewardMultipliers[rt.GetTeam()]);
                    return;
                case Team.OtherAlive:
                    amount = (int)(50 * Plugin.GetConfig().Scp079TerminationRewardMultipliers[rt.GetTeam()]);
                    return;
            }   
            amount = 0;
            return;
        }
    }
}

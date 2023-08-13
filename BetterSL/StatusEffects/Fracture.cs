using CustomPlayerEffects;
using PlayerRoles.FirstPersonControl;
using PlayerStatsSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Core;

namespace BetterSL.StatusEffects
{
    public class Fracture : TickingEffectBase, IStaminaModifier, IMovementSpeedModifier
    {
        public bool StaminaModifierActive => IsEnabled;

        public float StaminaUsageMultiplier => 2f;

        public float StaminaRegenMultiplier => 0.7f;

        public bool SprintingDisabled => false;

        public bool MovementModifierActive => IsEnabled;

        public float MovementSpeedMultiplier => 0.75f;

        public float MovementSpeedLimit => float.MaxValue;

        protected override void OnTick()
        {
            base.OnEffectUpdate();
            IFpcRole fpcRole = Hub.roleManager.CurrentRole as IFpcRole;
            if (fpcRole == null)
            {
                return;
            }
            if(fpcRole.FpcModule.CurrentMovementState == PlayerMovementState.Sprinting)
            {
                CustomReasonDamageHandler handler = new CustomReasonDamageHandler("Inner trauma due to broken bones caused shock", 1f);
                Hub.playerStats.DealDamage(handler);
            }
        }
    }
}

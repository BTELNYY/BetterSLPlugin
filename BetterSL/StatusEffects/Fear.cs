using CustomPlayerEffects;
using HarmonyLib;
using PlayerRoles.FirstPersonControl;
using PlayerStatsSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetterSL.StatusEffects
{
    public class Fear : StatusEffectBase, IStaminaModifier, IMovementSpeedModifier
    {
        protected override void Enabled()
        {
            CurStamina = 0.75f;
            base.Enabled();
        }

        protected override void Disabled()
        {
            base.Disabled();
        }

        public float MaxStaminaWithEffect = 0.75f;

        public bool StaminaModifierActive => IsEnabled;

        public float StaminaUsageMultiplier => 1.5f;

        public float StaminaRegenMultiplier
        {
            get
            {
                if (CurStamina >= MaxStaminaWithEffect)
                {
                    return 0f;
                }
                return 0.5f;
            }
        }

        private float CurStamina
        {
            get
            {
                StaminaStat stat = Hub.playerStats.StatModules[2] as StaminaStat;
                return stat.CurValue;
            }
            set
            {
                StaminaStat stat = Hub.playerStats.StatModules[2] as StaminaStat;
                stat.CurValue = value;
            }
        }

        public bool SprintingDisabled => false;

        public bool MovementModifierActive => IsEnabled;

        public float MovementSpeedMultiplier => 1.15f;

        public float MovementSpeedLimit => float.MaxValue;
    }
}

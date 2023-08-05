using CustomPlayerEffects;
using PlayerRoles.FirstPersonControl;
using PlayerStatsSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetterSL.StatusEffects
{
    public class BetterScp207 : TickingEffectBase, IMovementSpeedModifier, IStaminaModifier, IHealablePlayerEffect
    {
        public bool MovementModifierActive => IsEnabled;

        public float MovementSpeedMultiplier => 1.2f * Intensity;

        public float MovementSpeedLimit => float.MaxValue;

        public bool StaminaModifierActive => IsEnabled;

        public float StaminaUsageMultiplier
        {
            get
            {
                if (CurStamina <= 0.025f)
                {
                    return 0f;
                }
                return 1f;
            }
        }

        public float StaminaRegenMultiplier => 1f;

        public bool SprintingDisabled => false;

        private float _tickDamage = 3f;
        private float _staminaDamageStartsAt = 0.05f;
        private float[] _speeds = new float[3]; 

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

        protected override void Enabled()
        {
            base.Enabled();
            IFpcRole fpcRole = Hub.roleManager.CurrentRole as IFpcRole;
            if (fpcRole == null)
            {
                return;
            }
            _speeds[0] = fpcRole.FpcModule.SprintSpeed;
            _speeds[1] = fpcRole.FpcModule.WalkSpeed;
            _speeds[2] = fpcRole.FpcModule.SneakSpeed;
            fpcRole.FpcModule.SprintSpeed *= MovementSpeedMultiplier;
            fpcRole.FpcModule.WalkSpeed *= MovementSpeedMultiplier;
            fpcRole.FpcModule.SneakSpeed *= MovementSpeedMultiplier;
        }

        protected override void Disabled()
        {
            IFpcRole fpcRole = Hub.roleManager.CurrentRole as IFpcRole;
            if (fpcRole == null)
            {
                return;
            }
            fpcRole.FpcModule.SprintSpeed *= _speeds[0];
            fpcRole.FpcModule.WalkSpeed *= _speeds[1];
            fpcRole.FpcModule.SneakSpeed *= _speeds[2];
            base.Disabled();
        }

        protected override void OnTick()
        {
            IFpcRole fpcRole = Hub.roleManager.CurrentRole as IFpcRole;
            if (fpcRole == null)
            {
                return;
            }
            if (Vitality.CheckPlayer(Hub))
            {
                return;
            }
            if(fpcRole.FpcModule.CurrentMovementState == PlayerMovementState.Sprinting && CurStamina <= _staminaDamageStartsAt)
            {
                UniversalDamageHandler handler = new UniversalDamageHandler(_tickDamage * Intensity, DeathTranslations.Scp207);
                Hub.playerStats.DealDamage(handler);
            }
        }

        public bool IsHealable(ItemType item)
        {
            if(item == ItemType.SCP500)
            {
                return true;
            }
            return false;
        }
    }
}
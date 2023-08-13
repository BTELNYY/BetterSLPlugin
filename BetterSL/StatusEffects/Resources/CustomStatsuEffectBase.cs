using BetterSL.Resources.NewStatModules;
using CustomPlayerEffects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetterSL.StatusEffects.Resources
{
    public class CustomStatsuEffectBase : TickingEffectBase
    {
        protected override void Disabled()
        {
            base.Disabled();
            NewHealthStat healthStat = Hub.playerStats.StatModules[0] as NewHealthStat;
            NewAhpStat ahpStat = Hub.playerStats.StatModules[1] as NewAhpStat;
            NewHumeShieldStat hsStat = Hub.playerStats.StatModules[4] as NewHumeShieldStat;
        }

        protected override void Enabled()
        {
            base.Enabled();
            NewHealthStat healthStat = Hub.playerStats.StatModules[0] as NewHealthStat;
            NewAhpStat ahpStat = Hub.playerStats.StatModules[1] as NewAhpStat;
            NewHumeShieldStat hsStat = Hub.playerStats.StatModules[4] as NewHumeShieldStat;
            if(this is IHealthModifier)
            {
                IHealthModifier modifier = this as IHealthModifier;
                if (modifier.HealthModifierEnabled)
                {
                    healthStat.SetMaxValue = modifier.HealthMaxValue;
                    healthStat.SetMinValue = modifier.HealthMinValue;
                    healthStat.CurValue = modifier.HealthCurrentValue;
                }
            }
            if (this is IAdditionalHealthModifier)
            {
                IAdditionalHealthModifier modifier = this as IAdditionalHealthModifier;
                if (modifier.AdditionalHealthModifierEnabled)
                {
                    ahpStat.SetMaxValue = modifier.AhpMaxValue;
                    ahpStat.SetMinValue = modifier.AhpMinValue;
                    ahpStat.CurValue = modifier.AhpCurrentValue;
                }
            }
            if(this is IHumeShieldModifier)
            {
                IHumeShieldModifier modifier = this as IHumeShieldModifier;
                if (modifier.HumeShieldModifierEnabled)
                {
                    hsStat.SetMaxValue = modifier.HsMaxValue;
                    hsStat.SetMinValue = modifier.HsMinValue;
                    hsStat.CurValue = modifier.HsCurrentValue;
                }
            }
        }

        protected override void OnTick()
        {
            
        }
    }
}

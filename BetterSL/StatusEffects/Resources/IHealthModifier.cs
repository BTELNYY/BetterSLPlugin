using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetterSL.StatusEffects.Resources
{
    public interface IHealthModifier
    {
        float HealthMaxValue { get; }
        float HealthMinValue { get; }
        float HealthCurrentValue { get; }
        bool HealthModifierEnabled { get; }
    }
}

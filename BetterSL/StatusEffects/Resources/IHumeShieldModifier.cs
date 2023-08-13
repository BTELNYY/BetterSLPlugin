using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetterSL.StatusEffects.Resources
{
    public interface IHumeShieldModifier
    {
        float HsMaxValue { get; }
        float HsMinValue { get; }
        float HsCurrentValue { get; }
        bool HumeShieldModifierEnabled { get; }
    }
}

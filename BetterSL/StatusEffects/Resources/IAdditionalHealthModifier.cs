using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetterSL.StatusEffects.Resources
{
    public interface IAdditionalHealthModifier
    {
        float AhpMaxValue { get; }
        float AhpMinValue { get; }
        float AhpCurrentValue { get; }
        bool AdditionalHealthModifierEnabled { get; }
    }
}

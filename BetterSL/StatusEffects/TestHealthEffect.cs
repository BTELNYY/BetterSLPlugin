﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BetterSL.Resources.NewStatModules;
using BetterSL.StatusEffects.Resources;
using PlayerStatsSystem;

namespace BetterSL.StatusEffects
{
    public class TestHealthEffect : CustomStatsuEffectBase, IHealthModifier
    {
        public float HealthMaxValue 
        {
            get
            {
                HealthStat stat = Hub.playerStats.StatModules[0] as HealthStat;
                float hp = stat.MaxValue - Intensity;
                return hp;
            }
        }

        public float HealthMinValue => 0f;

        public float HealthCurrentValue => Hub.playerStats.StatModules[0].CurValue;

        public bool HealthModifierEnabled => IsEnabled;
    }
}

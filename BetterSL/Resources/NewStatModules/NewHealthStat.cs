using BetterSL.StatusEffects.Resources;
using Mirror;
using PlayerRoles;
using PlayerStatsSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BetterSL.Resources.NewStatModules
{
    public class NewHealthStat : HealthStat, IStatStart
    {
        public override float CurValue { get => base.CurValue; set => base.CurValue = value; }

        public override float MaxValue => SetMaxValue;

        public float SetMaxValue = 100f;

        public override float MinValue => SetMinValue;

        public float SetMinValue = 0f;

        public new void ServerHeal(float healAmount)
        {
            CurValue = Mathf.Min(CurValue + Mathf.Abs(healAmount), MaxValue);
        }

        public void Start()
        {
            if(Hub.roleManager.CurrentRole is IHealthbarRole)
            {
                IHealthbarRole role = Hub.roleManager.CurrentRole as IHealthbarRole;
                SetMaxValue = role.MaxHealth;
            }
            else
            {
                SetMaxValue = 0f;
            }
        }
    }
}

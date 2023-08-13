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
    public class NewHealthStat : HealthStat
    {
        public override float CurValue { get => base.CurValue; set => base.CurValue = value; }

        public override float MaxValue => SetMaxValue;

        public float SetMaxValue 
        {
            get
            {
                IHealthbarRole healthbarRole = Hub.roleManager.CurrentRole as IHealthbarRole;
                if (healthbarRole == null)
                {
                    return 0f;
                }
                float roleMaxHealth = healthbarRole.MaxHealth;
                if(SetMaxValue != roleMaxHealth)
                {
                    return SetMaxValue;
                }
                else
                {
                    return roleMaxHealth;
                }
            }
            set
            {
                SetMaxValue = value;
            }
        }

        public override float MinValue => SetMinValue;

        public float SetMinValue { get; set; } = 0f;

        public new void ServerHeal(float healAmount)
        {
            CurValue = Mathf.Min(CurValue + Mathf.Abs(healAmount), MaxValue);
        }
    }
}

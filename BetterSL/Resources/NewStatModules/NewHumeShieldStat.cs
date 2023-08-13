using Mirror;
using PlayerRoles.PlayableScps.HumeShield;
using PlayerStatsSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BetterSL.Resources.NewStatModules
{
    public class NewHumeShieldStat : HumeShieldStat
    {
        public override float MinValue => SetMinValue;

        public float SetMinValue { get; set; } = 0f;

        public override float MaxValue => base.MaxValue;

        public float SetMaxValue
        {
            get
            {
                HumeShieldModuleBase humeShieldModuleBase;
                if (!TryGetHsModule(out humeShieldModuleBase))
                {
                    return SetMaxValue;
                }
                float roleMaxHs = humeShieldModuleBase.HsMax;
                if(SetMaxValue != roleMaxHs)
                {
                    return SetMaxValue;
                }
                else
                {
                    return roleMaxHs;
                }
            }
            set
            {
                SetMaxValue = value;
            }
        }

        public HumeShieldModuleBase HumeShieldModule
        {
            get
            {
                if (HumeShieldModule != null)
                {
                    return HumeShieldModule;
                }
                if (!TryGetHsModule(out HumeShieldModuleBase module))
                {
                    return null;
                }
                else
                {
                    HumeShieldModule = module;
                    return module;
                }
            }
            set
            {
                HumeShieldModule = value;
            }
        }

        private bool TryGetHsModule(out HumeShieldModuleBase controller)
        {
            IHumeShieldedRole humeShieldedRole = Hub.roleManager.CurrentRole as IHumeShieldedRole;
            if (humeShieldedRole != null)
            {
                controller = humeShieldedRole.HumeShieldModule;
                HumeShieldModule = controller;
                return true;
            }
            controller = null;
            return false;
        }
    }
}
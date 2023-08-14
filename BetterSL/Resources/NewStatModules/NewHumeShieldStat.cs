using BetterSL.StatusEffects.Resources;
using Mirror;
using PlayerRoles.PlayableScps.HumeShield;
using PlayerStatsSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BetterSL.Resources.NewStatModules
{
    public class NewHumeShieldStat : HumeShieldStat, IStatStart
    {
        public override float MinValue => SetMinValue;

        public float SetMinValue = 0f;

        public override float MaxValue => SetMaxValue;

        public float SetMaxValue = 100f;

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

        public void Start()
        {
            if (!TryGetHsModule(out var controller))
            {
                SetMaxValue = 0f;
            }
            SetMaxValue = controller.HsMax;
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
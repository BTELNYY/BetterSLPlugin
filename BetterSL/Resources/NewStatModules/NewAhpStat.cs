using HarmonyLib;
using Mirror;
using PlayerStatsSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BetterSL.Resources.NewStatModules
{
    public class NewAhpStat : AhpStat
    {
        public override float MaxValue => SetMaxValue;

        public float SetMaxValue
        {
            get
            {
                float maxSoFar = (float)AccessTools.Field(typeof(AhpStat), "_maxSoFar").GetValue(this);
                float setMaxValue = SetMaxValue;
                if(setMaxValue != maxSoFar)
                {
                    return setMaxValue;
                }
                else
                {
                    return maxSoFar;
                }
            }
            set
            {
                SetMaxValue = value;
            }
        }

        public override float MinValue => SetMinValue;

        public float SetMinValue { get; set; } = 0f;

        public List<AhpProcess> AhpProcesses 
        {
            get
            {
                List<AhpProcess> procList = AccessTools.FieldRefAccess<AhpStat, List<AhpProcess>>(this, "_activeProcesses");
                AhpProcesses = procList;
                return procList;
            }
            set
            {
                List<AhpProcess> procList = AccessTools.FieldRefAccess<AhpStat, List<AhpProcess>>(this, "_activeProcesses");
                procList = value;
                AhpProcesses = value;
            }
        }
    }
}

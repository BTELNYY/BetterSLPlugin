using BetterSL.StatusEffects.Resources;
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
    public class NewAhpStat : AhpStat, IStatStart
    {
        public override float CurValue { get => base.CurValue; set => base.CurValue = value; }


        public override float MaxValue => SetMaxValue;

        public float SetMaxValue = 75f;

        public override float MinValue => SetMinValue;

        public float SetMinValue = 0f;

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

        public void Start()
        {
            
        }
    }
}

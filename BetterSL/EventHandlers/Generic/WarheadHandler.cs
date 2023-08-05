using PluginAPI.Events;
using System;
using PluginAPI.Core.Attributes;
using PluginAPI.Enums;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MEC;
using MapGeneration;
using BetterSL.StatusEffects;

namespace BetterSL.EventHandlers.Generic
{
    public class WarheadHandler
    {
        [PluginEvent(ServerEventType.WarheadDetonation)]
        public void OnWarheadDetonate(WarheadDetonationEvent ev)
        {
            Timing.CallDelayed(Plugin.GetConfig().DetonationRadiationDelay, () =>
            {
                Timing.CallPeriodically(float.PositiveInfinity, Plugin.GetConfig().CheckInterval, () =>
                {
                    KillPlayers();
                });
            });
        }

        private void KillPlayers()
        {
            foreach (var player in BetterSL.Resources.Extensions.GetPlayersByZone(FacilityZone.Surface))
            {
                player.playerEffectsController.ChangeState<Radiation>(1, 999f);
            }
        }
    }
}

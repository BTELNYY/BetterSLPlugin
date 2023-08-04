using CustomPlayerEffects;
using PlayerStatsSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetterSL.StatusEffects
{
    public class BetterBleeding : TickingEffectBase, IHealablePlayerEffect
    {
        protected override void OnTick()
        {
            UniversalDamageHandler handler = new UniversalDamageHandler(DamagePerTick * Intensity, DeathTranslations.Bleeding);
            Hub.playerStats.DealDamage(handler);
            Hub.playerEffectsController.ServerSendPulse<Bleeding>();
        }

        public bool IsHealable(ItemType item)
        {
            if(item == ItemType.SCP500 || item == ItemType.Medkit)
            {
                return true;
            }
            return false;
        }

        public float DamagePerTick = 2f;
    }
}

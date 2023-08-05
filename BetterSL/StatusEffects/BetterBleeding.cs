using CustomPlayerEffects;
using Decals;
using InventorySystem.Items.Firearms.BasicMessages;
using PlayerRoles;
using PlayerStatsSystem;
using PluginAPI.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using PluginAPI.Core;
using Utils.Networking;

namespace BetterSL.StatusEffects
{
    public class BetterBleeding : TickingEffectBase, IHealablePlayerEffect
    {
        protected override void OnTick()
        {
            UniversalDamageHandler handler = new UniversalDamageHandler(DamagePerTick * Intensity, DeathTranslations.Bleeding);
            Hub.playerStats.DealDamage(handler);
            Hub.playerEffectsController.ServerSendPulse<Bleeding>();
            Vector3 position = Hub.gameObject.transform.position;
            position.x += 1f;
            Ray ray = new Ray(position, Vector3.down);
            if(!Physics.Raycast(ray, out RaycastHit hit))
            {
                Log.Error("Hit Failed!", nameof(BetterBleeding));
            }
            else
            {
                SendDecalMessage(ray, hit, DecalPoolType.Blood);
            }
        }

        public bool IsHealable(ItemType item)
        {
            if(item == ItemType.SCP500 || item == ItemType.Medkit)
            {
                return true;
            }
            return false;
        }

        private void SendDecalMessage(Ray ray, RaycastHit hit, DecalPoolType decal)
        {
            GunDecalMessage msg = new GunDecalMessage(hit.point + (ray.origin - hit.point).normalized, ray.direction, decal);
            NetworkUtils.SendToAuthenticated(msg, 0);
        }

        public float DamagePerTick = 2f;
    }
}

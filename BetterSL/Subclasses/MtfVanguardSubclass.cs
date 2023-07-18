using BetterSL.Resources;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetterSL.Subclasses
{
    public class MtfVanguardSubclass : BaseSubclass
    {
        public override string FileName => "mtf_vanguard";
        public override RoleTypeId BaseRole => RoleTypeId.NtfSergeant;
        public override string DisplayName => "Nine-Tailed Fox Vanguard";
        public override Dictionary<ItemType, uint> SpawnItems => new Dictionary<ItemType, uint>()
        {
            [ItemType.Medkit] = 1,
            [ItemType.ArmorCombat] = 1,
            [ItemType.GrenadeHE] = 1,
            [ItemType.GunShotgun] = 1,
            [ItemType.KeycardNTFLieutenant] = 1,
            [ItemType.Radio] = 1,
            [ItemType.GunCOM18] = 1,
            [ItemType.Ammo12gauge] = 56,
            [ItemType.Ammo9x19] = 90
        };
    }
}

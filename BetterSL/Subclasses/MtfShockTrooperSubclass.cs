using BetterSL.Resources;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetterSL.Subclasses
{
    public class MtfShockTrooperSubclass : BaseSubclass
    {
        public override string FileName => "mtf_shock_trooper";
        public override RoleTypeId BaseRole => RoleTypeId.NtfPrivate;
        public override string DisplayName => "Nine-Tailed Fox Shock Trooper";
        public override Dictionary<ItemType, uint> SpawnItems => new Dictionary<ItemType, uint>()
        {
            [ItemType.Medkit] = 1,
            [ItemType.ArmorCombat] = 1,
            [ItemType.GrenadeFlash] = 1,
            [ItemType.GunCrossvec] = 1,
            [ItemType.KeycardNTFLieutenant] = 1,
            [ItemType.Radio] = 1,
            [ItemType.Adrenaline] = 1,
            [ItemType.Ammo556x45] = 40,
            [ItemType.Ammo9x19] = 120
        };
    }
}

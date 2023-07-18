using BetterSL.Resources;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetterSL.Subclasses
{
    public class MtfMarksmanSubclass : BaseSubclass
    {
        public override string FileName => "mtf_marksman";
        public override RoleTypeId BaseRole => RoleTypeId.NtfCaptain;
        public override string DisplayName => "Nine-Tailed Fox Marksman";
        public override Dictionary<ItemType, uint> SpawnItems => new Dictionary<ItemType, uint>() 
        {
            [ItemType.Medkit] = 1,
            [ItemType.ArmorHeavy] = 1,
            [ItemType.Painkillers] = 1,
            [ItemType.GunE11SR] = 1,
            [ItemType.KeycardNTFCommander] = 1,
            [ItemType.Radio] = 1,
            [ItemType.Ammo556x45] = 160,
            [ItemType.Ammo9x19] = 40
        };
    }
}

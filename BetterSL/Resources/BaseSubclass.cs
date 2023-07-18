using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace BetterSL.Resources
{
    public class BaseSubclass
    {
        public virtual string FileName { get; set; } = "subclass";
        public virtual string DisplayName { get; set; } = "Placeholder Text!";
        public virtual string Description { get; set; } = "Placeholder Text!";
        public virtual RoleTypeId BaseRole { get; set; } = RoleTypeId.ClassD;
        public virtual Dictionary<ItemType, uint> SpawnItems { get; set; } = new Dictionary<ItemType, uint>
        {
            [ItemType.Ammo9x19] = 10,
        };
        public virtual bool HideActualRole { get; set; } = true;
    }
}
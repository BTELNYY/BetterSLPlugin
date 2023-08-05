using HarmonyLib;
using Hints;
using InventorySystem.Disarming;
using InventorySystem.Items;
using PluginAPI.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetterSL.Patches.Generic
{
    [HarmonyPatch(typeof(DisarmedPlayers), nameof(DisarmedPlayers.CanDisarm))]
    public class DisarmPatch
    {
        public static bool Prefix(ReferenceHub disarmerHub, ReferenceHub targetHub, ref bool __result)
        {
            int currentDisarmedPlayers = BetterSL.Resources.Extensions.GetAllDisarmedPlayersByDisarmer(disarmerHub).Count;
            ItemType currentItemType = disarmerHub.inventory.CurItem.TypeId;
            if (Plugin.GetConfig().MaxDisarmsPerWeapon[currentItemType] == -1)
            {
                return true;
            }
            if (Plugin.GetConfig().MaxDisarmsPerWeapon[currentItemType] <= currentDisarmedPlayers)
            {
                Player.Get(disarmerHub).SendBroadcast(Plugin.GetConfig().MaxDisarmsReachedMessage, 2);
                TextHint hint = new TextHint(Plugin.GetConfig().MaxDisarmsReachedMessage);
                disarmerHub.hints.Show(hint);
                __result = false;
                return false;
            }
            return true;
        }
    }
}

using HarmonyLib;
using Interactables.Interobjects;
using Interactables.Interobjects.DoorUtils;
using InventorySystem.Items.ThrowableProjectiles;
using MapGeneration;
using Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetterSL.Patches.Generic
{
    [HarmonyPatch(typeof(Scp2176Projectile), "ServerShatter")]
    public class Scp2176Patch
    {
        public static void Postfix(Scp2176Projectile __instance)
        {
            if (!NetworkServer.active)
            {
                throw new InvalidOperationException("Tried to call ServerShatter from the client!");
            }
            RoomIdentifier rid = RoomIdUtils.RoomAtPositionRaycasts(__instance.transform.position, true);
            if (rid == null)
            {
                return;
            }
            IEnumerable<RoomLightController> enumerable = from x in RoomLightController.Instances
                                                          where x.Room == rid
                                                          select x;

            HashSet<DoorVariant> doors;
            if (!DoorVariant.DoorsByRoom.TryGetValue(rid, out doors))
            {
                return;
            }
            foreach(DoorVariant door in doors)
            {
                if(door is ElevatorDoor && Plugin.GetConfig().Scp2176AffectsElevators)
                {
                    DoorLockMode mode = DoorLockUtils.GetMode((DoorLockReason)door.NetworkActiveLocks);
                    DoorLockReason reason = (DoorLockReason)door.NetworkActiveLocks;
                    if(reason == DoorLockReason.Lockdown2176)
                    {
                        door.ServerChangeLock(DoorLockReason.Lockdown2176, false);
                    }
                    else
                    {
                        door.ServerChangeLock(DoorLockReason.Lockdown2176, true);
                        door.UnlockLater(13f, DoorLockReason.Lockdown2176);
                    }
                }
            }
        }
    }
}

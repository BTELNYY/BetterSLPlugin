using Interactables.Interobjects.DoorUtils;
using PluginAPI.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PluginAPI.Core.Attributes;
using PluginAPI.Enums;

namespace BetterSL.EventHandlers.Scp079
{
    public class Scp079DoorHandler
    {
        public static int DoorsLocked = 0;

        [PluginEvent(ServerEventType.Scp079LockDoor)]
        public void OnDoorLock(Player player, DoorVariant door)
        {
            DoorsLocked++;
        }

        [PluginEvent(ServerEventType.Scp079UnlockDoor)]
        public void OnDoorUnlock(Player player, DoorVariant door)
        {
            DoorsLocked--;
        }
    }
}

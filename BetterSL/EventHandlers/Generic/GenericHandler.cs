using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PluginAPI.Core.Attributes;
using PluginAPI.Enums;
using BetterSL.EventHandlers.Scp079;

namespace BetterSL.EventHandlers.Generic
{
    public class GenericHandler
    {
        [PluginEvent(ServerEventType.RoundEnd)]
        public static void OnRoundEnd()
        {
            DimensionBodyHandler.RagdollQueue.Clear();
            DimensionBodyHandler.ThreadStarted = false;
            DimensionBodyHandler.Handler.IsRunning = false;
            Scp079DoorHandler.DoorsLocked = 0;
        }
    }
}

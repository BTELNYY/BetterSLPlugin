using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PluginAPI.Core.Attributes;
using PluginAPI.Enums;

namespace BetterSL.EventHandlers
{
    public class GenericHandler
    {
        [PluginEvent(ServerEventType.RoundEnd)]
        public static void OnRoundEnd()
        {
            DimensionBodyHandler.RagdollQueue.Clear();
            Scp079DoorHandler.DoorsLocked = 0;
            DimensionBodyHandler.ThreadStarted = false;
            DimensionBodyHandler.Handler.IsRunning = false;;
        }
    }
}

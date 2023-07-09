using PluginAPI.Events;
using PluginAPI.Core.Attributes;
using PluginAPI.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BetterSL.Patches.Generic;
using MapGeneration;

namespace BetterSL.EventHandlers.Generic
{
    public class BreakableWindowHealthHandler
    {
        [PluginEvent(ServerEventType.RoundStart)]
        public void OnRoundStart(RoundStartEvent ev)
        {
            foreach(BreakableWindow w in BreakableWindowPatch.BreakableWindows)
            {
                if(RoomIdUtils.RoomAtPositionRaycasts(w.transform.position).Name != RoomName.Hcz049)
                {
                    continue;
                }
                w.health = float.MaxValue;
            }
        }
    }
}

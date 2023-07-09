using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PluginAPI.Events;
using PluginAPI.Core.Attributes;
using PluginAPI.Enums;


namespace BetterSL.EventHandlers.Generic
{
    public class Hcz049Handler
    {
        [PluginEvent(ServerEventType.RoundStart)]
        public void OnRoundStart(RoundStartEvent ev)
        {

        }
    }
}

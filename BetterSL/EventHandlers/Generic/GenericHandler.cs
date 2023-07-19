﻿using System;
using PluginAPI.Core.Attributes;
using PluginAPI.Enums;

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
            ChaosTargetHandler.ChaosTotal = 0;
            ChaosTargetHandler.ChaosAlive = 0;
            ItemSpawnHandler.ModifiedItems.Clear();
            CorrodingHandler.CorrodedPlayers.Clear();
        }
    }
}
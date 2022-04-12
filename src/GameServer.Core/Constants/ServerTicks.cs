using System;
using System.Collections.Generic;
using System.Text;

namespace GameServer.Core.Constants
{
    public class ServerTicks
    {
        public const int TicksPerSecond = 128;
        public const int MilisecondsPerTick = 1000 / TicksPerSecond;
    }
}

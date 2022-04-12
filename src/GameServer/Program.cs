using GameServer.Core;
using GameServer.Core.Constants;
using GameServer.Core.Entities;
using System;
using System.Threading;

namespace GameServer
{
    internal class Program
    {
        private static bool isRunning = false;
        static void Main(string[] args)
        {
            Console.Title = "Game Server";
            isRunning = true;

            Thread mainThread = new Thread(new ThreadStart(MainThread));

            mainThread.Start();

            Server.Start(10, 26950);
        }

        private static void MainThread()
        {
            Console.WriteLine($"Main thread started running at {ServerTicks.TicksPerSecond} ticks per second.");
            DateTime nextLoop = DateTime.Now;

            while (isRunning) 
            {
                while(nextLoop < DateTime.Now) 
                {
                    GameLogic.Update();

                    nextLoop = nextLoop.AddMilliseconds(ServerTicks.MilisecondsPerTick);

                    if(nextLoop > DateTime.Now) 
                    {
                        Thread.Sleep(nextLoop - DateTime.Now);
                    }
                }
            }
        }
    }
}

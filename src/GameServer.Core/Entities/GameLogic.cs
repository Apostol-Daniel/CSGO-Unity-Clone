using System;
using System.Collections.Generic;
using System.Text;

namespace GameServer.Core.Entities
{
    public class GameLogic
    {
        public static void Update() 
        {
            //Workaround missing Update method from Unity Engine
            ThreadManager.UpdateMain();
        }
    }
}

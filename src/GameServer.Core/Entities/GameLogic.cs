using System;
using System.Collections.Generic;
using System.Text;

namespace GameServer.Core.Entities
{
    public class GameLogic
    {
        public static void Update() 
        {
            //Calculate movement of all connectef players
            foreach (Client client in Server.Clients.Values)
            {
                if(client.Player != null) 
                {
                    client.Player.Update();
                }
            }

            //Workaround missing Update method from Unity Engine
            ThreadManager.UpdateMain();
        }
    }
}

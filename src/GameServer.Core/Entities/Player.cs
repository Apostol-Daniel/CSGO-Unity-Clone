using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace GameServer.Core.Entities
{
    public class Player
    {
        public int Id;
        public string UserName;

        public Vector3 Position;

        public Quaternion Rotation;

        public Player(int id, string userName, Vector3 spawnPosition)
        {
            Id = id;
            UserName = userName;
            Position = spawnPosition;
            Rotation = Quaternion.Identity;
        }
    }
}

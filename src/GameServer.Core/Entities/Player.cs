using GameServer.Core.Constants;
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

        private float MoveSpeed = 5f / ServerTicks.TicksPerSecond;
        private bool[] Inputs;

        public Player(int id, string userName, Vector3 spawnPosition)
        {
            Id = id;
            UserName = userName;
            Position = spawnPosition;
            Rotation = Quaternion.Identity;
        }

        public void Update() 
        {
            Vector2 inputDirection = Vector2.Zero;

            if (Inputs[0]) 
            {
                inputDirection.Y += 1;
            }
            if (Inputs[1])
            {
                inputDirection.Y -= 1;
            }
            if (Inputs[2])
            {
                inputDirection.Y += 1;
            }
            if (Inputs[3])
            {
                inputDirection.Y -= 1;
            }

            Move(inputDirection);
        }

        private void Move(Vector2 inputDirection) 
        {
            Vector3 forward = Vector3.Transform(new Vector3(0,0,1), Rotation);
            Vector3 right = Vector3.Normalize( Vector3.Cross(forward, new Vector3(0,1,0)));

            Vector3 moveDirection = right * inputDirection.X + forward * inputDirection.Y;
            Position += moveDirection * MoveSpeed;

            ServerSend.PlayerPosition(this);
            ServerSend.PlayerRotation(this);
        }

        public void SetInput(bool[] inputs, Quaternion rotation) 
        {
            Inputs = inputs;
            Rotation = rotation;
        }
    }
}

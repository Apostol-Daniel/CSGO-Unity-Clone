using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Server_Scripts
{
    public class Enemy : MonoBehaviour
    {
        public static int MaxEnemies = 10;
        public static Dictionary<int, Enemy> Enemies = new Dictionary<int, Enemy>();
        private static int NextEnemyId = 1;

        public int Id;
        public EnemyState State;
        public Player Target;
        public CharacterController Controller;
        public Transform ShootOrigin;
        public float Gravity = -19.62f;
        public float PatrolSpeed = 2f;
        public float ChaseSpeed = 8f;
        public float Health;
        public float MaxHealth;
        public float DetectionRange = 30f;
        public float ShootRange = 15f;
        public float ShootAccuracy = 3f;
        public float IdleDuration = 1f;

        private bool IsPatrolRoutineRunning;
        private float YVelocity = 0f;

        private void Start()
        {
            Id = NextEnemyId;
            NextEnemyId++;
            Enemies.Add(Id, this);

            State = EnemyState.Patrol;
            Gravity *= Time.fixedDeltaTime * Time.fixedDeltaTime;
            PatrolSpeed *= Time.fixedDeltaTime;
            ChaseSpeed *= Time.fixedDeltaTime;
        }

        private void FixedUpdate()
        {
            switch (State) 
            {
                case EnemyState.Idle:
                    break;
                case EnemyState.Patrol:
                    break;
                case EnemyState.Chase:
                    break;
                case EnemyState.Attack:
                    break;
            }
        }

    }

    public enum EnemyState 
    {
        Idle,
        Patrol,
        Chase,
        Attack
    }
}

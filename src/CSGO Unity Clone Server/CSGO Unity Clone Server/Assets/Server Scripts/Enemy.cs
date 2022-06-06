using System.Collections;
using System.Collections.Generic;
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
        public float ShootAccuracy = 0.1f;
        public float PatrolDuration = 3f;
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
                    LookForPlayer();
                    break;
                case EnemyState.Patrol:
                    if (!LookForPlayer()) 
                    {
                        Patrol();
                    }
                    break;
                case EnemyState.Chase:
                    break;
                case EnemyState.Attack:
                    break;

                default:

                    break;
            }
        }

        public bool LookForPlayer()
        {
            foreach (Client client in Server.Clients.Values) 
            {
                if(client.Player != null) 
                {
                    Vector3 distanceToPlayer = client.Player.transform.position - transform.position;
                    if(distanceToPlayer.magnitude <= DetectionRange) 
                    {
                        if(Physics.Raycast(ShootOrigin.position, distanceToPlayer, out RaycastHit raycastHit, DetectionRange)) 
                        {
                            if (raycastHit.collider.CompareTag("Player")) 
                            {
                                Target = raycastHit.collider.GetComponent<Player>();
                                if (IsPatrolRoutineRunning) 
                                {
                                    IsPatrolRoutineRunning = false;
                                    StopCoroutine(StartPatrol());
                                }

                                State = EnemyState.Chase;
                                Debug.Log("Ai has found a target Player.");
                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }

        public void Patrol() 
        {
            if (!IsPatrolRoutineRunning) 
            {
                StartCoroutine(StartPatrol());
            }
        }

        private IEnumerator StartPatrol() 
        {
            IsPatrolRoutineRunning = true;
            Vector2 randomPatrolDirectrion = Random.insideUnitCircle.normalized;
            transform.forward = new Vector3(randomPatrolDirectrion.x, 0f, randomPatrolDirectrion.y);

            yield return new WaitForSeconds(PatrolDuration);

            State = EnemyState.Idle;

            yield return new WaitForSeconds(IdleDuration);

            State = EnemyState.Patrol;
            IsPatrolRoutineRunning = false;
        }

        private void Move(Vector3 direction, float speed) 
        {
            direction.y = 0f;
            transform.forward = direction;
            Vector3 movement = transform.forward * speed;

            if (Controller.isGrounded) 
            {
                YVelocity = 0f;
            }
            YVelocity += Gravity;

            movement.y = YVelocity;
            Controller.Move(movement);
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

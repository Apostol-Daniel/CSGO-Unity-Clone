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

            ServerSend.SpawnEnemy(this);

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
                    LookingForPlayer();
                    break;
                case EnemyState.Patrol:
                    if (!LookingForPlayer()) 
                    {
                        Patrol();
                    }
                    break;
                case EnemyState.Chase:
                    Chase();
                    break;
                case EnemyState.Attack:
                    Attack();
                    break;

                default:

                    break;
            }
        }

        public bool LookingForPlayer()
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

            Move(transform.forward, PatrolSpeed);
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

        private void Chase() 
        {
            if (CanSeeTarget())
            {
                Vector3 distanceToPlayer = Target.transform.position - transform.position;
                if (distanceToPlayer.magnitude <= ShootRange)
                {
                    State = EnemyState.Attack;
                }
                else
                {
                    Move(distanceToPlayer, ChaseSpeed);
                }

            }
            else 
            {
                Target = null;
                State = EnemyState.Patrol;
            }
        }

        private void Attack()
        {
            if (CanSeeTarget())
            {
                Vector3 distanceToPlayer = Target.transform.position - transform.position;
                transform.forward = new Vector3(distanceToPlayer.x, 0f, distanceToPlayer.z);

                if (distanceToPlayer.magnitude <= ShootRange)
                {
                    Shoot(distanceToPlayer);
                }
                else
                {
                    Move(distanceToPlayer, ChaseSpeed);
                }

            }
            else
            {
                Target = null;
                State = EnemyState.Patrol;
            }
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

            ServerSend.EnemyPosition(this);
        }

        private void Shoot(Vector3 shootDirection) 
        {
            if(Physics.Raycast(ShootOrigin.position, shootDirection, out RaycastHit raycastHit, ShootRange)) 
            {
                if (raycastHit.collider.CompareTag("Player"))                
                {
                    if(Random.value <= ShootAccuracy) 
                    {
                        raycastHit.collider.GetComponent<Player>().TakeDamage(25f);
                    }
                }
            }
        }

        private bool CanSeeTarget() 
        {
            if (Target == null) 
            {
                return false;
            }

            if(Physics.Raycast(ShootOrigin.position, Target.transform.position - transform.position, out RaycastHit raycastHit, DetectionRange)) 
            {
                if (raycastHit.collider.CompareTag("Player")) 
                {
                    return true;
                }
            }

            return false;
        }

        public void TakeDamage(float damage) 
        {
            Health -= damage;
            if(Health <= 0) 
            {
                Health = 0f;
                Enemies.Remove(Id);
                Destroy(gameObject);
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

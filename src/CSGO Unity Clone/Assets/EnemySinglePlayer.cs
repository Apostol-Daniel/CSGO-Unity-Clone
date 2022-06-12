using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySinglePlayer : MonoBehaviour
{
    public static EnemySinglePlayer EnemyInstance;

    public static int MaxEnemies = 5;
    public static Dictionary<int, EnemySinglePlayer> Enemies = new Dictionary<int, EnemySinglePlayer>();

    private static int NextEnemyId = 1;

    public int Id;
    public EnemyState State;
    public PlayerMovement Target;
    public CharacterController Controller;
    public Transform ShootOrigin;
    public float Gravity = -19.62f;
    public float PatrolSpeed = 2f;
    public float ChaseSpeed = 3f;
    public float Damage = 5f;
    public float Health;
    public float MaxHealth = 100f;
    public float DetectionRange = 15f;
    public float ShootRange = 5f;
    public float ShootAccuracy = 0.1f;
    public float PatrolDuration = 3f;
    public float IdleDuration = 1f;

    private bool IsPatrolRoutineRunning;
    private float YVelocity = 0f;

    public static EnemySinglePlayer Instance()
    {
        return EnemyInstance;
    }

    public void Reset()
    {

    }

    private void Start()
    {
        Id = NextEnemyId;
        NextEnemyId++;
        Enemies.Add(Id, this);
        Health = MaxHealth;

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
            if (Target != null)
            {
                Vector3 distanceToPlayer = Target.transform.position - transform.position;
                if (distanceToPlayer.magnitude <= DetectionRange)
                {
                    if (Physics.Raycast(ShootOrigin.position, distanceToPlayer, out RaycastHit raycastHit, DetectionRange))
                    {
                        if (raycastHit.collider.CompareTag("Player"))
                        {
                            Target = raycastHit.collider.GetComponent<PlayerMovement>();
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
    }

    private void Shoot(Vector3 shootDirection)
    {
        //Not implemented yet
        //Singleplayer player can not take damage yet
    }

    private bool CanSeeTarget()
    {
        if (Target == null)
        {
            return false;
        }

        if (Physics.Raycast(ShootOrigin.position, Target.transform.position - transform.position, out RaycastHit raycastHit, DetectionRange))
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
        if (Health <= 0)
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

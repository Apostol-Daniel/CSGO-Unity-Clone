using Assets.Server_Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int Id;
    public string UserName;
    public CharacterController CharacterController;
    public Transform ShootOrigin;

    public float Health;
    public float MaxHealth = 100f;
    public float MoveSpeed = 5f;
    public float Gravity = -19.62f;
    public float JumpHeight = 5;
    public float ThrowForce = 20f; 
    public int ItemAmmount = 0;
    public int MaxItemAmmount = 3;

    public float YVelocity = 0;
    private bool[] Inputs;

    public void Start()
    {
        Gravity *= Time.fixedDeltaTime * Time.fixedDeltaTime;
        MoveSpeed *= Time.fixedDeltaTime;
        JumpHeight *= Time.fixedDeltaTime;
    }
    public void Initialize(int id, string userName)
    {
        Id = id;
        UserName = userName;
        Health = MaxHealth;

        Inputs = new bool[5];
    }

    public void FixedUpdate()
    {
        if(Health <= 0f) 
        {
            return;
        }

        Vector2 inputDirection = Vector2.zero;

        if (Inputs[0])
        {
            inputDirection.y += 1;
        }
        if (Inputs[1])
        {
            inputDirection.y -= 1;
        }
        if (Inputs[2])
        {
            inputDirection.x -= 1;
        }
        if (Inputs[3])
        {
            inputDirection.x += 1;
        }

        Move(inputDirection);
    }

    private void Move(Vector2 inputDirection)
    {       
        Vector3 moveDirection = transform.right * inputDirection.x + transform.forward * inputDirection.y;
        moveDirection *= MoveSpeed;

        if (CharacterController.isGrounded) 
        {
            YVelocity = 0f;
            if (Inputs[4]) 
            {
                YVelocity = JumpHeight;
            }
        }

        YVelocity += Gravity;

        moveDirection.y = YVelocity;
        CharacterController.Move(moveDirection);

        ServerSend.PlayerPosition(this);
        ServerSend.PlayerRotation(this);
    }

    public void SetInput(bool[] inputs, Quaternion rotation)
    {
        Inputs = inputs;
        transform.rotation = rotation;
    }

    public void Shoot(Vector3 direction) 
    {

        if(Health <= 0f) 
        {
            return;
        }

        if(Physics.Raycast(ShootOrigin.position, direction, out RaycastHit hitInfo, 25f)) 
        {
            Debug.Log($"Target hit on Server before player tag:{hitInfo.collider.name}");

            if (hitInfo.collider.CompareTag("Player")) 
            {
                hitInfo.collider.GetComponentInParent<Player>().TakeDamage(25f);
                Debug.Log($"Player hit.");
            }
            else if (hitInfo.collider.CompareTag("Enemy"))
            {
                hitInfo.collider.GetComponentInParent<Enemy>().TakeDamage(2*25f);
                Debug.Log($"Enemy bot hit.");
            }
        }
    }

    public void ThrowItem(Vector3 direction) 
    {
        if(Health <= 0) 
        {
            return;
        }

        if(ItemAmmount > 0)
        {
            ItemAmmount--;
            NetworkManager.Instance.InstantiateProjectile(ShootOrigin).Initialize(direction, ThrowForce, Id);
        }
    }

    public void TakeDamage(float damage) 
    {
        if(Health <= 0f) 
        {
            return;
        }

        Health -= damage;
        if(Health <= 0f) 
        {
            Health = 0f;
            CharacterController.enabled = false;
            transform.position = new Vector3(0f,25f,0f);
            ServerSend.PlayerPosition(this);
            StartCoroutine(Respawn());
        }

        ServerSend.PlayerHealth(this);
    }

    private IEnumerator Respawn() 
    {
        yield return new WaitForSeconds(5f);
        Health = MaxHealth;
        CharacterController.enabled = true;
        ServerSend.PlayerRespawed(this);
    }

    public bool AttemptPickUpItem() 
    {
        if(ItemAmmount >= MaxItemAmmount) 
        {
            return false;
        }

        ItemAmmount++;

        return true;
    }
}

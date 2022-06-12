using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public int Id;
    public string UserName;
    public float Health;
    public float MaxHealth;
    public int ItemCount = 0;
    public MeshRenderer Model;
    public Text TxtHealth;
    public Text TxtGrenades;

    public void FixedUpdate()
    {
        TxtGrenades.text = ItemCount.ToString();
        TxtHealth.text = Health.ToString();
    }

    public void Initialize(int id, string userName)
    {
        Id = id;
        UserName = userName;
        Health = MaxHealth;    
    }

    public void SetHealth(float health) 
    {
        Health = health;     

        if(health <= 0f) 
        {
            Die();
        }
    }

    public void Die() 
    {
        Model.enabled = false;
    }

    public void Respawn() 
    {
        Model.enabled = true;
        SetHealth(MaxHealth);

    }
}

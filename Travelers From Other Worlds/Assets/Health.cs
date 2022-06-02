using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    // Start is called before the first frame update
    public float HealthPoints = 100;
    public float MaxHealthPoints = 100;
    public float ShieldPoints = 100;
    public float MaxShieldPoints = 100;
    public float ShieldRegenCooldown = 1.0f;
    public float ShieldRegenSpeed = 1.0f;

    void Start()
    {

    }

    public void TakeDamage(float damage)
    {
        if (ShieldPoints > 0)
        {
            ShieldPoints -= damage;
            if (ShieldPoints < 0)
            {
                HealthPoints += ShieldPoints;
                ShieldPoints = 0;
            }
        }
        else
        {
            HealthPoints -= damage;
        }
    }
    public void RegenHealth(float health)
    {
        HealthPoints += health;
        if (HealthPoints > MaxHealthPoints)
        {
            HealthPoints = MaxHealthPoints;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (HealthPoints <= 0)
        {
            //game over
        }
        if (ShieldPoints < MaxShieldPoints)
        {
            ShieldRegenCooldown -= Time.deltaTime;
            if (ShieldRegenCooldown <= 0)
            {
                ShieldPoints += ShieldRegenSpeed;
                if(ShieldPoints > MaxShieldPoints)
                {
                    ShieldPoints = MaxShieldPoints;
                }
                ShieldRegenCooldown = 1.0f;
            }
        }
        if (ShieldPoints < 0)
        {
            HealthPoints += ShieldPoints;
            ShieldPoints = 0;
        }
    }
}

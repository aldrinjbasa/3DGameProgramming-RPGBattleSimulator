using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Humanoid : MonoBehaviour
{

    public string charName;
    public int level;
    public float baseDamage;
    public float damage;

    public float currentHealth;
    public float maxHealth;

    public int currentMP;
    public int maxMP;

    public int currentLimit;
    public int maxLimit;

    public float str;
    public float luck;



    public bool TakeDamage(float damageTaken) //Argument is used as BaseDamage
    {
        currentHealth -= damageTaken;
        if(currentHealth <= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public float CalculateDamage()
    {
        damage = Mathf.Floor(baseDamage + (baseDamage * (str / 100)) + Random.Range(0, ((baseDamage / 2) * (luck / 3))));
        return damage;
    }

}

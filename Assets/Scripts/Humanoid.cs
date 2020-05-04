using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Humanoid : MonoBehaviour
{

    public string charName;
    public int level;
    public int damage;

    public int currentHealth;
    public int maxHealth;

    public int currentMP;
    public int maxMP;

    public int currentLimit;
    public int maxLimit;



    public bool DamageTaken(int damageTaken)
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


}

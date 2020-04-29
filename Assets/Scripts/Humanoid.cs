using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Humanoid : MonoBehaviour
{

    public int level;
    public int damage;
    public int currentHealth;
    public int maxHealth;

    public bool DamageTaken(int damageTaken)
    {
        currentHealth -= damageTaken;

        if(currentHealth <= 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }


}

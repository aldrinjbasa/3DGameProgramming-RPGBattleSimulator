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

    public float currentLimit;
    public float maxLimit;

    public float currentXP;
    public float maxXP = 100;
    public float giveXP;

    public int gold;
    public int giveGold;

    public float str;
    public float intelligence;
    public float luck;

    public string sceneComingFrom;

    private void Start()
    {
    }

    public bool TakeDamage(float damageTaken) //Argument is used as BaseDamage
    {
        currentHealth -= damageTaken;
        if(damageTaken < 0)
        {
            //Do Nothing
        }
        else
        {
            currentLimit += damageTaken * (str / 200);
        }
        
        if(currentHealth <= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public int removeMP(int mpToRemove)
    {
        currentMP -= mpToRemove;
        return currentMP;
    }

    public float CalculateDamage()
    {
        damage = Mathf.Floor(baseDamage + (baseDamage * (str / 100)) + Random.Range(0, ((baseDamage / 2) * (luck / 3))));
        return damage;
    }

    public float CalculateXSlash()
    {
        damage = Mathf.Floor(baseDamage + (baseDamage * (str / 10)) + Random.Range(0, ((baseDamage / 2) * (luck / 3))));
        return damage;
    }

    public float Cure()
    {
        damage = Mathf.Floor(-1 * (200 + (100 * (intelligence / 100)) + Random.Range(0, ((100 / 2) * (luck / 3)))));
        return damage;
    }
    public float CalculateJump()
    {
        damage = Mathf.Floor((baseDamage + (baseDamage * (str / 5)) + Random.Range(0, ((baseDamage / 2) * (luck / 3)))));
        return damage;
    }
    public float CalculateBigBoyMove()
    {
        damage = Mathf.Floor((baseDamage + (baseDamage * (str / 2)) + Random.Range(0, ((baseDamage / 2) * (luck / 3)))));
        return damage;
    }

    public void giveRewardOnKill(Humanoid humanoidToGiveReward, Humanoid humanoidWithReward)
    {
        humanoidToGiveReward.currentXP += humanoidWithReward.giveXP;
        humanoidToGiveReward.gold += humanoidWithReward.giveGold;
    }

    public void updateBattleStats(Humanoid playerHumanoid)
    {
        playerHumanoid.charName = charName;
        playerHumanoid.currentHealth = currentHealth;
        playerHumanoid.maxHealth = maxHealth;
        playerHumanoid.currentMP = currentMP;
        playerHumanoid.maxMP = maxMP;
        playerHumanoid.currentLimit = currentLimit;
        playerHumanoid.maxLimit = maxLimit;
        playerHumanoid.baseDamage = baseDamage;
        playerHumanoid.str = str;
        playerHumanoid.intelligence = intelligence;
        playerHumanoid.luck = luck;
        playerHumanoid.gold = gold;
        playerHumanoid.currentXP = currentXP;
        playerHumanoid.maxXP = maxXP;
        playerHumanoid.currentHealth = playerHumanoid.maxHealth;
        playerHumanoid.currentMP = playerHumanoid.maxMP;
    }

    void Update()
    {
        //Level Up
        if(currentXP >= maxXP)
        {
            level++;
            currentXP = 0;
            maxXP += Mathf.Floor(4 * (level ^ 3) / 5);
            maxHealth += 100;
            str += 3;
            intelligence += 3;
            luck += 3;
            baseDamage += 10;
        }
    }

}

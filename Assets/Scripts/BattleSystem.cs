using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum BattleState { START, PLAYERTURN, ENEMYTURN, WIN, DEFEAT }
public class BattleSystem : MonoBehaviour
{

    public BattleState currentState;
    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    Humanoid playerHumanoid;
    Humanoid enemyHumanoid;

    void Start()
    {
        currentState = BattleState.START;
    }

    IEnumerator StartBattle()
    {
        yield return new WaitForSeconds(2f);
        currentState = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    IEnumerator Attack()
    {
        bool isDead = enemyHumanoid.DamageTaken(playerHumanoid.damage);

        //Update Enemy HealthBar

        /////
        
        yield return new WaitForSeconds(2f);
        if (isDead)
        {
            currentState = BattleState.WIN;
            EndBattle();
        }
        else
        {
            currentState = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
    }

    void PlayerTurn()
    {

    }

    IEnumerator EnemyTurn()
    {
        yield return new WaitForSeconds(2f);

        //Enemy Attack
        bool isDead = playerHumanoid.DamageTaken(enemyHumanoid.damage);
        //Update Player Health Bar

        //////
        yield return new WaitForSeconds(1f);
        if (isDead)
        {
            currentState = BattleState.DEFEAT;
            EndBattle();
        }
        else
        {
            currentState = BattleState.PLAYERTURN;
            PlayerTurn();
        }
    }


    public void OnAttack()
    {
        if(currentState != BattleState.PLAYERTURN)
        {
            return;
        }

        StartCoroutine(Attack());
    }

    void EndBattle()
    {
        if(currentState == BattleState.WIN)
        {
            //Destroy Enemy
            //Reward EXP
        }
        else if(currentState == BattleState.DEFEAT)
        {
            //Stuff to put when you lose
        }
    }
}

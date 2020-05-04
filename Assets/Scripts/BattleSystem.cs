using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WIN, DEFEAT }
public class BattleSystem : MonoBehaviour
{

    public BattleState currentState;
    public GameObject playerPrefab;
    public GameObject enemyPrefab;
    public GameObject playerObject;
    public GameObject enemyObject;
    public Transform playerPlatform;
    public Transform enemyPlatform;

    public GameObject BattleUIMenu;
    public GameObject BattleUIAlert;

    Humanoid playerHumanoid;
    Humanoid enemyHumanoid;
    private Vector3 adjustVector;

    public BattleUI playerUI;

    void Start()
    {
        currentState = BattleState.START;

        StartCoroutine(StartBattle());
    }

    IEnumerator StartBattle()
    {
        adjustVector = new Vector3(0, 1, 0);
        playerObject = Instantiate(playerPrefab, playerPlatform.transform.position, Quaternion.identity);
        playerObject.transform.position += adjustVector;
        //playerObject.transform.SetParent(playerPlatform, true);
        playerHumanoid = playerObject.GetComponent<Humanoid>();

        enemyObject = Instantiate(enemyPrefab, enemyPlatform.transform.position, Quaternion.identity);
        enemyObject.transform.position += adjustVector;
        //enemyObject.transform.SetParent(enemyPlatform, true);
        enemyHumanoid = enemyObject.GetComponent<Humanoid>();

        playerUI.StartHUD(playerHumanoid);

        yield return new WaitForSeconds(2f);
        currentState = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    IEnumerator Attack()
    {
        BattleUIMenu.SetActive(false);
        //PlayAnimation
        playerObject.transform.position = enemyObject.transform.position;
        Vector3 hitAdjust = new Vector3(0f, 0f, 1f);
        playerObject.transform.position += hitAdjust;
        yield return new WaitForSeconds(0.5f);
        playerObject.GetComponent<Animator>().Play("Player_Attack");

        //Deal Damage to Enemy
        playerHumanoid.CalculateDamage();
        bool isDead = enemyHumanoid.TakeDamage(playerHumanoid.damage);

        //Update Enemy HealthBar (If I decide to implement one l o l)
        playerUI.ShowDamage(enemyHumanoid, playerHumanoid.damage);

        //Animation Buffer
        yield return new WaitForSeconds(1f);

        //Move Back Animation
        playerObject.GetComponent<Animator>().Play("Player_Idle");
        yield return new WaitForSeconds(0.5f);
        while(playerObject.transform.position != playerPlatform.transform.position) //Move Back
        {
            playerObject.transform.position = Vector3.MoveTowards(playerObject.transform.position, playerPlatform.transform.position, 1f * Time.deltaTime);
        }
        playerObject.transform.position += adjustVector;

        //Check for game state
        if (isDead)
        {
            currentState = BattleState.WIN;
            enemyObject.GetComponent<Animator>().Play("Enemy_Death");
            yield return new WaitForSeconds(1f);
            Destroy(enemyObject);
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
        BattleUIMenu.SetActive(true);
    }

    IEnumerator EnemyTurn()
    {
        yield return new WaitForSeconds(1f);

        //PlayAnimation
        enemyObject.transform.position = playerObject.transform.position;
        Vector3 hitAdjust = new Vector3(0f, 0f, 1f);
        enemyObject.transform.position -= hitAdjust;
        yield return new WaitForSeconds(0.5f);
        enemyObject.GetComponent<Animator>().Play("Enemy_Attack");

        //Enemy Attack
        enemyHumanoid.CalculateDamage();
        bool isDead = playerHumanoid.TakeDamage(enemyHumanoid.damage);

        //Update Player Health Bar
        playerUI.UpdateHP(playerHumanoid);
        playerUI.ShowDamage(playerHumanoid, enemyHumanoid.damage);

        //Animation Buffer
        yield return new WaitForSeconds(1f);

        //Move Back Animation
        enemyObject.GetComponent<Animator>().Play("Enemy_Idle");
        yield return new WaitForSeconds(0.5f);
        while (enemyObject.transform.position != enemyPlatform.transform.position) //Move back
        {
            enemyObject.transform.position = Vector3.MoveTowards(enemyObject.transform.position, enemyPlatform.transform.position, 1f * Time.deltaTime);
        }
        enemyObject.transform.position += adjustVector;

        //Check for game state
        if (isDead)
        {
            currentState = BattleState.DEFEAT;
            playerObject.GetComponent<Animator>().Play("Player_Death");
            yield return new WaitForSeconds(1f);
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
            //Alert player of win
            BattleUIAlert.GetComponentInChildren<Text>().text = "You've beaten " + enemyHumanoid.charName + " !";
            BattleUIAlert.SetActive(true);
            //Reward EXP

            //Return to Game World
            StartCoroutine(ReturnToWorld());
        }
        else if(currentState == BattleState.DEFEAT)
        {
            //Stuff to put when you lose
            BattleUIAlert.GetComponentInChildren<Text>().text = "You lost" + enemyHumanoid.charName + " !";
            BattleUIAlert.SetActive(true);
            StartCoroutine(ReturnToWorld()); //Change this to game over on final game
        }
    }

    IEnumerator ReturnToWorld()
    {
        yield return new WaitForSeconds(2.5f);
        SceneManager.LoadScene(0);
    }
}

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
    public GameObject SkillMenu;
    public GameObject Limit;
    public GameObject LimitMenu;
    public GameObject Credits;
    public GameObject player;

    Humanoid playerHumanoid;
    Humanoid enemyHumanoid;
    private Vector3 adjustVector;

    public BattleUI playerUI;

    void Start()
    {
        currentState = BattleState.START;
        player = GameObject.Find("Player");
        StartCoroutine(StartBattle());
    }

    IEnumerator StartBattle()
    {
        adjustVector = new Vector3(0, 1, 0);
        playerObject = Instantiate(playerPrefab, playerPlatform.transform.position, Quaternion.identity);
        playerObject.transform.position += adjustVector;
        //playerObject.transform.SetParent(playerPlatform, true);
        playerHumanoid = playerObject.GetComponent<Humanoid>();
        player.GetComponent<Humanoid>().updateBattleStats(playerHumanoid);
        enemyObject = Instantiate(enemyPrefab, enemyPlatform.transform.position, Quaternion.identity);
        enemyObject.transform.position += adjustVector;
        //enemyObject.transform.SetParent(enemyPlatform, true);
        enemyHumanoid = enemyObject.GetComponent<Humanoid>();

        playerUI.StartHUD(playerHumanoid);
        player.SetActive(false);
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
        FindObjectOfType<AudioManager>().Play("BasicAttack");

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
        if(playerHumanoid.currentLimit >= playerHumanoid.maxLimit)
        {
            playerHumanoid.currentLimit = playerHumanoid.maxLimit;
            Limit.SetActive(true);
        }
    }

    IEnumerator EnemyTurn()
    {
        yield return new WaitForSeconds(1f);

        int enemyAction = Random.Range(1, 5);

        //PlayAnimation(Attack)
        enemyObject.transform.position = playerObject.transform.position;
        Vector3 hitAdjust = new Vector3(0f, 0f, 1f);
        enemyObject.transform.position -= hitAdjust;
        yield return new WaitForSeconds(0.5f);
        //AI For Boss
        if(enemyObject.name == "Boss(Clone)")
        {
            if (enemyAction != 4)
            {
                enemyObject.GetComponent<Animator>().Play("Enemy_Attack");
                enemyHumanoid.CalculateDamage();
                FindObjectOfType<AudioManager>().Play("BasicAttack");
            }
            //PlayAnimation(Jump)
            else if (enemyAction == 4)
            {
                BattleUIAlert.GetComponentInChildren<Text>().text = "Jump";
                BattleUIAlert.SetActive(true);
                enemyObject.GetComponent<Animator>().Play("Boss_Jump");
                enemyHumanoid.CalculateJump();
                yield return new WaitForSeconds(2f);
                FindObjectOfType<AudioManager>().Play("Slash");
            }
        }
        else
        {
            enemyObject.GetComponent<Animator>().Play("Enemy_Attack");
            enemyHumanoid.CalculateDamage();
        }
       

        //Enemy Attack
        bool isDead = playerHumanoid.TakeDamage(enemyHumanoid.damage);

        //Update Player Health Bar
        playerUI.UpdateHP(playerHumanoid);
        playerUI.ShowDamage(playerHumanoid, enemyHumanoid.damage);
        playerUI.UpdateLimit(playerHumanoid);

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
        FindObjectOfType<AudioManager>().Play("Menu");
        StartCoroutine(Attack());
    }

    

    IEnumerator ReturnToWorld(string sceneName)
    {
        yield return new WaitForSeconds(2.5f);
        player.SetActive(true);
        FindObjectOfType<AudioManager>().Play("WorldMusic");
        SceneManager.LoadScene(sceneName);
    }


    public void OpenSkillMenu()
    {
        if(LimitMenu.activeInHierarchy == true)
        {
            LimitMenu.SetActive(false);
        }
        FindObjectOfType<AudioManager>().Play("Menu");
        SkillMenu.SetActive(true);
    }

    public void CloseSkillMenu()
    {
        FindObjectOfType<AudioManager>().Play("Menu");
        SkillMenu.SetActive(false);
    }

    public void OpenLimitMenu()
    {
        if(SkillMenu.activeInHierarchy == true)
        {
            SkillMenu.SetActive(false);
        }
        FindObjectOfType<AudioManager>().Play("Menu");
        LimitMenu.SetActive(true);
    }

    public void CloseLimitMenu()
    {
        FindObjectOfType<AudioManager>().Play("Menu");
        LimitMenu.SetActive(false);
    }

    public void OnXSlash()
    {
        if (currentState != BattleState.PLAYERTURN)
        {
            return;
        }
        FindObjectOfType<AudioManager>().Play("Menu");
        StartCoroutine(XSlash());
    }

    public void OnCure()
    {
        FindObjectOfType<AudioManager>().Play("Menu");
        if (currentState != BattleState.PLAYERTURN)
        {
            return;
        }
        if(playerHumanoid.currentMP <= 0)
        {
            //Do Nothing (Should throw a "No MP Text")
        }
        else
        {
            StartCoroutine(Cure());
        }
    }

    public void OnBigBoyMove()
    {
        if (currentState != BattleState.PLAYERTURN)
        {
            return;
        }
        FindObjectOfType<AudioManager>().Play("Menu");
        StartCoroutine(BigBoyMove());
    }

    IEnumerator XSlash()
    {
        BattleUIMenu.SetActive(false);
        BattleUIAlert.GetComponentInChildren<Text>().text = "X-Slash";
        BattleUIAlert.SetActive(true);
        //PlayAnimation
        playerObject.transform.position = enemyObject.transform.position;
        Vector3 hitAdjust = new Vector3(0f, 0f, 1f);
        playerObject.transform.position += hitAdjust;
        yield return new WaitForSeconds(0.5f);
        playerObject.GetComponent<Animator>().Play("Player_XSlash");
        yield return new WaitForSeconds(1f); //Wait for XSlash to finish
        BattleUIAlert.SetActive(false);

        //Deal Damage to Enemy
        playerHumanoid.CalculateXSlash();
        bool isDead = enemyHumanoid.TakeDamage(playerHumanoid.damage);

        //Update Enemy HealthBar (If I decide to implement one l o l)
        playerUI.ShowDamage(enemyHumanoid, playerHumanoid.damage);
        FindObjectOfType<AudioManager>().Play("Slash");

        //Deal Damage Again
        playerHumanoid.CalculateXSlash();
        isDead = enemyHumanoid.TakeDamage(playerHumanoid.damage);
        yield return new WaitForSeconds(0.5f);
        playerUI.ShowDamage(enemyHumanoid, playerHumanoid.damage);
        FindObjectOfType<AudioManager>().Play("Slash");

        //Animation Buffer
        yield return new WaitForSeconds(1f);

        //Move Back Animation
        playerObject.GetComponent<Animator>().Play("Player_Idle");
        yield return new WaitForSeconds(0.5f);
        while (playerObject.transform.position != playerPlatform.transform.position) //Move Back
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
            EndBattle();
        }
        else
        {
            currentState = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
    }

    IEnumerator Cure()
    {
        BattleUIMenu.SetActive(false);
        BattleUIAlert.GetComponentInChildren<Text>().text = "Cure 1";
        BattleUIAlert.SetActive(true);
        //PlayAnimation
        ParticleSystem cureAnimation = playerObject.GetComponent<ParticleSystem>();
        cureAnimation.Play();
        ParticleSystem.EmissionModule cureParticles = cureAnimation.emission;
        cureParticles.enabled = true;
        yield return new WaitForSeconds(1.5f); //Wait for Cure to finish
        BattleUIAlert.SetActive(false);
        cureAnimation.Stop();
        cureParticles.enabled = false;

        //Deal Damage to Enemy
        playerHumanoid.Cure();
        bool isDead = playerHumanoid.TakeDamage(playerHumanoid.damage);
        playerHumanoid.removeMP(5);
        if(playerHumanoid.currentHealth >= playerHumanoid.maxHealth)
        {
            playerHumanoid.currentHealth = playerHumanoid.maxHealth;
        }

        //Update Player HealthBar
        playerUI.ShowDamage(playerHumanoid, playerHumanoid.damage);
        playerUI.UpdateHP(playerHumanoid);

        //Animation Buffer
        yield return new WaitForSeconds(1f);

        //Move Back Animation
        playerObject.GetComponent<Animator>().Play("Player_Idle");
        yield return new WaitForSeconds(0.5f);

        //Check for game state
        if (isDead)
        {
            currentState = BattleState.WIN;
            enemyObject.GetComponent<Animator>().Play("Enemy_Death");
            yield return new WaitForSeconds(1f);
            EndBattle();
        }
        else
        {
            currentState = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
    }

    IEnumerator BigBoyMove()
    {
        BattleUIMenu.SetActive(false);
        BattleUIAlert.GetComponentInChildren<Text>().text = "Limit: Big Boy Move";
        BattleUIAlert.SetActive(true);
        //PlayAnimation
        playerObject.transform.position = enemyObject.transform.position;
        Vector3 hitAdjust = new Vector3(0f, 0f, 1f);
        playerObject.transform.position += hitAdjust;
        yield return new WaitForSeconds(0.5f);
        playerObject.GetComponent<Animator>().Play("Player_Limit1");
        yield return new WaitForSeconds(3f); //Wait for XSlash to finish
        BattleUIAlert.SetActive(false);

        //Deal Damage to Enemy
        playerHumanoid.CalculateBigBoyMove();
        bool isDead = enemyHumanoid.TakeDamage(playerHumanoid.damage);
        FindObjectOfType<AudioManager>().Play("Slash");

        //Update Enemy HealthBar
        playerUI.ShowDamage(enemyHumanoid, playerHumanoid.damage);
        playerHumanoid.currentLimit = 0;
        playerUI.UpdateLimit(playerHumanoid);
        Limit.SetActive(false);

        //Animation Buffer
        yield return new WaitForSeconds(1f);

        //Move Back Animation
        playerObject.GetComponent<Animator>().Play("Player_Idle");
        yield return new WaitForSeconds(0.5f);
        while (playerObject.transform.position != playerPlatform.transform.position) //Move Back
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
            EndBattle();
        }
        else
        {
            currentState = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
    }

    public void toCredits()
    {
        SceneManager.LoadScene("Credits");
    }

    void EndBattle()
    {
        if (currentState == BattleState.WIN)
        {
            //Alert player of win
            BattleUIAlert.GetComponentInChildren<Text>().text = "You've beaten " + enemyHumanoid.charName + " !";
            BattleUIAlert.SetActive(true);
            //Reward Player with XP
            enemyObject.GetComponent<Humanoid>().giveRewardOnKill(playerObject.GetComponent<Humanoid>(), enemyObject.GetComponent<Humanoid>());
            BattleUIAlert.GetComponentInChildren<Text>().text = "Gained " + enemyHumanoid.giveXP + "XP and " + enemyHumanoid.giveGold + " gold!";
            playerHumanoid.currentXP += enemyHumanoid.giveXP;
            playerHumanoid.gold += enemyHumanoid.giveGold;
            playerObject.GetComponent<Humanoid>().updateBattleStats(player.GetComponent<Humanoid>());
            if(enemyObject.name == "Boss(Clone)")
            {
                BattleUIAlert.GetComponentInChildren<Text>().text = "Thank you for playing.";
                Credits.SetActive(true);
            }
            //Return to Game World
            else if(enemyObject.name != "Boss(Clone)")
            {
                FindObjectOfType<AudioManager>().Stop("BattleMusic");
                StartCoroutine(ReturnToWorld("Grass Field 1"));
            }
        }
        else if (currentState == BattleState.DEFEAT)
        {
            //Stuff to put when you lose
            BattleUIAlert.GetComponentInChildren<Text>().text = "You lost to " + enemyHumanoid.charName + " !";
            BattleUIAlert.SetActive(true);
            FindObjectOfType<AudioManager>().Stop("BattleMusic");
            FindObjectOfType<AudioManager>().Play("WorldMusic");
            if (enemyObject.name == "Boss(Clone)")
            {
                StartCoroutine(ReturnToWorld("Boss Room"));
            }
            else
            {
                StartCoroutine(ReturnToWorld("Grass Field 1"));
            }
        }
    }

}

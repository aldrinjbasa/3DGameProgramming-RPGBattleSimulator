using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

public class WorldHandler : MonoBehaviour
{
    public GameObject AudioManager;

    // Start is called before the first frame update
    void Start()
    {
        if(SceneManager.GetActiveScene().name == "Home")
        {
            AudioManager.GetComponent<AudioManager>().Play("WorldMusic");
        }
        else if(SceneManager.GetActiveScene().name == "Grass Field 1")
        {
            StartCoroutine(startPositionGrassField());
        }
        else if(SceneManager.GetActiveScene().name == "Boss Room")
        {
            StartCoroutine(startPositionBossRoom());
        }
        else if(SceneManager.GetActiveScene().name == "Battle" || SceneManager.GetActiveScene().name == "Battle2")
        {
            AudioManager.GetComponent<AudioManager>().Stop("WorldMusic");
            AudioManager.GetComponent<AudioManager>().Play("BattleMusic");
        }
    }


    IEnumerator startPositionGrassField()
    {
        yield return new WaitForSeconds(0.3f);
        Vector3 startPosition = new Vector3(6.49f, 0.75f, -0.769f);
        GameObject.Find("Player").transform.position = startPosition;
        GameObject.Find("Player").GetComponent<NavMeshAgent>().enabled = true;
    }

    IEnumerator startPositionBossRoom()
    {
        yield return new WaitForSeconds(0.3f);
        Vector3 startPosition = new Vector3(6.49f, 0.75f, -0.769f);
        GameObject.Find("Player").transform.position = startPosition;
        GameObject.Find("Player").GetComponent<NavMeshAgent>().enabled = true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

public class WorldHandler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if(SceneManager.GetActiveScene().name == "Grass Field 1")
        {
            StartCoroutine(startPositionGrassField());
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Vector3 startPosition = new Vector3(6.49f, 0.75f, -0.769f);
        //GameObject.Find("Player").transform.position = startPosition;
    }

    IEnumerator startPositionGrassField()
    {
        yield return new WaitForSeconds(0.3f);
        Vector3 startPosition = new Vector3(6.49f, 0.75f, -0.769f);
        GameObject.Find("Player").transform.position = startPosition;
        GameObject.Find("Player").GetComponent<NavMeshAgent>().enabled = true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{

    public GameObject player;
    public LayerMask movementLayer;
    private NavMeshAgent playerNavMeshAgent;
    public GameObject battleTransition;


    // Start is called before the first frame update
    void Start()
    {
        playerNavMeshAgent = GetComponent<NavMeshAgent>();
        player = GameObject.Find("Player").gameObject;
        DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    void Update()
    {
        battleTransition = GameObject.Find("BattleTransitions");
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit clickPosition;
            Ray clickRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            
            if(Physics.Raycast(clickRay, out clickPosition, 300, movementLayer))
            {
                playerNavMeshAgent.SetDestination(clickPosition.point);
            }
        }
    }

    IEnumerator Transition(int levelIndex, string transitionTrigger)
    {
        if(levelIndex == 2)
        {
            if (battleTransition.transform.GetChild(0).gameObject.activeSelf == false)
            {
                battleTransition.transform.GetChild(0).gameObject.SetActive(true);
            }
        }
        battleTransition.GetComponent<Animator>().SetTrigger(transitionTrigger);
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(levelIndex);

    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Player entered trigger");
        playerNavMeshAgent.enabled = false;
        if(other.tag == "Enemy")
        {
            if(other.gameObject.name == "Enemy")
            {
                StartCoroutine(Transition(2, "StartBattleTransition"));
            }
            else if (other.gameObject.name == "Boss")
            {
                StartCoroutine(Transition(4, "StartBattleTransition"));
            }

        }
        else if(other.tag == "Portal")
        {
            if(other.gameObject.name == "Portal")
            {
                StartCoroutine(Transition(1, "StartMapTransition"));
            }
            else if(other.gameObject.name == "PortalToBoss")
            {
                StartCoroutine(Transition(3, "StartMapTransition"));
            }
        }
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{

    public LayerMask movementLayer;
    private NavMeshAgent playerNavMeshAgent;
    public Animator transitionAnimator;


    // Start is called before the first frame update
    void Start()
    {
        playerNavMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            RaycastHit clickPosition;
            Ray clickRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            
            if(Physics.Raycast(clickRay, out clickPosition, 300, movementLayer))
            {
                playerNavMeshAgent.SetDestination(clickPosition.point);
            }
        }
    }

    IEnumerator BattleTransition(int levelIndex)
    {
        transitionAnimator.SetTrigger("StartBattleTransition");
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(levelIndex);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Player entered trigger");
        if(other.tag == "Enemy")
        {
            StartCoroutine(BattleTransition(1));
        }
        
    }
}

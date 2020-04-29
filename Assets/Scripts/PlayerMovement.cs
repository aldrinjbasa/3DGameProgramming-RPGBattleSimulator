using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour
{

    public LayerMask movementLayer;
    private NavMeshAgent playerNavMeshAgent;


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
}

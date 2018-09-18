using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : MonoBehaviour {
    [Header("Settings")]
    public float VisionDistance;
    public LayerMask DetectableLayers;

    public GenericGun AttachedGun;

    //Private Variables
    NavMeshAgent agent;
    Transform player;
    

    Vector3 LastSeenPlayer;
	// Use this for initialization
	void Start () {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.Find("Player").transform;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        agent.SetDestination(player.position);

        Ray ray = new Ray(transform.position, player.GetComponent<Movement>().GetMiddle() - transform.position);
        Debug.DrawRay(transform.position, player.GetComponent<Movement>().GetMiddle() - transform.position, Color.red, Time.fixedDeltaTime);
        RaycastHit hit;
        Physics.Raycast(ray, out hit, VisionDistance, DetectableLayers);

        if (hit.collider != null)
        {
            if (hit.transform.name == "Player")
            {
                AttachedGun.Shoot();
            }
        }
      
	}
}

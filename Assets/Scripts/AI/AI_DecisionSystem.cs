using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI_DecisionSystem : MonoBehaviour {
    public Transform[] PatrolWayPoints;

    AI_SensorySystem.SensoryInfo info;
    int patrolDest = 0;
    NavMeshAgent agent;

	// Use this for initialization
	void Start () {
        agent = GetComponent<NavMeshAgent>();
        SetupAgent();
	}
	
    void SetupAgent()
    {
        agent.autoBraking = false;
    }


    public void RecieveSensoryInfo(AI_SensorySystem.SensoryInfo _info)
    {
        info = _info;
        MakeDecision();
    }

    void MakeDecision()
    {
        switch (info._alertnessState)
        {
            case AI_SensorySystem.AlertnessStates.Low:
                Patrol();
                break;
            case AI_SensorySystem.AlertnessStates.Medium:
                Patrol();
                break;
            case AI_SensorySystem.AlertnessStates.High:
                ChasePlayer();
                break;
        }
    }
    
    void Patrol()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            GotoNextPatrolPoint();
        }
    }

    void GotoNextPatrolPoint()
    {
        if (PatrolWayPoints.Length == 0)
        {
            Debug.LogWarningFormat("{0} doesn't have specified patrol points");
            return;
        }
        agent.destination = PatrolWayPoints[patrolDest].position;
        patrolDest = (patrolDest + 1) % PatrolWayPoints.Length;
    }

    void ChasePlayer()
    {
        if(info.movementTarget == Vector3.zero)
        {
            SearchPlayer();
        }else
        {
            agent.SetDestination(info.movementTarget);
        }
    }

    void SearchPlayer()
    {

    }
    // Update is called once per frame
    void Update () {

    }
}

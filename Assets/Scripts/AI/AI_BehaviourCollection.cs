﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class AI_BehaviourCollection : MonoBehaviour {
    public Transform[] PatrolWayPoints;
    public AI_Alarm[] Alarms;
    int patrolDest = 0;
    NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        SetupAgent();
    }

    void SetupAgent()
    {
        agent.autoBraking = false;
    }

    public void Patrol()
    {
        if (!agent.pathPending && isOnPatrolWaypoint())
        {
            GotoNextPatrolPoint();
        }
    }

    public void Shoot()
    {
        print("BANG BANG BANG PULLED MY DEVIL TRIGGER");
        GetComponentInChildren<GenericGun>().Shoot();
    }

    public void Melee()
    {
        print("MELEE");
    }

    public void Investigate(Transform poi)
    {
        agent.destination = poi.position;
    }

    public void SoundAlarm()
    {
        Transform closestAlarm = Alarms[0].transform;
        for(int i = 0; i < Alarms.Length; i++)
        {
            if(Vector3.Distance(transform.position, Alarms[i].transform.position) <= Vector3.Distance(transform.position, closestAlarm.position))
            {
                closestAlarm = Alarms[i].transform;
            }
        }

        agent.destination = closestAlarm.position;

        if (agent.remainingDistance < 0.5f)
        {
            closestAlarm.GetComponent<AI_Alarm>().Sound();
        }
    }

    public bool isOnPatrolWaypoint()
    {
        return (agent.remainingDistance < 0.5f);
    }

    void GotoNextPatrolPoint()
    {
        if (PatrolWayPoints.Length == 0)
        {
            Debug.LogWarningFormat("{0} doesn't have specified patrol points", name);
            return;
        }
        agent.destination = PatrolWayPoints[patrolDest].position;
        patrolDest = (patrolDest + 1) % PatrolWayPoints.Length;
    }
}
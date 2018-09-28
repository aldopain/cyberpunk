using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class AI_BehaviourCollection : MonoBehaviour {
    public Transform[] PatrolWayPoints;
    public AI_Alarm[] Alarms;
    public float TargetAvoidanceRadius;
    public float ApproachAngleError;

    public GameObject DEBUG_DESTINATION_MARKER;

    float _angleError;
    float _currentAvoidanceRadius;
    int patrolDest = 0;
    Vector3 prevPOI;
    Vector3 pos;
    NavMeshAgent agent;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        SetupAgent();
        _currentAvoidanceRadius = TargetAvoidanceRadius;
    }

    void Update()
    {
        DEBUG_DESTINATION_MARKER.transform.position = agent.destination;
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

    public void Stop()
    {
        agent.isStopped = true;
    }

    public void Shoot()
    {
        GetComponentInChildren<GenericGun>().Shoot();
    }

    public void Melee()
    {
        print("MELEE");
    }

    public void Investigate(Vector3 poi, bool _aimAtPoi = true, bool _direct = false)
    {
        if (_direct)
        {
            agent.isStopped = false;
            agent.destination = poi;
        }
        else
        {
            if (Vector3.Distance(poi, prevPOI) > .3f)
            {
                agent.isStopped = false;
                agent.updateRotation = true;
                _currentAvoidanceRadius = TargetAvoidanceRadius;
                _angleError = Random.Range(-ApproachAngleError / 2, ApproachAngleError / 2);
            }
            else
            {
                if(agent.remainingDistance < 0.5f)
                {
                    //_currentAvoidanceRadius /= 4;
                    agent.isStopped = true;
                    agent.updateRotation = false;
                    transform.LookAt(poi);
                }
            }

            pos = (transform.position - poi);
            pos.Normalize();
            pos *= _currentAvoidanceRadius;

            //PUT ACTUAL VECTOR ROTATION HERE

            agent.destination = pos + poi;
        }

        if (_aimAtPoi)
        {
            agent.updateRotation = false;
            transform.LookAt(poi);
        }else
        {
            agent.updateRotation = true;
        }
        prevPOI = poi;
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

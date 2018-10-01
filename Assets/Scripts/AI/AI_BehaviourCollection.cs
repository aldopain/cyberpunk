using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class AI_BehaviourCollection : MonoBehaviour {
    [Header ("Pathfinding Settings")]
    public Transform[] PatrolWayPoints;         
    public AI_Alarm[] Alarms;                   //All alarms that AI can trigger
    public float TargetAvoidanceRadius;         //Distance to the player
    public float ApproachAngleError;            //Used for slight path variation

    [Header("DEBUG")]
    public GameObject DEBUG_DESTINATION_MARKER;


    private float _angleError;
    private float _currentAvoidanceRadius;
    private int patrolDest = 0;
    private Vector3 prevPOI;
    private Vector3 pos;
    private NavMeshAgent agent;

    //Sets up initial variables
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        SetupAgent();
        _currentAvoidanceRadius = TargetAvoidanceRadius;
    }

    //Default Update is not needed for AI to function, used only for debugging
    void Update()
    {
        DEBUG_DESTINATION_MARKER.transform.position = agent.destination;
    }

    //More default settings that would be applied at the start should go here
    void SetupAgent()
    {
        agent.autoBraking = false;
    }

    //Patrol behaviour
    public void Patrol()
    {
        if (!agent.pathPending && isOnPatrolWaypoint())
        {
            GotoNextPatrolPoint();
        }
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

    //Immideatly stops agent
    public void Stop()
    {
        agent.isStopped = true;
    }

    //Forces agent to restart
    public void ForceStart()
    {
        agent.isStopped = false;
    }
    //Shoots the attached gun
    public void Shoot()
    {
        GetComponentInChildren<GenericGun>().Shoot();
    }

    //PLACEHOLDER. TO BE FIXED WHEN ACTUAL MELEE CODE IS DONE
    public void Melee()
    {
        print("MELEE");
    }

    //Goes to the specified point
    public void Investigate(Vector3 poi, bool _aimAtPoi = true, bool _direct = false)
    {
        if (_direct)                                                                            //if direct flag is true, then go to the point
        {
            agent.isStopped = false;
            agent.destination = poi;
        }
        else
        {
            if (Vector3.Distance(poi, prevPOI) > .3f)                                           //if destination moved, then find the new approach angle and reset the avoidance radius
            {
                agent.isStopped = false;
                agent.updateRotation = true;
                _currentAvoidanceRadius = TargetAvoidanceRadius;
                _angleError = Random.Range(-ApproachAngleError / 2, ApproachAngleError / 2);
            }
            else
            {
                if(agent.remainingDistance < 0.5f)                                              //This fixes jerky actor movement when it arrives at the destination
                {
                    agent.isStopped = true;
                    agent.updateRotation = false;
                    transform.LookAt(poi);
                }
            }

            pos = (transform.position - poi);                                                   //choosing approach point
            pos.Normalize();
            pos *= _currentAvoidanceRadius;

            //PUT ACTUAL VECTOR ROTATION HERE                                                   //randomizing the approach angle so agents would take different paths

            agent.destination = pos + poi;
        }

        if (_aimAtPoi)                                                                          //basically, does actor need to aim at a player or not
        {
            agent.updateRotation = false;
            transform.LookAt(poi);
        }else
        {
            agent.updateRotation = true;
        }
        prevPOI = poi;
    }

    //Trigger closest alarm
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

        if (agent.remainingDistance < 0.5f)                                     //if actor is close enough, then trigger the alarm
        {
            closestAlarm.GetComponent<AI_Alarm>().Sound();
        }
    }

    //Possibly interchangeable functions
    public bool isOnPatrolWaypoint()
    {
        return (agent.remainingDistance < 0.5f);
    }

    public bool isNear(Vector3 pos)
    {
        return (Vector3.Distance(transform.position, pos) < 0.5f);
    }
}

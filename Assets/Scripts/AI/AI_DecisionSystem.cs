using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI_DecisionSystem : MonoBehaviour {
    public float TEMP_MELEE_RANGE; //TO BE REPLACE WHEN ACTUAL MELEE COMPONENT IS COMPLETE

    AI_SensorySystem.SensoryInfo info;
    AI_BehaviourCollection behaviour;
    GameObject _player;

    AI_SensorySystem.AlertnessStates prevState;
    void Start()
    {
        behaviour = GetComponent<AI_BehaviourCollection>();
        _player = GameObject.Find("Player");
        prevState = AI_SensorySystem.AlertnessStates.Low;
    }

    public void RecieveSensoryInfo(AI_SensorySystem.SensoryInfo _info)
    {
        prevState = info._alertnessState;
        info = _info;
        MakeDecision();
    }

    void MakeDecision()
    {
        switch (info._alertnessState)
        {
            case AI_SensorySystem.AlertnessStates.Low:
                CallBehaviour_Low();
                break;
            case AI_SensorySystem.AlertnessStates.Medium:
                CallBehaviour_Medium();
                break;
            case AI_SensorySystem.AlertnessStates.High:
                CallBehaviour_High();
                break;
        }
    }
    
    void CallBehaviour_Low()
    {
        behaviour.Patrol();
    }

    void CallBehaviour_Medium()
    {
        if (info.hasSeenPlayer && !info.isSeeingPlayer)
        {
            //TO BE FIXED WITH ACTUAL "LAST SEEN POSITION" SYSTEM
            behaviour.Investigate(GameObject.Find("Player").transform);
        }

        if (info.hasHeardPlayer)
        {
            behaviour.Investigate(info.PointsOfInterest[0]);
            return;
        }

        behaviour.Patrol();
    }

    void CallBehaviour_High()
    {
        if(info.isSeeingPlayer && Vector3.Distance(transform.position, GameObject.Find("Player").transform.position) < TEMP_MELEE_RANGE)
        {
            behaviour.Melee();
            return;
        }

        if (info.isSeeingPlayer)
        {
            behaviour.Shoot();
            return;
        }

        //TO BE FIXED WITH ACTUAL "LAST SEEN POSITION" SYSTEM
        if (info.hearsGlobalAlert)
        {
            behaviour.Investigate(GameObject.Find("Player").transform);
        }else
        {
            behaviour.SoundAlarm();
        }
    }


    // Update is called once per frame
    void Update () {

    }
}

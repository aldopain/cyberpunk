using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI_DecisionSystem : MonoBehaviour {
    public bool BlockingSensoryInfo;
    public float TEMP_MELEE_RANGE; //TO BE REPLACE WHEN ACTUAL MELEE COMPONENT IS COMPLETE
    public int MaxIdleTicks;

    AI_SensorySystem.SensoryInfo info;
    AI_BehaviourCollection behaviour;
    GameObject _player;

    AI_SensorySystem.AlertnessStates prevState;
    Vector3 prevPosition;
    AI_SensorySystem.PointOfInterest prevPOI;
    int _idleTicks;
    bool _forceMovement;
    void Start()
    {
        behaviour = GetComponent<AI_BehaviourCollection>();
        _player = GameObject.Find("Player");
        prevState = AI_SensorySystem.AlertnessStates.Low;
    }

    public void RecieveSensoryInfo(AI_SensorySystem.SensoryInfo _info)
    {
        if (!BlockingSensoryInfo)
        {
            prevState = info._alertnessState;
            info = _info;
            HandleIdleTicks();
            MakeDecision();
        }else
        {
            behaviour.Stop();
        }
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
    
    void HandleIdleTicks()
    {
        if (transform.position == prevPosition)
        {
            _idleTicks++;
        }
        else
        {
            _idleTicks = 0;
        }

        if (_idleTicks > MaxIdleTicks)
        {
            _forceMovement = true;
        }else
        {
            if (info.isSeeingPlayer)
            {
                _forceMovement = false;
            }
        }
        prevPosition = transform.position;
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
            behaviour.Investigate(GameObject.Find("Player").transform.position);
        }

        if (info.hasHeardPlayer)
        {
            behaviour.Investigate(info.poi[0].position);
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
            behaviour.Investigate(ChooseTargetPOI(), info.isSeeingPlayer, _forceMovement);
        }else
        {
            behaviour.SoundAlarm();
        }
    }
    
    Vector3 ChooseTargetPOI()
    {
        AI_SensorySystem.PointOfInterest[] poiArr = info.poi.ToArray();
        if(poiArr.Length == 0)
        {
            if (prevPOI.position == Vector3.zero && prevPOI.ThreatLevel == AI_SensorySystem.AlertnessStates.Low && !prevPOI.isVisual)
            {
                print("What");
                prevPOI.position = GameObject.Find("Player").transform.position;
                return GameObject.Find("Player").transform.position;
            }else
            {
                return prevPOI.position;
            }
        }
        AI_SensorySystem.PointOfInterest tmp = new AI_SensorySystem.PointOfInterest(Vector3.zero, false, AI_SensorySystem.AlertnessStates.Low);

        //Bubble sort by threat level
        for(int i = 0; i < poiArr.Length; i++)
        {
            for(int j = 0; j < poiArr.Length - 1; j++)
            {
                if(poiArr[j].ThreatLevel >  poiArr[j + 1].ThreatLevel)
                {
                    tmp = poiArr[j + 1];
                    poiArr[j + 1] = poiArr[j];
                    poiArr[j] = tmp;
                }
            }
        }

        //Get max threat level and number of max level pois
        AI_SensorySystem.AlertnessStates maxThreatState = poiArr[poiArr.Length - 1].ThreatLevel;
        int maxThreat_FirstIndex = poiArr.Length - 1;
        if (poiArr[0].ThreatLevel == maxThreatState)
        {
            maxThreat_FirstIndex = 0;
        }
        else
        {
            while (poiArr[maxThreat_FirstIndex].ThreatLevel == maxThreatState)
            {
                maxThreat_FirstIndex--;
            }
        }

        List<AI_SensorySystem.PointOfInterest> maxThreatPOI = info.poi.GetRange(maxThreat_FirstIndex, poiArr.Length - maxThreat_FirstIndex);
        List<AI_SensorySystem.PointOfInterest> visualPOI = new List<AI_SensorySystem.PointOfInterest>();
        foreach(AI_SensorySystem.PointOfInterest poi in maxThreatPOI)
        {
            if (poi.isVisual)
            {
                visualPOI.Add(poi);
            }
        }

        Vector3 target;
        if(visualPOI.ToArray().Length == 0)             //Get the closest sound
        {
            target = maxThreatPOI.ToArray()[0].position;
            foreach (AI_SensorySystem.PointOfInterest poi in maxThreatPOI)
            {
                if(Vector3.Distance(transform.position, poi.position) < Vector3.Distance(transform.position, target))
                {
                    target = poi.position;
                }   
            }
        }
        else                                           //Get the closest visual poi
        {
            target = visualPOI.ToArray()[0].position;
            foreach (AI_SensorySystem.PointOfInterest poi in visualPOI)
            {
                if (Vector3.Distance(transform.position, poi.position) < Vector3.Distance(transform.position, target))
                {
                    target = poi.position;
                }
            }
        }

        return target;
    }
}

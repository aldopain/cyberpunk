using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI_DecisionSystem : MonoBehaviour {
    public bool BlockingSensoryInfo;                //does this actor blocks the information coming from sensory System; is used to imitate the "stun" effect
    public float TEMP_MELEE_RANGE;                  //TO BE REPLACED WHEN ACTUAL MELEE COMPONENT IS COMPLETE
    public int MaxIdleTicks;                        //Maximum amount of sensory ticks actor is willing to wait until forcing itself to move

    AI_SensorySystem.SensoryInfo info;              //recieved Sensory Info that is processed this tick
    AI_BehaviourCollection behaviour;               
    GameObject _player;                             //TO BE FIXED: possibly redundant

    AI_SensorySystem.AlertnessStates prevState;     
    Vector3 prevPosition;                           //previous position of the actor; is used in counting idle ticks
    AI_SensorySystem.PointOfInterest prevPOI;       //last POI; possibly deprecated
    Vector3 lastSeenPlayer;
    int _idleTicks;
    bool _forceMovement;            

    //Initial Setup
    void Start()
    {
        behaviour = GetComponent<AI_BehaviourCollection>();
        _player = GameObject.Find("Player");                    //TO BE FIXED: possibly redundant
        prevState = AI_SensorySystem.AlertnessStates.Low;
    }

    //TO BE FIXED:
    //Needs better system for clearing already visited points of interest
    //Update shouldn't be used for actual actor manipulation
    void Update()
    {
        if(prevPOI.position != Vector3.zero && info._alertnessState == AI_SensorySystem.AlertnessStates.High && behaviour.isNear(prevPOI.position))
        {
            print("Clears POI");
            prevPOI.position = Vector3.zero;
            prevPOI.isVisual = false;
            prevPOI.ThreatLevel = AI_SensorySystem.AlertnessStates.Low;
        }

        if (behaviour.isNear(lastSeenPlayer))
        {
            lastSeenPlayer = Vector3.zero;
        }
    }

    //Function that is called by Sensory System to send its info here
    public void RecieveSensoryInfo(AI_SensorySystem.SensoryInfo _info)
    {
        if (!BlockingSensoryInfo)
        {
            prevState = info._alertnessState;
            info = _info;

            if (info.isSeeingPlayer)
            {
                lastSeenPlayer = _player.transform.position;
            }

            HandleIdleTicks();
            MakeDecision();
        }else
        {
            behaviour.Stop();
        }
    }

    //Choose one of three behaviour branches depending on alertness state
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
    
    //Counts idle ticks and force actor's movement
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

    /* Calls low alertness behavior
     * 
     * For now it always patrols; TO BE FIXED with different behaviours
     * 
     */
    void CallBehaviour_Low()
    {
        if (_forceMovement)
        {
            behaviour.ForceStart();
        }
        behaviour.Patrol();
    }

    // Calls medium alertness behaviour
    void CallBehaviour_Medium()
    {
        if (!info.isSeeingPlayer)
        {
            if(lastSeenPlayer == Vector3.zero)
            {
                print("Patroling");
                behaviour.Patrol();
            }else
            {
                print("Going to the last player position");
                behaviour.Investigate(lastSeenPlayer, false, _forceMovement);
            }

            return;
        }else
        {
            //Debug.Break();
            print("Following player");
            behaviour.Investigate(_player.transform.position, false, _forceMovement);
            return;
        }

        behaviour.Patrol();
    }

    // Calls high alert behaviour
    void CallBehaviour_High()
    {
        if(info.isSeeingPlayer && Vector3.Distance(transform.position, GameObject.Find("Player").transform.position) < TEMP_MELEE_RANGE)        //If can see player
        {
            behaviour.Melee();
            return;
        }

        if (info.isSeeingPlayer)
        {
            behaviour.Shoot();
            behaviour.Investigate(_player.transform.position, true, _forceMovement);
            return;
        }

        if (info.hearsGlobalAlert)                                                                                                              //if can't see player
        {
            if (lastSeenPlayer == Vector3.zero)
            {
                behaviour.Investigate(_player.transform.position, false, _forceMovement);
            }
            else
            {
                behaviour.Investigate(lastSeenPlayer, false, _forceMovement);
            }
        }else
        {
            behaviour.SoundAlarm();
        }
    }
    


    //Chooses most important point of interest
    Vector3 ChooseTargetPOI()
    {
        AI_SensorySystem.PointOfInterest[] poiArr = info.poi.ToArray();

        //if no pois detected
        if(poiArr.Length == 0)
        {
            print("have no pois to work with");
            if (prevPOI.position == Vector3.zero && prevPOI.ThreatLevel == AI_SensorySystem.AlertnessStates.Low && !prevPOI.isVisual)
            {
                return Vector3.zero;
            }else
            {
                return prevPOI.position;
            }
        }

        print("have pois to work with");
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

        prevPOI.position = target;
        return target;
    }
}

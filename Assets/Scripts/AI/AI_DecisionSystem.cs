using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI_DecisionSystem : MonoBehaviour {
    public bool BlockingSensoryInfo;                //does this agent blocks the information coming from sensory System; is used to imitate the "stun" effect
    public float TEMP_MELEE_RANGE;                  //TO BE REPLACED WHEN ACTUAL MELEE COMPONENT IS COMPLETE
    public int MaxIdleTicks;                        //Maximum amount of sensory ticks agent is willing to wait until forcing itself to move

    AI_SensorySystem.SensoryInfo info;              //recieved Sensory Info that is processed this tick
    AI_BehaviourCollection behaviour;               
    GameObject _player;                             

    AI_SensorySystem.AlertnessStates prevState;     
    Vector3 prevPosition;                           //previous position of the agent; is used in counting idle ticks
    Vector3 lastSeenPlayer;
    int _idleTicks;
    bool _forceMovement;
    bool _canCheat;                                 
    //Initial Setup
    void Start()
    {
        behaviour = GetComponent<AI_BehaviourCollection>();
        _player = GameObject.Find("Player");                    
        prevState = AI_SensorySystem.AlertnessStates.Low;
        _canCheat = true;
    }

    //Clear last seen player position, if agent arrived at it
    void Update()
    {
        if (behaviour.isNear(lastSeenPlayer))
        {
            behaviour.PatrolAround_BuildPath(lastSeenPlayer);
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
                _canCheat = true;
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
    
    //Counts idle ticks and force agent's movement
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

    // Calls low alertness behavior
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
                behaviour.Patrol();
            }else
            {
                behaviour.Investigate(lastSeenPlayer, false, _forceMovement);
            }

            return;
        }else
        {
            behaviour.Investigate(lastSeenPlayer, true, _forceMovement);
            return;
        }
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
            behaviour.Investigate(lastSeenPlayer, true, _forceMovement);
            return;
        }

        if (info.hearsGlobalAlert)                                                                                                              //if can't see player
        {
            if (lastSeenPlayer == Vector3.zero)
            {
                behaviour.PatrolAround();
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
    
    public void ForceLastSeenPosition()
    {
        lastSeenPlayer = _player.transform.position;
    }
}

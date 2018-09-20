using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_SensorySystem : MonoBehaviour {
    public struct SensoryInfo
    {
        AlertnessStates _alertnessState;
        int currentHealth;
        int playerHealth;

    }

    enum AlertnessStates
    {
        Low,
        Medium,
        High
    }

    [Header("Settings")]
    public float TimeToDetectPlayer;
    
    [Header("System")]
    public float UpdateRate;

    AlertnessStates CurrentAlertnessState;
    float CurrentAlertnessLevel;
    List<GameObject> VisibleObjects;
    List<GameObject> RealSounds;
    List<GameObject> PseudoSounds;

    bool isSeeingPlayer;
    bool isSeenByPlayer;

    GameObject _player;

    void OnBecameVisible()
    {
        isSeenByPlayer = true;
    }

	// Use this for initialization
	void Start () {
        _player = GameObject.Find("Player");
	}
	
	// Update is called once per frame
	void Update () {
        if (isSeeingPlayer)
        {
            if(CurrentAlertnessLevel < TimeToDetectPlayer)
            {
                IncreaseAlertness();
            }else
            {
                DetectPlayer();
            }
        }else
        {   if(CurrentAlertnessLevel > 0)
            {
                DecreaseAlertness();
            }
        }
	}

    public void RecieveVision(List<GameObject> visibleObjects)
    {
        VisibleObjects = visibleObjects;
        if (VisibleObjects.Contains(_player))
        {
            isSeeingPlayer = true;
        }else
        {
            isSeeingPlayer = false;
        }
    }

    public void RecieveSounds(List<GameObject> real, List<GameObject> pseudo)
    {
        RealSounds = real;
        PseudoSounds = pseudo;
    }
    
    void IncreaseAlertness(float multiplier = 1f)
    {
        CurrentAlertnessLevel += Time.deltaTime;
    }

    void DecreaseAlertness(float multiplier = 1f)
    {
        CurrentAlertnessLevel -= Time.deltaTime;
    }

    public void DetectPlayer()
    {
        CurrentAlertnessState = AlertnessStates.High;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_SensorySystem : MonoBehaviour {

    //Struct that is send to the Decision System
    public struct SensoryInfo
    {
        public AlertnessStates _alertnessState;
        public int currentHealth;
        public int playerHealth;
        public bool isSeeingPlayer;
        public bool hasSeenPlayer;
        public bool hasHeardPlayer;
        public bool hearsGlobalAlert;
    }

    public enum AlertnessStates
    {
        Low,
        Medium,
        High
    }

    [Header("Settings")]
    public float MaxAlertness;
    public float TimeToMediumAlert;
    public float TimeToHighAlert;

    [Header("System")]
    public float UpdateRate;
    public AI_Sound AlertSound;

    //Sensory Info
    AlertnessStates CurrentAlertnessState;
    float CurrentAlertnessLevel;
    List<GameObject> VisibleObjects;
    List<GameObject> RealSounds;
    List<GameObject> PseudoSounds;

    //Flags that store info about important events and conditions
    bool hasSeenPlayer;
    bool hasHeardPlayer;
    bool isSeeingPlayer;
    bool isSeenByPlayer;
    bool hearsGlobalAlert;

    //Required objects and components
    GameObject _player;
    AI_DecisionSystem _decision;
    HealthController _hc;

    void OnBecameVisible()
    {
        isSeenByPlayer = true;
    }

	// Use this for initialization
	void Start () {
        _player = GameObject.Find("Player");
        _decision = GetComponent<AI_DecisionSystem>();
        _hc = GetComponent<HealthController>();

        StartCoroutine(SendInfo());
	}

    //Compile and send sensory info to the Decision System
    IEnumerator SendInfo()
    {
        while (true)
        {
            yield return new WaitForSeconds(UpdateRate);
            _decision.RecieveSensoryInfo(CompileSensoryInfo());
        }
    }

    // Update is called once per frame
    void Update () {
        if (isSeeingPlayer)                                                     //Increase alertness if is seeing player
        {
            IncreaseAlertness();

            if(CurrentAlertnessLevel > TimeToMediumAlert)                       
            {
                if(CurrentAlertnessLevel > TimeToHighAlert)
                {
                    if(CurrentAlertnessState != AlertnessStates.High)           //Move to high alert if alertness level is high enough
                        GotoHighAlert();
                }else
                {
                    if (CurrentAlertnessState != AlertnessStates.Medium)        //Move to medium alert if alertness level is high enough
                        GotoMediumAlert();
                }
            }
        }else
        {
            DecreaseAlertness(2f);                                              //Decrease alerntess if is seeing player
            if (CurrentAlertnessLevel < TimeToHighAlert)
            {
                if (CurrentAlertnessLevel < TimeToMediumAlert)
                {
                    if (CurrentAlertnessState != AlertnessStates.Low)           //Move to low alert if alertness level is low enough
                        GotoLowAlert();
                }
                else
                {
                    if (CurrentAlertnessState != AlertnessStates.Medium)        //Move to medium alert if alertness level is low enough
                        GotoMediumAlert();
                }
            }
        }
	}

    //This function is used by a Vision System to send vision info
    public void RecieveVision(List<GameObject> visibleObjects)
    {
        VisibleObjects = visibleObjects;
        if (VisibleObjects.Contains(_player))
        {
            isSeeingPlayer = true;
            hasSeenPlayer = true;
            if(CurrentAlertnessState == AlertnessStates.High)
            {
                CurrentAlertnessLevel = MaxAlertness;
            }
        }else
        {
            isSeeingPlayer = false;
        }
    }

    //Compiles all available info into one struct
    SensoryInfo CompileSensoryInfo()
    {
        SensoryInfo info = new SensoryInfo();

        info._alertnessState = CurrentAlertnessState;
        info.currentHealth = _hc.GetHealth();
        info.playerHealth = _player.GetComponent<HealthController>().GetHealth();
        info.isSeeingPlayer = isSeeingPlayer;
        info.hasSeenPlayer = hasSeenPlayer;
        info.hasHeardPlayer = hasHeardPlayer;
        info.hearsGlobalAlert = hearsGlobalAlert;
        return info;
    }

    //This function is used by a Hearing System to send audio info
    public void RecieveSounds(List<GameObject> real, List<GameObject> pseudo)
    {
        RealSounds = real;
        PseudoSounds = pseudo;
        CheckRecievedSounds();
    }
    
    //is used in RecieveSounds; checks owners of recieved sounds and triggers appropriate events
    void CheckRecievedSounds()
    {
        hearsGlobalAlert = false;
        foreach (GameObject s in RealSounds)
        {
            if (s.GetComponent<AI_Sound>().OwnerName == "Player")
            {
                hasHeardPlayer = true;
            }

            if (s.GetComponent<AI_Sound>().OwnerName == "Alert")
            {
                GotoHighAlert();
            }

            if (s.GetComponent<AI_Sound>().OwnerName == "Global Alert")
            {
                hearsGlobalAlert = true;
                GotoHighAlert();
            }
        }
    }

    //Increase alertness if it's not max already
    void IncreaseAlertness(float multiplier = 1f)
    {
        if(CurrentAlertnessLevel < MaxAlertness)
            CurrentAlertnessLevel += Time.deltaTime * multiplier;
    }

    //Decrease alertness if it's not 0 already
    void DecreaseAlertness(float multiplier = 1f)
    {
        if (CurrentAlertnessLevel > 0)
            CurrentAlertnessLevel -= Time.deltaTime * multiplier;
    }

    //Set alertness states
    public void GotoLowAlert()
    {
        if (CurrentAlertnessState != AlertnessStates.Low)
        {
            CurrentAlertnessState = AlertnessStates.Low;
        }

        CurrentAlertnessLevel = 0;
    }

    public void GotoMediumAlert()
    {
        if (CurrentAlertnessState != AlertnessStates.Medium)
        {
            CurrentAlertnessState = AlertnessStates.Medium;
        }

        CurrentAlertnessLevel = TimeToMediumAlert - 0.01f;
    }

    public void GotoHighAlert()
    {
        if(CurrentAlertnessState != AlertnessStates.High)
        {
            CurrentAlertnessState = AlertnessStates.High;
            Destroy(Instantiate(AlertSound, transform.position, transform.rotation), 1);
        }

        CurrentAlertnessLevel = MaxAlertness;
    }

    //returns alertness state
    public string GetCurrentAlertnessState()
    {
        switch (CurrentAlertnessState)
        {
            case AlertnessStates.High:
                return "High";
            case AlertnessStates.Medium:
                return "Medium";
            case AlertnessStates.Low:
                return "Low";
            default:
                return "BUG";
        }
    }

    //returns alertness level
    public float GetCurrentAlertnessLevel()
    {
        return CurrentAlertnessLevel;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_SensorySystem : MonoBehaviour {
    public struct SensoryInfo
    {
        public AlertnessStates _alertnessState;
        public int currentHealth;
        public int playerHealth;
        public List<Transform> PointsOfInterest;
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

    AlertnessStates CurrentAlertnessState;
    float CurrentAlertnessLevel;
    List<GameObject> VisibleObjects;
    List<GameObject> RealSounds;
    List<GameObject> PseudoSounds;
    List<Transform> pointsOfInterest = new List<Transform>();

    bool hasSeenPlayer;
    bool hasHeardPlayer;
    bool isSeeingPlayer;
    bool isSeenByPlayer;
    bool hearsGlobalAlert;

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
        if (isSeeingPlayer)
        {
            IncreaseAlertness();

            if(CurrentAlertnessLevel > TimeToMediumAlert)
            {
                if(CurrentAlertnessLevel > TimeToHighAlert)
                {
                    if(CurrentAlertnessState != AlertnessStates.High)
                        GotoHighAlert();
                }else
                {
                    if (CurrentAlertnessState != AlertnessStates.Medium)
                        GotoMediumAlert();
                }
            }
        }else
        {
            DecreaseAlertness(.5f);
            if (CurrentAlertnessLevel < TimeToHighAlert)
            {
                if (CurrentAlertnessLevel < TimeToMediumAlert)
                {
                    if (CurrentAlertnessState != AlertnessStates.Low)
                        GotoLowAlert();
                }
                else
                {
                    if (CurrentAlertnessState != AlertnessStates.Medium)
                        GotoMediumAlert();
                }
            }
        }
	}

    public void RecieveVision(List<GameObject> visibleObjects)
    {
        VisibleObjects = visibleObjects;
        if (VisibleObjects.Contains(_player))
        {
            isSeeingPlayer = true;
            hasSeenPlayer = true;
            pointsOfInterest.Add(VisibleObjects[VisibleObjects.IndexOf(_player)].transform);
        }else
        {
            isSeeingPlayer = false;
        }
    }

    SensoryInfo CompileSensoryInfo()
    {
        SensoryInfo info = new SensoryInfo();

        info._alertnessState = CurrentAlertnessState;
        info.currentHealth = _hc.GetHealth();
        info.playerHealth = _player.GetComponent<HealthController>().GetHealth();
        info.PointsOfInterest = pointsOfInterest;
        pointsOfInterest.Clear();
        info.isSeeingPlayer = isSeeingPlayer;
        info.hasSeenPlayer = hasSeenPlayer;
        info.hasHeardPlayer = hasHeardPlayer;
        info.hearsGlobalAlert = hearsGlobalAlert;
        return info;
    }

    public void RecieveSounds(List<GameObject> real, List<GameObject> pseudo)
    {
        RealSounds = real;
        PseudoSounds = pseudo;

        foreach(GameObject s in RealSounds)
        {
            if(s.GetComponent<AI_Sound>().OwnerName == "Player")
            {
                hasHeardPlayer = true;
                pointsOfInterest.Add(s.transform);
            }

            if(s.GetComponent<AI_Sound>().OwnerName == "Alert")
            {
                GotoHighAlert();
            }

            if(s.GetComponent<AI_Sound>().OwnerName == "Global Alert")
            {
                hearsGlobalAlert = true;
                GotoHighAlert();
            }
        }
    }
    
    void IncreaseAlertness(float multiplier = 1f)
    {
        if(CurrentAlertnessLevel < MaxAlertness)
            CurrentAlertnessLevel += Time.deltaTime * multiplier;
    }

    void DecreaseAlertness(float multiplier = 1f)
    {
        if (CurrentAlertnessLevel > 0)
            CurrentAlertnessLevel -= Time.deltaTime * multiplier;
    }

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

        CurrentAlertnessLevel = TimeToHighAlert - 0.01f;
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

    public void ForceAlertness(AlertnessStates state)
    {
        CurrentAlertnessState = state;
    }

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

    public float GetCurrentAlertnessLevel()
    {
        return CurrentAlertnessLevel;
    }
}

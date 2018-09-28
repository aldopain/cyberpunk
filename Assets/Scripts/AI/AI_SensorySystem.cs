using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_SensorySystem : MonoBehaviour {
    [System.Serializable]
    public struct PointOfInterest
    {
        public Vector3 position;
        public bool isVisual;           //determines if it's sound or not
        public AlertnessStates ThreatLevel;

        public PointOfInterest(Vector3 pos, bool _isVisual, AlertnessStates _threat)
        {
            position = pos;
            isVisual = _isVisual;
            ThreatLevel = _threat;
        }
    }

    public struct SensoryInfo
    {
        public AlertnessStates _alertnessState;
        public int currentHealth;
        public int playerHealth;
        public List<PointOfInterest> poi;
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
    List<PointOfInterest> pointsOfInterest = new List<PointOfInterest>();

    bool hasSeenPlayer;
    bool hasHeardPlayer;
    bool isSeeingPlayer;
    bool isSeenByPlayer;
    bool hearsGlobalAlert;

    //Reuired objects and components
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
            pointsOfInterest.Add(new PointOfInterest(VisibleObjects[VisibleObjects.IndexOf(_player)].transform.position, true, AlertnessStates.High));
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
        info.poi = pointsOfInterest;
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
                pointsOfInterest.Add(new PointOfInterest(s.transform.position, false, AlertnessStates.Low));
            }

            if(s.GetComponent<AI_Sound>().OwnerName == "Alert")
            {
                pointsOfInterest.Add(new PointOfInterest(s.transform.position, false, AlertnessStates.High));
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

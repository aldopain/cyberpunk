using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_SensorySystem : MonoBehaviour {
    public struct SensoryInfo
    {
        public AlertnessStates _alertnessState;
        public int currentHealth;
        public int playerHealth;
        public Vector3 movementTarget;
    }

    public enum AlertnessStates
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

    SensoryInfo CompileSensoryInfo()
    {
        SensoryInfo info = new SensoryInfo();

        info._alertnessState = CurrentAlertnessState;
        info.currentHealth = _hc.GetHealth();
        info.playerHealth = _player.GetComponent<HealthController>().GetHealth();

        switch (CurrentAlertnessState)
        {
            case AlertnessStates.Low:
                info.movementTarget = Vector3.zero;
                break;
            case AlertnessStates.Medium:
                info.movementTarget = Vector3.zero;
                break;
            case AlertnessStates.High:
                info.movementTarget = _player.transform.position;
                break;
        }

        return info;
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

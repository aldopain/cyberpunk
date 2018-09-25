using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Alarm : MonoBehaviour {
    public AI_Sound AlarmSound;

    GameObject _soundedAlarm;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Sound()
    {
        if(_soundedAlarm == null)
        {
            _soundedAlarm = Instantiate(AlarmSound.gameObject, transform.position, transform.rotation, transform);
        }
    }

    public void Silence()
    {
        Destroy(_soundedAlarm);
    }
}

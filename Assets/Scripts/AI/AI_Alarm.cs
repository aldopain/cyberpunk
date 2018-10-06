using UnityEngine;
using UnityEngine.Events;

public class AI_Alarm : MonoBehaviour {
    public AI_Sound AlarmSound;          //Alarm sound prefab, which determines the size of sound sphere
    public float InteractionDistance;    //Used for player's interactions with alarm
    public UnityEvent OnSound;
    public UnityEvent OnSilence;

    GameObject _soundedAlarm;       
    Collider[] OverlapColliders;        //Used for player's interactions with alarm
	
	//This is done so that player can turn them on and off
	void Update () {
        OverlapColliders = Physics.OverlapSphere(transform.position, InteractionDistance);

        if (OverlapColliders.Length != 0)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                foreach(Collider c in OverlapColliders)
                {
                    if(c.tag == "Player")
                    {
                        ToggleAlarm();
                    }
                }
            }
        }
	}

    //Interactions with alarm
    public void Sound()
    {
        if(_soundedAlarm == null)
        {
            _soundedAlarm = Instantiate(AlarmSound.gameObject, transform.position, transform.rotation, transform);
            OnSound.Invoke();
        }
    }

    public void Silence()
    {
        Destroy(_soundedAlarm);
        OnSilence.Invoke();
    }

    void ToggleAlarm()
    {
        if(_soundedAlarm == null)
        {
            Sound();
        }
        else
        {
            Silence();
        }
    }
}

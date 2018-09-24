using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Alarm : MonoBehaviour {
    public float Radius;
    public LayerMask DetectionMask;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Sound()
    {
        Collider[] tmp = Physics.OverlapSphere(transform.position, Radius, DetectionMask);
        foreach(Collider c in tmp)
        {
            if (c.GetComponent<AI_SensorySystem>() != null)
            {
                c.GetComponent<AI_SensorySystem>().ForceAlertness(AI_SensorySystem.AlertnessStates.High);
            }
        }
    }
}

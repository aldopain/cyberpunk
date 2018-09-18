using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugOnHit : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    public void Hit(string log)
    {
        Debug.Log(log);
    }	
	// Update is called once per frame
	void Update () {
		
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class AI_Sound : MonoBehaviour {
    public string OwnerName;
    public enum SoundTypes
    {
        Real,
        Pseudo
    }

    public SoundTypes Type;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

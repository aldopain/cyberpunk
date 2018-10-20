using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IK_Head : MonoBehaviour {

	public Transform target;
	Animator anim;

	public float lookIKWeight;
	public float eyesWeight;
	public float headWeight;
	public float bodyWeight;
	public float clampWeight;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
	}

	void OnAnimatorIK () {
        anim.SetLookAtWeight (lookIKWeight, bodyWeight, headWeight, eyesWeight, clampWeight);
		anim.SetLookAtPosition(target.position);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

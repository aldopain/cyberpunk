using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IK_Hands : MonoBehaviour {

	Animator anim;

	public Transform objToPickUp;

	public float rightHandWeight;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnAnimatorIK () {
        anim.SetIKPositionWeight(AvatarIKGoal.RightHand, rightHandWeight);
        anim.SetIKPosition(AvatarIKGoal.RightHand, objToPickUp.position);
	}
}

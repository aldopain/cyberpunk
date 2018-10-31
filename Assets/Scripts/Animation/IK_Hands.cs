using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IK_Hands : MonoBehaviour {

	Animator anim;

	public Transform rb;
	public Transform lb;

	public float rightHandWeight;
	public float leftHandWeight;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnAnimatorIK () {
        anim.SetIKPositionWeight(AvatarIKGoal.RightHand, rightHandWeight);
        anim.SetIKPosition(AvatarIKGoal.RightHand, rb.position);
		
        anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, leftHandWeight);
        anim.SetIKPosition(AvatarIKGoal.LeftHand, lb.position);
	}
}

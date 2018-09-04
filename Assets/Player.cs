using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
	public float speed = 1.0f;
	CharacterController cc;

	// Use this for initialization
	void Start () {
		cc = GetComponent<CharacterController>();
	}

	void Move () {
		var move = new Vector3(Input.GetAxis("Horizontal"), -1000f, Input.GetAxis("Vertical"));
		cc.Move(move * Time.deltaTime * speed);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		Move();
	}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
	public float speed = 1.0f;
	CharacterController cc;
	Transform transform;
	float originalRotation = -180;

	// Use this for initialization
	void Start () {
		cc = GetComponent<CharacterController>();
		transform = GetComponent<Transform>();
	}

	void Move () {
		var h = Input.GetAxis("Horizontal");
		var v = Input.GetAxis("Vertical");
		var move = new Vector3(h, -1000f, v);
		float r = 0f;
		if (v == 0) {
			Debug.Log("h = " + h);
			r = 180 * (h + 1);
			Debug.Log("r = " + r);
		} else if (h == 0)
			r = 90 * v;
		else
			r = v * (90 + 45 * h);
		transform.rotation = Quaternion.Euler(0, r, 0);
		cc.Move(move * Time.deltaTime * speed);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		Move();
	}
}
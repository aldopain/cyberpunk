using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {
	public float speed = 1.0f;
	public float lookSpeed = 10;
	private Vector3 curLoc;
	private Vector3 prevLoc;
	CharacterController cc;
	Transform target;
	float originalRotation = -180;

	// Use this for initialization
	void Start () {
		cc = GetComponent<CharacterController>();
		target = GameObject.Find("TargetForRotation").transform;
	}

	void Move () {
		var h = Input.GetAxisRaw("Horizontal");
		var v = Input.GetAxisRaw("Vertical");

		if (h != 0 || v != 0) {
			Vector3 direction = new Vector3(h, 0f, v);
	
			direction = Camera.main.transform.TransformDirection(direction);

			var buf = transform.position + direction;
			buf.y = transform.position.y;
			target.position = buf;
			
			direction.y = -1000f;
			transform.LookAt(target, Vector3.up);
			transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + 90, transform.eulerAngles.z);
			cc.Move(direction * Time.deltaTime * speed);
		}
	}
	
	// Update is called once per frame
	void Update () {
		Move();
	}
}
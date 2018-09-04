using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
	public float speed = 1.0f;

	// Use this for initialization
	void Start () {
		
	}

	void Move(){
		var x = Input.GetAxis("Horizontal") * Time.deltaTime * speed;
        var z = Input.GetAxis("Vertical") * Time.deltaTime * speed;

        transform.Translate(x, 0, z);
	}
	
	// Update is called once per frame
	void Update () {
		Move();
	}
}

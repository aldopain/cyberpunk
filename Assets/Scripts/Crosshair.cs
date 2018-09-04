using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour {
    Vector2 _mousePos;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        _mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = _mousePos;

    }
}

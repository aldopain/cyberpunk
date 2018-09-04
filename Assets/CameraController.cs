using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
	GameObject player;
	Vector3 diff;

    // Use this for initialization
    void Start () {
		player = GameObject.Find("Player");
		diff = transform.position - player.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = player.transform.position + diff;
	}
}

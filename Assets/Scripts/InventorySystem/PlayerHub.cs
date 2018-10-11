using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHub : MonoBehaviour {
    private GameObject _panel;
    private bool isActive = false;
    private float _baseTimeScale;

	// Use this for initialization
	void Start () {
        _panel = GameObject.Find ("Inventory");
        _panel.SetActive (false);
        _baseTimeScale = Time.timeScale;
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.I)) {
            Time.timeScale = isActive ? _baseTimeScale : 0f;
            isActive = !isActive;
            _panel.SetActive (isActive);
        }
	}
}

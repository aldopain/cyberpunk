using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour {
    Vector2 _mousePos;
    public LineRenderer line;
    public LayerMask RaycastMask;
    // Use this for initialization
    void Start () {
        line.positionCount = 2;
    }
	
	// Update is called once per frame
	void Update () {
        //_mousePos = Input.mousePosition;
        //transform.position = _mousePos;
    }
}

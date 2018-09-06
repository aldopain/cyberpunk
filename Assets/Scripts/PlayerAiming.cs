using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAiming : MonoBehaviour {
    public LayerMask mask;
    public Crosshair _crosshair;
    public LineRenderer _line;
    public float AimingDistance;

	// Use this for initialization
	void Start () {
        _line.SetPosition(0, Vector3.zero);
	}
    
    // This converts mouse position to world coordinates
    Vector3 mousePositionToWorld(){
        RaycastHit mouse;
        Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out mouse);
        return mouse.point;
    }
	
	// Update is called once per frame
	void Update () {
        // Set the start of the line at the player position
        _line.SetPosition(0, new Vector3(transform.position.x, transform.position.y + GetComponent<CharacterController>().bounds.extents.y, transform.position.z));

        RaycastHit hit;

        Ray ray = new Ray(transform.position, mousePositionToWorld() - transform.position);
        
        Physics.Raycast(ray, out hit);

        _crosshair.transform.position = hit.point;
        
        _line.SetPosition(1, _crosshair.transform.position);
    }
}

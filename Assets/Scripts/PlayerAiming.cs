using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAiming : MonoBehaviour {
    public LayerMask mask;
    public Crosshair _crosshair;
    public LineRenderer _line;
    public float AimingDistance;

    Vector3 hui = new Vector3(1,0,0);

	// Use this for initialization
	void Start () {
        _line.SetPosition(0, Vector3.zero);
	}
    
    // This converts mouse position to world coordinates
    Vector3 mousePositionToWorld(){
        RaycastHit mouse;
        Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition+hui), out mouse);
        
        return mouse.point;

    }
	
	// Update is called once per frame
	void Update () {
        // Set the start of the line at the player position
        _line.SetPosition(0, new Vector3(transform.position.x, transform.position.y + GetComponent<CharacterController>().bounds.extents.y, transform.position.z));

        
        RaycastHit hit;

        //Ray ray = new Ray(transform.position, mousePositionToWorld() - transform.position);
        Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition + hui), out hit);
        //Physics.Raycast(ray, out hit);
        Vector3 vec = Camera.main.ScreenToWorldPoint(Input.mousePosition) - hit.point;
        Debug.Log(vec.normalized);
        Debug.DrawRay(hit.point, Camera.main.ScreenToWorldPoint(Input.mousePosition), Color.green);
        vec = hit.point+ vec.normalized;
        _crosshair.transform.position = vec;//hit.point;
       
        _line.SetPosition(1, _crosshair.transform.position);
    }
}

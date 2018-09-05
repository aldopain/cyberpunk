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
	
	// Update is called once per frame
	void Update () {
        //Set the start of the line at the player position
        _line.SetPosition(0, new Vector3(transform.position.x, transform.position.y + GetComponent<CharacterController>().bounds.extents.y, transform.position.z));

        //Edit the mouseInput and position vectors, so they start in the middle of the GameObject
        Vector3 mouseVector = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, transform.position.y + GetComponent<CharacterController>().bounds.extents.y, Camera.main.ScreenToWorldPoint(Input.mousePosition).z);
        Vector3 positionVector = new Vector3(transform.position.x, transform.position.y + GetComponent<CharacterController>().bounds.extents.y, transform.position.z);

        //Find the point at the edge of the circle with the AimingDistance radius that corresponds with an angle between mouse position and player position
        float mousePosAngle = Mathf.Atan2(mouseVector.z - transform.position.z, mouseVector.x - transform.position.x);
        Vector3 crosshairPos = Vector3.zero;
        crosshairPos.x = AimingDistance * Mathf.Cos(mousePosAngle);
        crosshairPos.z = AimingDistance * Mathf.Sin(mousePosAngle);

        //Fire the ray at the point on the edge of the circle
        RaycastHit hit;
        Ray ray = new Ray(transform.position, mouseVector - positionVector);
        Physics.Raycast(ray, out hit, AimingDistance, mask);
        
        //If ray hits something, draw crosshair at hit point
        //Else, draw crosshair at the point on the edge of the circle
        if(hit.collider != null)
        {
            _crosshair.transform.position = hit.point;
        }
        else
        {
            _crosshair.transform.position = transform.position + crosshairPos;
        }
        
        //Set the end of the line at the crosshair position
        _line.SetPosition(1, _crosshair.transform.position);
    }
}

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
        _line.SetPosition(0, new Vector3(transform.position.x, transform.position.y + GetComponent<CharacterController>().bounds.extents.y, transform.position.z));

        Vector3 mouseVector = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, transform.position.y + GetComponent<CharacterController>().bounds.extents.y, Camera.main.ScreenToWorldPoint(Input.mousePosition).z);
        Vector3 positionVector = new Vector3(transform.position.x, transform.position.y + GetComponent<CharacterController>().bounds.extents.y, transform.position.z);

        float mousePosAngle = Mathf.Atan2(mouseVector.z - transform.position.z, mouseVector.x - transform.position.x);
        Vector3 crosshairPos = Vector3.zero;
        crosshairPos.x = AimingDistance * Mathf.Cos(mousePosAngle);
        crosshairPos.z = AimingDistance * Mathf.Sin(mousePosAngle);

        RaycastHit hit;
        Ray ray = new Ray(transform.position, mouseVector - positionVector);
        Physics.Raycast(ray, out hit, AimingDistance, mask);
        print(hit.collider != null);
        if(hit.collider != null)
        {
            _crosshair.transform.position = hit.point;
        }
        else
        {
            _crosshair.transform.position = transform.position + crosshairPos;
        }



        _line.SetPosition(1, _crosshair.transform.position);
    }
}

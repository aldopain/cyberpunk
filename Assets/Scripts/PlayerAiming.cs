using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAiming : MonoBehaviour {
    public LayerMask mask;
    public Crosshair _crosshair;
    public LineRenderer _line;
    public bool useRadialAiming;
    public float AimingDistance;
    public float AboveGroundCrosshairHeight = 1f;
    public Vector3 heightOffset;
    private Vector3 target;

    private float RotationAngle;

	// Use this for initialization
	void Start () {
        _line.transform.position = Vector3.zero;
        _line.SetPosition(0, Vector3.zero);
        heightOffset = new Vector3 (0.989949f, 1f, 1.02813133f);
        //heightOffset = new Vector3 (1.427f, 1f, 0.027f);
	}
    
    // This converts mouse position to world coordinates
    // Vector3 mousePositionToWorld(){
    //     RaycastHit mouse;
    //     if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out mouse))
    //         return mouse.point;
    //     else
    //         return Vector3.zero;
    // }

    Vector3 GetPositionOnCircle(Vector3 pos)
    {
        Vector3 targetPos;

        targetPos.x = transform.position.x + AimingDistance * Mathf.Cos(RotationAngle);
        targetPos.z = transform.position.z + AimingDistance * Mathf.Sin(RotationAngle);
        targetPos.y = 0;

        return targetPos;
    }

    public float GetAngle_Rad()
    {
        return RotationAngle;
    }

    public float GetAngle_Deg()
    {
        return RotationAngle * Mathf.Rad2Deg;
    }

    public Vector3 GetCrosshairPosition()
    {
        return _crosshair.transform.position;
    }

	// Update is called once per frame
	void Update () {
        // Set the start of the line at the player position
        _line.SetPosition(0, new Vector3(transform.position.x, transform.position.y + GetComponent<CharacterController>().bounds.extents.y, transform.position.z));

        _crosshair.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        var diff = _crosshair.transform.position.y - (transform.position.y + GetComponent<CharacterController>().bounds.extents.y);

        target = _crosshair.transform.position - diff * heightOffset;

        _line.SetPosition(1, target);
    }
}

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

    private float RotationAngle;

	// Use this for initialization
	void Start () {
        _line.transform.position = Vector3.zero;
        _line.SetPosition(0, Vector3.zero);
	}
    
    // This converts mouse position to world coordinates
    Vector3 mousePositionToWorld(){
        RaycastHit mouse;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out mouse))
            return mouse.point;
        else
            return Vector3.zero;
    }

    Vector3 GetPositionOnCircle(Vector3 pos)
    {
        Vector3 crosshairPos;

        crosshairPos.x = transform.position.x + AimingDistance * Mathf.Cos(RotationAngle);
        crosshairPos.z = transform.position.z + AimingDistance * Mathf.Sin(RotationAngle);
        crosshairPos.y = 0;

        return crosshairPos;
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

        print("transform.position: " + transform.position.y + "; extents " + GetComponent<CharacterController>().bounds.extents.y);

        Vector3 hit = mousePositionToWorld();
        
        //if mousePositionToWorld() returned not a Vector3.zero, then calculate crosshair.position
        //else do nothing
        if (!hit.Equals(Vector3.zero)) {
            Vector3 vec = Camera.main.ScreenToWorldPoint(Input.mousePosition) - hit;
            vec = hit + vec.normalized * AboveGroundCrosshairHeight;

            RotationAngle = Mathf.Atan2(vec.y - transform.position.y, vec.x - transform.position.x);

            if (useRadialAiming)
            {
                _crosshair.transform.position = GetPositionOnCircle(vec);
            }
            else
            {
                _crosshair.transform.position = vec;
            }

            _line.SetPosition(1, _crosshair.transform.position);
        }
    }
}

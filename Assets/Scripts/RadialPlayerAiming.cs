using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadialPlayerAiming : MonoBehaviour
{
    public LayerMask mask;
    public Crosshair _crosshair;
    public LineRenderer _line;
    public float AimingDistance;
    public float AimingSpeed;

    private float RotationAngle;
    // Use this for initialization
    void Start()
    {
        _line.SetPosition(0, Vector3.zero);
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Set the start of the line at the player position
        _line.SetPosition(0, new Vector3(transform.position.x, transform.position.y + GetComponent<CharacterController>().bounds.extents.y, transform.position.z));

        RotationAngle = Mathf.Atan2(Input.mousePosition.y - Camera.main.WorldToScreenPoint(transform.position).y, Input.mousePosition.x - Camera.main.WorldToScreenPoint(transform.position).x) + (Mathf.Deg2Rad * 90);

        RotationAngle *= AimingSpeed;
        Vector3 crosshairPos;
        crosshairPos.x = transform.position.x + AimingDistance * Mathf.Cos(RotationAngle);
        crosshairPos.z = transform.position.z + AimingDistance * Mathf.Sin(RotationAngle);
        crosshairPos.y = 0;
        //_crosshair.transform.position = crosshairPos;

        Vector3 positionVector = new Vector3(transform.position.x, transform.position.y + GetComponent<CharacterController>().bounds.extents.y, transform.position.z);

        RaycastHit hit;
        Ray ray = new Ray(positionVector, crosshairPos - positionVector);
        Physics.Raycast(ray, out hit, AimingDistance, mask);

        //If ray hits something, draw crosshair at hit point
        //Else, draw crosshair at the point on the edge of the circle
        if (hit.collider != null)
        {
            _crosshair.transform.position = hit.point;
        }else{
            _crosshair.transform.position = crosshairPos;
        }




    //Set the end of the line at the crosshair position
    _line.SetPosition(1, _crosshair.transform.position);
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
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAiming : MonoBehaviour {
    public LayerMask mask;
    public Crosshair _crosshair;
    public Crosshair _mouseCrosshair;
    public LineRenderer _line;
    public bool useRadialAiming;
    public float AimingDistance;
    public Vector3 heightOffset;
    private Vector3 target;

    public float transparencyMultiplier;
    private float RotationAngle;

    public GameObject _gun;

	// Use this for initialization
	void Start () {
        _line.transform.position = Vector3.zero;
        _line.SetPosition(0, Vector3.zero);
        heightOffset = new Vector3 (0.989949f, 1f, 1.02813133f);
	}

    public Vector3 GetTargetPosition() { 
        return target; 
    }

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

    void SetupMouseCrosshair(Vector3 a, Vector3 b)
    {
        _mouseCrosshair.transform.position = target;
        _mouseCrosshair.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, Mathf.Pow(Vector3.Distance(a, b), 2) * transparencyMultiplier);
    }

	// Update is called once per frame
	void FixedUpdate () {
        // Set the start of the line at the player position
        var height = transform.position.y + GetComponent<CharacterController>().bounds.extents.y;
        var GunPosition = _gun.transform.position;

        _line.SetPosition(0, GunPosition);

        var mousePositionInWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        var diff = mousePositionInWorld.y - height;

        target = mousePositionInWorld - diff * heightOffset;

        RotationAngle = Mathf.Atan2(target.y - transform.position.y, target.x - transform.position.x); 

        RaycastHit rh;
        Ray ray = new Ray(GunPosition, target - GunPosition);


        if(Vector3.Distance(mousePositionInWorld, transform.position) < AimingDistance)
        {
            if (Physics.Raycast(ray, out rh, AimingDistance, mask))
            {
                if(Vector3.Distance(mousePositionInWorld, transform.position) < Vector3.Distance(rh.point, transform.position))
                {
                    _crosshair.transform.position = target;

                    _mouseCrosshair.GetComponent<SpriteRenderer>().color = new Color(1,1,1,0);
                }
                else
                {
                    _crosshair.transform.position = rh.point;

                    SetupMouseCrosshair(mousePositionInWorld, rh.point);
                }
            }else
            {
                _crosshair.transform.position = target;

                _mouseCrosshair.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
            }
        }
        else
        {
            if (Physics.Raycast(ray, out rh, AimingDistance))
            {
                _crosshair.transform.position = rh.point;

                SetupMouseCrosshair(mousePositionInWorld, rh.point);
            }
            else
            {
                _crosshair.transform.position = GunPosition + AimingDistance * (target - GunPosition).normalized;

                SetupMouseCrosshair(mousePositionInWorld, _crosshair.transform.position);
            }
        }

        _line.SetPosition(1, _crosshair.transform.position);
    }
}

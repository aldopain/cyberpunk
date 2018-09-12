using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericGun : MonoBehaviour {
    [Header("Gun Settings")]
    public Bullet BulletPrefab;
    public float ShootingDelay;
    
    [Header("Orbit Settings")]
    public Vector3 OrbitCenterOffset;
    public float OrbitingRadius;

    //With this enabled, this gun won't be able to rotate by itself (thus, eliminating the need for a PlayerAiming to operate), and instead would be able to recieve rotation info from other Components
    public bool RecieveRotation;
    public bool RecieveShootingInputs;

    //Private variables
    private PlayerAiming _aim;
    private Transform _startingTransform;
    float TimeSinceShot;

	// Use this for initialization
	void Start () {
        _aim = GameObject.Find("Player").GetComponent<PlayerAiming>();
        _startingTransform = transform;
	}

    void UpdateRotation()
    {
        Vector3 position;
        //print(_aim.GetAngle_Rad() + "|" + _aim.GetAngle_Deg());
        position.x = (_aim.transform.position.x + OrbitCenterOffset.x) + OrbitingRadius * Mathf.Cos(_aim.GetAngle_Rad());
        position.z = (_aim.transform.position.z + OrbitCenterOffset.z) + OrbitingRadius * Mathf.Sin(_aim.GetAngle_Rad());
        position.y = _aim.transform.position.y + _aim.GetComponent<CharacterController>().bounds.extents.y + OrbitCenterOffset.y;

        transform.position = position;

        // -90 fixes non-matching coordiantion systems of Blender and Unity. 
        //transform.rotation = Quaternion.Euler(-90, -_aim.GetAngle_Deg(), -90);
        transform.LookAt(_aim._crosshair.transform);
        //transform.rotation = Quaternion.Euler(90, transform.rotation.y, 90);
    }

    public void Shoot()
    {
        if(TimeSinceShot >= ShootingDelay)
        {
            GameObject bullet = Instantiate(BulletPrefab.gameObject, transform.position, transform.rotation);
            bullet.GetComponent<Bullet>().Shoot();
            TimeSinceShot = 0;
        }
    }

	// Update is called once per frame
	void Update () {
        TimeSinceShot += Time.deltaTime;

        if (!RecieveRotation)
        {
            UpdateRotation();
        }

        if (RecieveShootingInputs)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Shoot();
            }
        }
	}
}

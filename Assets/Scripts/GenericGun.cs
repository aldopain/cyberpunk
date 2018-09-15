using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericGun : MonoBehaviour {
    [Header("Gun Settings")]
    public Bullet BulletPrefab;
    public float ShootingDelay;
    
    [Header("Orbit Settings")]
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
        Vector3 player = new Vector3 (_aim.transform.position.x, _aim.transform.position.y + _aim.GetComponent<CharacterController>().bounds.extents.y, _aim.transform.position.z);

        Vector3 target = _aim.GetComponent<PlayerAiming>().GetTargetPosition();

        transform.position = player + OrbitingRadius * (target - player).normalized;

        transform.LookAt(_aim._crosshair.transform);
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

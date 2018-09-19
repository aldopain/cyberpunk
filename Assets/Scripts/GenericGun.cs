using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericGun : MonoBehaviour {
    public enum FireMode
    {
        FullAuto,
        Single,
        Burst
    }

    [Header("Gun Settings")]
    public Bullet BulletPrefab;
    public float ShootingDelay;
    public int MaxMagazineCapacity;
    public float ReloadTime;
    public FireMode CurrentFireMode;
    public int BurstShotsCount;
    public int BulletsPerShot;

    //Fire modes
    public bool canAutoFire;
    public bool canSingleFire;
    public bool canBurstFire;

    [Header("Orbit Settings")]
    public Vector3 OrbitCenterOffset;
    public float OrbitingRadius;

    //With this enabled, this gun won't be able to rotate by itself (thus, eliminating the need for a PlayerAiming to operate), and instead would be able to recieve rotation info from other Components
    public bool RecieveRotation;
    public bool RecieveShootingInputs;


    //Private variables
    private PlayerAiming _aim;
    private Transform _startingTransform;
    private bool isReloading;
    private int CurrentMagazineCapacity;
    float TimeSinceShot;
    int consecutiveShotsFired;
    bool[] fireModeMask;


	// Use this for initialization
	void Start () {
        CurrentMagazineCapacity = MaxMagazineCapacity;
        _aim = GameObject.Find("Player").GetComponent<PlayerAiming>();
        _startingTransform = transform;

        SetupFireModeMask();
	}

    void SetupFireModeMask()
    {
        int FIRE_MODE_LENGTH = 3;
        fireModeMask = new bool[FIRE_MODE_LENGTH];

        if(!(canAutoFire || canSingleFire || canBurstFire))
        {
            canSingleFire = true;
        }

        fireModeMask[0] = canAutoFire;
        fireModeMask[1] = canSingleFire;
        fireModeMask[2] = canBurstFire;

        //Setting the default firemode
        if (!fireModeMask[(int)CurrentFireMode])
        {
            FiremodeSwitch();
        }
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
        if (!isReloading && CurrentMagazineCapacity > 0) {
            if(consecutiveShotsFired < BurstShotsCount || !(CurrentFireMode == FireMode.Burst))
            {
                if (TimeSinceShot >= ShootingDelay)
                {
                    CurrentMagazineCapacity--;
                    TimeSinceShot = 0;
                    consecutiveShotsFired++;
                    for (int i = 0; i < BulletsPerShot; i++) {
                        GameObject bullet = Instantiate(BulletPrefab.gameObject, transform.position, transform.rotation);
                        bullet.GetComponent<Bullet>().Shoot();
                    }
                }
            }
        } else if (!isReloading) {
            StartCoroutine(Reload());
        } else {
            //add here some animation to show player that he is still reloading
        }
    }

    public float GetRange () {
        return BulletPrefab != null ? BulletPrefab.Range : 0;
    }

    IEnumerator Reload () {
        isReloading = true;
        yield return new WaitForSeconds(ReloadTime);
        CurrentMagazineCapacity = MaxMagazineCapacity; //fix when add inventory
        isReloading = false;
        consecutiveShotsFired = 0;
    }

    void FiremodeSwitch()
    {
        bool flag = false;
        int curMode = (int)CurrentFireMode;

        while(flag == false)
        {
            if (curMode == System.Enum.GetNames(typeof(FireMode)).Length - 1)
            {
                curMode = 0;
            }
            else
            {
                curMode++;
            }

            if (fireModeMask[curMode])
            {
                flag = true;
                CurrentFireMode = (FireMode)curMode;
            }
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

            //Shooting
            if (CurrentFireMode == FireMode.Single)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Shoot();
                }
            }else
            {
                if (Input.GetMouseButton(0))
                {
                    Shoot();
                }
            }

            //Other interactions
            if (Input.GetKeyDown(KeyCode.R))
            {
                StartCoroutine(Reload());
            }

            if (Input.GetKeyDown(KeyCode.V))
            {
                FiremodeSwitch();
            }

            if (Input.GetMouseButtonUp(0))
            {
                consecutiveShotsFired = 0;
            }
        }
	}
}

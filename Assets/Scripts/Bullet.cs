using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class Bullet : MonoBehaviour {
    public float ShootingVelocity;
    public int Damage;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Shoot()
    {
        GetComponent<Rigidbody>().velocity = ShootingVelocity * transform.forward;
    }

    void OnTriggerEnter(Collider other)
    {
        print("Collided With " + other.name);
        if(other.GetComponent<HealthController>() != null)
        {
            print(other.name + " has a health");
            other.GetComponent<HealthController>().ChangeHealth(-Mathf.Abs(Damage));
            Destroy(gameObject);
        }
    }
}

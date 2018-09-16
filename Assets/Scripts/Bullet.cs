using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class Bullet : MonoBehaviour {
    public float ShootingVelocity;
    public int Damage;
    public float Range;
    public string[] IgnoredTags;

    private Vector3 StartPosition;
	// Use this for initialization
	void Start () {
		StartPosition = transform.position;
        print (StartPosition);
	}
	
	// Update is called once per frame
	void Update () {
		if (Vector3.Distance(transform.position, StartPosition) >= Range) {
            print (transform.position);
            Destroy (this.gameObject);
        }
	}

    public void Shoot()
    {
        GetComponent<Rigidbody>().velocity = ShootingVelocity * transform.forward;
    }

    bool isIgnoredTag(string s)
    {
        foreach(string tag in IgnoredTags)
        {
            if(s == tag)
            {
                return true;
            }
        }
        return false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!isIgnoredTag(other.tag))
        {
            if (other.GetComponent<HealthController>() != null)
            {
                other.GetComponent<HealthController>().ChangeHealth(-Mathf.Abs(Damage));
            }

            Destroy(gameObject);
        }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class Bullet : MonoBehaviour {
    public float ShootingVelocity;
    public int Damage;
    public float Range;
    public float AngleDeflectionAbs;
    public string[] IgnoredTags;
    public float ParalizeTime;

	// Use this for initialization
	void Start () {
        Destroy (this.gameObject, Range / ShootingVelocity);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Shoot()
    {
        transform.Rotate (new Vector3 (0, Random.Range(-AngleDeflectionAbs, AngleDeflectionAbs), 0));
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

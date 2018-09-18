using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class Bullet : MonoBehaviour {
    public float ShootingVelocity;
    public int Damage;
    public float Range;
    public float Accuracy;
    public string[] IgnoredTags;

    private Vector3 StartPosition;

    [HideInInspector]
    public Vector3 Shift;
	// Use this for initialization
	void Start () {
		StartPosition = transform.position;
        Destroy (this.gameObject, Range / ShootingVelocity);
        foreach (GenericMutator mutator in GetComponents<GenericMutator>())
        {
            mutator.onStart();
        }
    }

    // Update is called once per frame
    //TODO!: При создании сделать список на апдейт что бы не перебирать всегда все мутаторы
    void Update () {
        foreach (GenericMutator mutator in GetComponents<GenericMutator>())
        {
            mutator.onUpdate();
        }
    }

    

    public void Shoot()
    {
        foreach (GenericMutator mutator in GetComponents<GenericMutator>())
        {
            mutator.onShot();
        }
        ShootFly();//Тут говно код, потому что зависает почему то..


    }

    public void ShootFly()
    {
        GetComponent<Rigidbody>().velocity = ShootingVelocity * (transform.forward + Shift);
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
            foreach (GenericMutator mutator in GetComponents<GenericMutator>())
            {
                mutator.onHit(other);
            }
            Destroy(gameObject); 
            //float Spread = 0.1f;
            //GetComponent<Rigidbody>().velocity = new Vector3(Random.Range(-Spread, Spread), Random.Range(-Spread, Spread), Random.Range(-Spread, Spread));
        }

    }
}

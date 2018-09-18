using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSlowingMutator : GenericMutator{

    public float Slowing;

    public override void onUpdate()
    {
        GetComponent<Rigidbody>().velocity = Vector3.Scale(GetComponent<Rigidbody>().velocity,new Vector3(Slowing, Slowing, Slowing));
    }
}

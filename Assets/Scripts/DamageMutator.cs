using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageMutator : GenericMutator{

    public int hitDamage;


    public override void onHit(Collider other)
    {
        base.onHit(other);
        if (other.GetComponent<HealthController>() != null)
        {
            other.GetComponent<HealthController>().ChangeHealth(-hitDamage);
            Debug.Log(other.GetComponent<HealthController>().GetHealth());
        }

    }
}

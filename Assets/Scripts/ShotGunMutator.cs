using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotGunMutator : GenericMutator
{

    public int AdditionalPellets;
    public float Spread;

    public override void onStart() {
        base.onStart();
    }
    public override void onShot()
    {
        for (int i = 0; i < AdditionalPellets; i++)
        {
            Bullet pellet = Instantiate(GetComponent<Bullet>());
            Destroy(pellet.GetComponent<ShotGunMutator>());
            Vector3 shift = new Vector3(Random.Range(-Spread, Spread), Random.Range(-Spread, Spread), Random.Range(-Spread, Spread));
            pellet.Shift = shift;
            pellet.ShootFly();
        }
    }
}

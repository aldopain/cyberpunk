using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GenericMutator : MonoBehaviour
{

    protected Bullet parent;
    public virtual void onShot() { }
    public virtual void onStart() { }
    public virtual void onUpdate() { }
    public virtual void onHit(Collider other) { }
}

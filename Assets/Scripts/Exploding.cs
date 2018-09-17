using UnityEngine;

public class Exploding : MonoBehaviour {
    public Vector3 CenterOffset;
    public float Radius;
    public int Damage;
    public float ExplosionTimeout;

    private void Start() {
        if (ExplosionTimeout > 0) {
            Destroy (gameObject, ExplosionTimeout);
        }
    }

    public void Explode () {
        var Center = transform.position + CenterOffset;
        Collider[] hitColliders = Physics.OverlapSphere (Center, Radius);
        foreach (Collider current in hitColliders) {
            Ray ray = new Ray (Center, current.transform.position - Center);
            RaycastHit rh;

            if (Physics.Raycast (ray, out rh, Radius) && 
                current.GetComponent<HealthController>() != null &&
                rh.collider.Equals (current)) {
                    current.GetComponent<HealthController>().ChangeHealth(-Mathf.Abs(Damage));
            }
        }
    }

    private void OnDestroy() {
        if (enabled)
        {
            Explode();
        }      
    }
}
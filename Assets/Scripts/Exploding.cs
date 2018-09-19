using UnityEngine;
using System.Collections.Generic;
using System.Linq;

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

    //Sorts colliders by their distance to center of explosion
    private List<KeyValuePair<Collider, float>> SortByDistance (Collider[] _hit) {
        Dictionary<Collider, float> _dict = new Dictionary<Collider, float>();
        foreach (Collider _current in _hit) {
            _dict.Add(_current, Vector3.Distance (_current.transform.position, transform.position));
        }
        var _sorted = from entry in _dict orderby entry.Value ascending select entry;
        return _sorted.ToList();
    }

    public void Explode () {
        var Center = transform.position + CenterOffset;
        Collider[] hitColliders = Physics.OverlapSphere (Center, Radius);
        var CollidersAndLayers = new List<KeyValuePair<Collider, int>>();

        foreach (KeyValuePair<Collider, float> _currentPair in SortByDistance(hitColliders)) {
            Collider current = _currentPair.Key;
            Ray ray = new Ray (Center, current.transform.position - Center);
            RaycastHit rh;

            //if we can raycast in collider and it has health controller
            //then we change health, add pair Collider, Layer in list
            //and set this collider's object's layer to 2: IgnoreRaycast
            if (Physics.Raycast (ray, out rh, Radius) && 
                current.GetComponent<HealthController>() != null &&
                rh.collider.Equals (current)) {
                    current.GetComponent<HealthController>().ChangeHealth(-Mathf.Abs(Damage));
                    CollidersAndLayers.Add (new KeyValuePair<Collider, int>(current, current.gameObject.layer));
                    current.gameObject.layer = 2;
                    Debug.Log (current.gameObject.layer);
                    Debug.DrawRay(ray.origin, rh.point - transform.position, Color.yellow, Mathf.Infinity);
            }
        }

        //returning layers back
        foreach (var _currentPair in CollidersAndLayers){
            _currentPair.Key.gameObject.layer = _currentPair.Value;
            Debug.Log (_currentPair.Key.gameObject.layer);
        }
    }

    private void OnDestroy() {
        Explode();
    }
}
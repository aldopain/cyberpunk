using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_VisionSystem : MonoBehaviour {
    [Header("Settings")]
    public LayerMask EnvironmentLayer;
    public bool isBlind;

    [Header("System")]
    public AI_SensorySystem SensorySystem;

    List<GameObject> ObjectsInCone = new List<GameObject>();
    List<GameObject> ObjectsInVision = new List<GameObject>();

    void Start() {
        StartCoroutine(SendVision());
    }

    IEnumerator SendVision() {
        while (true)
        {
            yield return new WaitForSeconds(SensorySystem.UpdateRate);
            SensorySystem.RecieveVision(ObjectsInVision);
        }
    }

    //Returns true if other transform with layer Environment can be hit with a ray
    bool DetectVisibility (Transform other) {
        return DetectVisibility (other.position);
    }

    bool DetectVisibility (GameObject go) {
        return DetectVisibility (gameObject.transform.position);
    }

    bool DetectVisibility (Vector3 position) {
        RaycastHit hit;
        Physics.Linecast(transform.parent.position, position, out hit, EnvironmentLayer);
        return (hit.collider == null);
    }

    void OnTriggerEnter(Collider other) {
        if (!isBlind)
        {
            ObjectsInCone.Add(other.gameObject);
            if (DetectVisibility(other.transform))
            {
                ObjectsInVision.Add(other.gameObject);
            }
        }
    }

    void OnTriggerExit(Collider other) {
        ObjectsInCone.Remove(other.gameObject);
        if (ObjectsInVision.Contains(other.gameObject))
        {
            ObjectsInVision.Remove(other.gameObject);
        }
    }

    void Update() {
        var tmpVision = new List<GameObject>(ObjectsInVision);
        var tmpCone = new List<GameObject>(ObjectsInCone);
        foreach (var obj in tmpVision) {
            if (obj != null && !DetectVisibility (obj))
                ObjectsInVision.Remove (obj);
        }
        foreach (var obj in tmpCone) {
            if (obj != null && DetectVisibility (obj) && !ObjectsInVision.Contains (obj))
                ObjectsInVision.Add (obj);
        }
    }
}
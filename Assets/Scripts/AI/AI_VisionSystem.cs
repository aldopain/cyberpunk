using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_VisionSystem : MonoBehaviour {
    [Header("Settings")]
    public LayerMask DetectableLayers;
    public bool isBlind;

    [Header("System")]
    public AI_SensorySystem SensorySystem;

    List<GameObject> ObjectsInCone = new List<GameObject>();
    List<GameObject> ObjectsInVision = new List<GameObject>();

    void Start() {
        StartCoroutine(SendVision());
    }

    IEnumerator SendVision() {
        while (true) {
            yield return new WaitForSeconds(SensorySystem.UpdateRate);
            SensorySystem.RecieveVision(ObjectsInVision);
        }
    }

    //Returns true if other transform with layer Environment can be hit with a ray
    bool DetectVisibility (Transform other) {
        Vector3 ThisCharacterPosition = GetComponentInParent<Transform>().position;
        RaycastHit hit;
        Physics.Linecast (ThisCharacterPosition, other.position, out hit, (1 << 9));
        return (hit.collider == null);
    }

    void OnTriggerEnter(Collider other) {
        if (!isBlind) {
            ObjectsInCone.Add(other.gameObject);
            if (DetectVisibility(other.transform)) {
                print ("I SEE U");
                ObjectsInVision.Add(other.gameObject);
            }
        }
    }

    void OnTriggerExit(Collider other) {
        ObjectsInCone.Remove(other.gameObject);
        if (ObjectsInVision.Contains(other.gameObject)) {
            print ("I DONT SEE U");
            ObjectsInVision.Remove(other.gameObject);
        }
    }
}

using UnityEngine;

// Attach this script to the sprite you want to properly render.

public class FacingCamera : MonoBehaviour {
	void Update () {
        transform.rotation = Camera.main.transform.rotation;
	}
}

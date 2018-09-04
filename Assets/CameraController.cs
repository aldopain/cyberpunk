using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    public Transform FollowedObject;
    [Range(0, 1)]
    public float FollowSpeed;
    public float OffsetMultiplier;

	void Update () {
        if(FollowedObject != null)
        {
            transform.position = Vector3.Lerp(transform.position, (FollowedObject.position + (Vector3.one * OffsetMultiplier) + Camera.main.ScreenToWorldPoint(Input.mousePosition))/2, FollowSpeed);
        }
	}

    public void SetTarget(Transform t)
    {
        FollowedObject = t;
    }
}

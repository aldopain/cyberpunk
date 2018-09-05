using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    [Header("Follow Settings")]
    public Transform FollowedObject;
    [Range(0, 1)]
    public float FollowSpeed;

    [Header("Zoom Settings")]
    public float orthoSizeTarget;
    [Range(0, 1)]
    public float zoomSpeed;

    [Header("Camera Settings")]
    public float OffsetMultiplier;

    private Camera attachedCamera;

    void Start()
    {
        attachedCamera = GetComponent<Camera>();
    }

	void Update () {
        if(FollowedObject != null)
        {
            transform.position = Vector3.Lerp(transform.position, (FollowedObject.position + (Vector3.one * OffsetMultiplier) + Camera.main.ScreenToWorldPoint(Input.mousePosition))/2, FollowSpeed);
            attachedCamera.orthographicSize = Mathf.Lerp(attachedCamera.orthographicSize, orthoSizeTarget, zoomSpeed);
        }else
        {
            Debug.LogError(name + ": Doesn't have an object to follow.");
        }

        //TEST CODE; TO BE REPLACED
        if (Input.GetKey(KeyCode.Tab))
        {
            orthoSizeTarget = 3f;
        }else
        {
            orthoSizeTarget = 1.5f;
        }
	}

    public void SetTarget(Transform t)
    {
        FollowedObject = t;
    }
}

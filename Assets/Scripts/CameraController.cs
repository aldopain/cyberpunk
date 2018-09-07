﻿using UnityEngine;

public class CameraController : MonoBehaviour {
    //Editor-exposed variables
    [Header("Follow Settings")]
    public Transform FollowedObject;
    [Range(0, 1)]
    public float FollowSpeed;

    [Header("Zoom Settings")]
    public float minZoom;
    public float maxZoom;
    public float orthoSizeTarget_Default;
    public float orthoSizeTarget_Crouch;
    [Range(0, 1)]
    public float zoomSpeed;
    public float scrollAdd;

    //Private variables
    private float orthoSizeTarget;
    private Camera attachedCamera;
    private Vector3 diff;

    void Start()
    {
        attachedCamera = GetComponent<Camera>();
        diff = FollowedObject != null ? transform.position - FollowedObject.position : Vector3.zero;
        orthoSizeTarget = orthoSizeTarget_Default;
    }

	void Update () {
        if(FollowedObject != null) {
            
            //Position setting
            if (Input.GetKey(KeyCode.Tab))
            {
                transform.position = Vector3.Lerp(transform.position, (FollowedObject.position + diff + Camera.main.ScreenToWorldPoint(Input.mousePosition)) / 2, FollowSpeed);
            }
            else
            {
                transform.position = Vector3.Lerp(transform.position, FollowedObject.position + diff, FollowSpeed);
            }
            
            //Size setting
            attachedCamera.orthographicSize = Mathf.Lerp(attachedCamera.orthographicSize, orthoSizeTarget, zoomSpeed);
        } else {
            Debug.LogError(name + ": Doesn't have an object to follow.");
        }

        //Zoom
        if (Input.GetAxis("Mouse ScrollWheel") < 0) {
            if (orthoSizeTarget < maxZoom)
                orthoSizeTarget += scrollAdd;
        }else if (Input.GetAxis("Mouse ScrollWheel") > 0) {
            if (orthoSizeTarget > minZoom)
                orthoSizeTarget -= scrollAdd;
        }


	}

    public void SetSizeTarget(float target)
    {
        orthoSizeTarget = target;
    }
    
    public void SetTarget(Transform t)
    {
        FollowedObject = t;
    }
}

using UnityEngine;

public class CameraController : MonoBehaviour {
    //Editor-exposed variables
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

    //Private variables
    private Camera attachedCamera;

    void Start()
    {
        attachedCamera = GetComponent<Camera>();
    }

	void Update () {
        if(FollowedObject != null)
        {
            //Move camera between mouse cursor and FollowTarget
            transform.position = Vector3.Lerp(transform.position, (FollowedObject.position + (Vector3.one * OffsetMultiplier) + Camera.main.ScreenToWorldPoint(Input.mousePosition))/2, FollowSpeed);
            
            //Zoom to the orthoSizeTarget
            attachedCamera.orthographicSize = Mathf.Lerp(attachedCamera.orthographicSize, orthoSizeTarget, zoomSpeed);
        }else
        {
            Debug.LogError(name + ": Doesn't have an object to follow.");
        }

        //TEST CODE; TO BE REPLACED
        //Pressing Tab zoomes out the camera to the hardcoded value (for now; don't see any reason to develop this part further)
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

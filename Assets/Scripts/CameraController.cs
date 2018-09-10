using UnityEngine;

public class CameraController : MonoBehaviour {
    //Editor-exposed variables
    [Header("Follow Settings")]
    public Transform FollowedObject;
    [Range(0, 1)]
    public float FollowSpeed;

    [Header("Freelook Settings")]
    [Range(0,1)]
    public float FreelookSpeed;
    public float FreelookLength;
    public float orthoSizeTarget_Freelook;

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
                if(Vector3.Magnitude(Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position) < FreelookLength)
                {
                    transform.position = Vector3.Lerp(transform.position, (FollowedObject.position + diff + Camera.main.ScreenToWorldPoint(Input.mousePosition)) / 2, FreelookSpeed);
                }else
                {
                    Vector3 cameraPos;
                    float angle = Mathf.Atan2(Camera.main.ScreenToWorldPoint(Input.mousePosition).y - transform.position.y, Camera.main.ScreenToWorldPoint(Input.mousePosition).x - transform.position.x);
                    cameraPos.x = FollowedObject.position.x + FreelookLength * Mathf.Cos(-angle);
                    cameraPos.z = FollowedObject.position.z + FreelookLength * Mathf.Sin(-angle);
                    cameraPos.y = Camera.main.transform.position.y;
                    transform.position = Vector3.Lerp(transform.position, cameraPos, FreelookSpeed);
                }
                SetSizeTarget(orthoSizeTarget_Freelook);
            }
            else
            {
                transform.position = Vector3.Lerp(transform.position, FollowedObject.position + diff, FollowSpeed);
            }

            if (Input.GetKeyUp(KeyCode.Tab))
            {
                SetSizeTarget(orthoSizeTarget_Default);
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

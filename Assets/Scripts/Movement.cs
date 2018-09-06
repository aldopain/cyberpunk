using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {
    [Header("Character Speed")]
	public float MovementSpeed = 1.0f;
	public float lookSpeed = 10;
    public float CrouchSpeed = .5f;

    private float _currentSpeed;
    public float Speed
    {
        get
        {
            return _currentSpeed;
        }

        set
        {
            _currentSpeed = value;

            //Possible animation speed changes and audio-visual effects have to be set here
        }
    }

	private Vector3 curLoc;
	private Vector3 prevLoc;
	CharacterController cc;
    [SerializeField]
    Transform target;

    //Original values
	float originalRotation = -180;
    float originalCCHeight;

    private bool _isCrouching;
    public bool isCrouching
    {
        get
        {
            return _isCrouching;
        }
    }


	// Use this for initialization
	void Start () {
		cc = GetComponent<CharacterController>();
        target.position = transform.position;
        originalCCHeight = cc.height;
        Speed = MovementSpeed;
	}

	void Move () {
		var h = Input.GetAxisRaw("Horizontal");
		var v = Input.GetAxisRaw("Vertical");

		if (h != 0 || v != 0) {
			Vector3 direction = new Vector3(h, 0f, v);
	
			direction = Camera.main.transform.TransformDirection(direction);

			var buf = transform.position + direction;
			buf.y = transform.position.y;
			target.position = buf;
			
			direction.y = -1000f;
			transform.LookAt(target, Vector3.up);
			transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + 90, transform.eulerAngles.z);
			cc.Move(direction * Time.deltaTime * Speed);
		}
	}

    void Crouch()
    {
        //Return CC to its original height and position
        cc.height /= 2;
        cc.Move(Vector3.down * (cc.height));

        //Set flag
        _isCrouching = true;

        //Set camera size
        CameraController _cam = Camera.main.GetComponent<CameraController>();
        _cam.SetSizeTarget(_cam.orthoSizeTarget_Crouch);

        //Set movement speed
        Speed = CrouchSpeed;
    }

    void Stand()
    {
        //Return CC to its original height and position
        cc.height = originalCCHeight;
        cc.Move(Vector3.up * (cc.height / 4));

        //Set flag
        _isCrouching = false;

        //Set camera size
        CameraController _cam = Camera.main.GetComponent<CameraController>();
        _cam.SetSizeTarget(_cam.orthoSizeTarget_Default);

        //Set movement speed
        Speed = MovementSpeed;
    }

	// Update is called once per frame
	void Update () {
		Move();

        //TO BE REWORKED; TESTING CROUCH MECHANICS
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Crouch();
        }

        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            Stand();
        }
	}
}
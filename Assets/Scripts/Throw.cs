using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throw : MonoBehaviour {
	public float firingAngle = 45.0f;
	public GameObject SelectedThrowableObject;
	public bool RecieveInput;
	private PlayerAiming _aim;

	// Use this for initialization
	void Start () {
		if (RecieveInput) {
			_aim = GameObject.Find ("Player").GetComponent<PlayerAiming>();
		}
	}

    bool SelectThrowableObject (GameObject t) {
        if (t.GetComponent<ThrowableObject>() != null) {
            SelectedThrowableObject = t;
            return true;
        }
        return false;
    }
	
	// Update is called once per frame
	void Update () {
		if (RecieveInput && Input.GetKeyDown (KeyCode.G)) {
			var Thrown = Instantiate (SelectedThrowableObject);
			
			Thrown.transform.position = transform.position + new Vector3 (0, 2 * GetComponent<CharacterController>().bounds.extents.y, 0);
			Thrown.GetComponent<ThrowableObject>().Throw (_aim.GetTargetPosition(), 45f);
		}
	}
}

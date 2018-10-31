using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IK_Legs : MonoBehaviour {

	Animator anim;
	Vector3 leftFootPos;
	Vector3 rightFootPos;
	Quaternion leftFootRot;
	Quaternion rightFootRot;

	Transform leftFoot;
	Transform rightFoot;

	public float offsetY;
    public float footOffset;
    public float leftFootWeight;
    public float rightFootWeight;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
        leftFoot = anim.GetBoneTransform(HumanBodyBones.LeftFoot);
        leftFootRot = leftFoot.rotation;
        rightFoot = anim.GetBoneTransform(HumanBodyBones.RightFoot);
        rightFootRot = rightFoot.rotation;
	}
	
	// Update is called once per frame
	void Update () {
		r();
	}

    void r () {
		RaycastHit leftHit;
        Vector3 lpos = leftFoot.position;
        if (Physics.Raycast (lpos + Vector3.up * footOffset, Vector3.down, out leftHit, Mathf.Infinity, 1 << 9)) {
            leftFootPos = leftHit.point + Vector3.up * offsetY;
            leftFootRot = Quaternion.FromToRotation (transform.up, leftHit.normal) * transform.rotation;
            Debug.DrawLine(lpos, leftFootPos);
        }

        RaycastHit rightHit;
        Vector3 rpos = rightFoot.position;
        if (Physics.Raycast (rpos + Vector3.up * footOffset, Vector3.down, out rightHit, Mathf.Infinity, 1 << 9)) {
            rightFootPos = Vector3.Lerp (rpos, rightHit.point + Vector3.up * offsetY, Time.deltaTime * 100f);
            rightFootRot = Quaternion.FromToRotation (transform.up, rightHit.normal) * transform.rotation;
            Debug.DrawLine(rpos, rightFootPos);
        }
	}

    void OnAnimatorIK () {
        // leftFootWeight = anim.GetFloat();
        // rightFootWeight = anim.GetFloat();

        anim.SetIKPositionWeight (AvatarIKGoal.LeftFoot, leftFootWeight);
        anim.SetIKPosition (AvatarIKGoal.LeftFoot, leftFootPos);

        anim.SetIKRotationWeight (AvatarIKGoal.LeftFoot, leftFootWeight);
        anim.SetIKRotation (AvatarIKGoal.LeftFoot, leftFootRot);

        anim.SetIKPositionWeight (AvatarIKGoal.RightFoot, rightFootWeight);
        anim.SetIKPosition (AvatarIKGoal.RightFoot, rightFootPos);

        anim.SetIKRotationWeight (AvatarIKGoal.RightFoot, rightFootWeight);
        anim.SetIKRotation (AvatarIKGoal.RightFoot, rightFootRot);
    }
}
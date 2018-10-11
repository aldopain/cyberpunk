using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IK_Legs : MonoBehaviour {

	Animator anim;
	Vector3 leftFootPos;
	Vector3 rightFootPos;
	Quaternion leftFootRot;
	Quaternion rightFootRot;
	float leftFootWeight;
	float rightFootWeight;

	Transform leftFoot;
	Transform rightFoot;

	public float offsetY;

	public float lookIKWeight;
	public float eyesWeight;
	public float headWeight;
	public float bodyWeight;
	public float clampWeight;

	public Transform targetPos;

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
		RaycastHit leftHit;
        Vector3 lpos = leftFoot.position;
        if (Physics.Raycast (lpos + Vector3.up * 0.5f, Vector3.down, out leftHit, 1)) {
            leftFootPos = Vector3.Lerp (lpos, leftHit.point + Vector3.up * offsetY, Time.deltaTime * 10f);
            leftFootRot = Quaternion.FromToRotation (transform.up, leftHit.normal) * transform.rotation;
            Debug.DrawLine(lpos, leftFootPos);
        }

        RaycastHit rightHit;
        Vector3 rpos = leftFoot.position;
        if (Physics.Raycast (rpos + Vector3.up * 0.5f, Vector3.down, out rightHit, 1)) {
            rightFootPos = Vector3.Lerp (rpos, rightHit.point + Vector3.up * offsetY, Time.deltaTime * 10f);
            rightFootRot = Quaternion.FromToRotation (transform.up, rightHit.normal) * transform.rotation;
            Debug.DrawLine(rpos, rightFootPos);
        }
	}

    void OnAnimatorIK () {
        anim.SetLookAtWeight (lookIKWeight, bodyWeight, headWeight, eyesWeight, clampWeight);
        anim.SetLookAtPosition (targetPos.position);

        leftFootWeight = anim.GetFloat ("LeftFoot");
        Debug.Log(leftFootWeight);

        anim.SetIKPositionWeight (AvatarIKGoal.LeftFoot, leftFootWeight);
        anim.SetIKPosition (AvatarIKGoal.LeftFoot, leftFootPos);

        anim.SetIKRotationWeight (AvatarIKGoal.LeftFoot, leftFootWeight);
        anim.SetIKRotation (AvatarIKGoal.LeftFoot, leftFootRot);

        rightFootWeight = anim.GetFloat ("RightFoot");

        anim.SetIKPositionWeight (AvatarIKGoal.RightFoot, rightFootWeight);
        anim.SetIKPosition (AvatarIKGoal.RightFoot, rightFootPos);

        anim.SetIKRotationWeight (AvatarIKGoal.RightFoot, rightFootWeight);
        anim.SetIKRotation (AvatarIKGoal.RightFoot, rightFootRot);
    }
}

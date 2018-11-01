using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolveHelper : MonoBehaviour {
    public float Speed;
    float f = 0f;
    bool isDissolving;
    bool isSolving;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (isDissolving && f < 1f)
        {
            f += Speed;
        }

        if (isSolving && f > 0f)
        {
            f -= Speed;
        }

        SetCutout();
    }

    void SetCutout()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).GetComponent<MeshRenderer>() != null)
                transform.GetChild(i).GetComponent<MeshRenderer>().material.SetFloat("_Cutout", f);
        }
    }

    public void DissolveChildren()
    {
        isDissolving = true;
        isSolving = false;
    }

    public void SolveChildren()
    {
        isDissolving = false;
        isSolving = true;
    }
}

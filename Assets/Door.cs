using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Door : MonoBehaviour {

    public List<string> tags;

    UnityEvent OnOpen;
    UnityEvent OnClose;

    Animator animator;
    List<Collider> colliders;

	void Start () {
        colliders = new List<Collider>();
        OnOpen = new UnityEvent();
        OnClose = new UnityEvent();
        animator = GetComponent<Animator>();
	}

    private void OnTriggerEnter(Collider other)
    {
        bool validCollider = false;

        foreach(var tag in tags)
        {
            if (other.CompareTag(tag))
                validCollider = true;
        }

        if (validCollider)
        {
            colliders.Add(other);
            OnOpen.Invoke();
            animator.SetBool("IsClosed", false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (colliders.Contains(other))
            colliders.Remove(other);

        if (colliders.Count == 0)
        {
            OnClose.Invoke();
            animator.SetBool("IsClosed", true);
        }
    }
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_VisionSystem : MonoBehaviour {
    [Header("Settings")]
    public LayerMask DetectableLayers;
    public bool isBlind;

    [Header("System")]
    public AI_SensorySystem SensorySystem;

    List<GameObject> ObjectsInCone = new List<GameObject>();
    List<GameObject> ObjectsInVision = new List<GameObject>();

    void Start()
    {
        StartCoroutine(SendVision());
    }

    IEnumerator SendVision()
    {
        while (true)
        {
            yield return new WaitForSeconds(SensorySystem.UpdateRate);
            SensorySystem.RecieveVision(ObjectsInVision);
        }
    }

    //Returns true if other transform can be hit with a ray
    bool DetectVisibility(Transform other)
    {
        return true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!isBlind)
        {
            ObjectsInCone.Add(other.gameObject);
            if (DetectVisibility(other.transform))
            {
                ObjectsInVision.Add(other.gameObject);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        ObjectsInCone.Remove(other.gameObject);
        if (ObjectsInVision.Contains(other.gameObject))
        {
            ObjectsInVision.Remove(other.gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerGroup : MonoBehaviour {

    void Start()
    {

    }

    public void SpawnAll()
    {
        Transform tmp;
        for(int i = 0; i < transform.childCount; i++)
        {
            tmp = transform.GetChild(i);
            if(tmp.GetComponent<Spawner>() != null)
            {
                tmp.GetComponent<Spawner>().Spawn();
            }
        }
    }

    public void ResetAll()
    {
        Transform tmp;
        for (int i = 0; i < transform.childCount; i++)
        {
            tmp = transform.GetChild(i);
            if (tmp.GetComponent<Spawner>() != null)
            {
                tmp.GetComponent<Spawner>().Reset();
            }
        }
    }
}

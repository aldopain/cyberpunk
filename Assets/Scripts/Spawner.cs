using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {
    //Editor-exposed settings
    [Header("Objects Settings")]
    [SerializeField]
    private GameObject ObjectPrefab;

    [Header("Spawner Settings")]
    public int SpawnLimit;
    public bool Continuous;
    public bool SpawnOnStart;

    //Private variables
    private int objectsSpawned;

	// Use this for initialization
	void Start () {
        if (SpawnOnStart)
        {
            Spawn();
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Reset()
    {
        objectsSpawned = 0;
    }

    public void Spawn()
    {
        if(objectsSpawned < SpawnLimit && SpawnLimit > 0)
        {
            GameObject spawnedObject = Instantiate(ObjectPrefab, transform.position, transform.rotation);
            objectsSpawned++;

            if (Continuous && spawnedObject.GetComponent<HealthController>() != null)
            {
                spawnedObject.GetComponent<HealthController>().OnDeath.AddListener(Spawn);
            }
        }
    }
}

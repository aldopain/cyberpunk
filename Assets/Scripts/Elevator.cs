using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class Elevator : MonoBehaviour {
    public Transform[] Stops;
    public float Speed;
    public float DistanceOffset;

    Rigidbody body;

    [Header("Events")]
    public UnityEvent OnStart;
    public UnityEvent OnMoving;
    public UnityEvent OnStop;

    private Vector3 targetStop;
    private bool isMoving;
    private int lastStop;

    void Start()
    {
        body = GetComponent<Rigidbody>();
    }
    
    void Update()
    {
        if(Vector3.Distance(transform.position, targetStop) <= DistanceOffset)
        {
            Stop();
        }
    }

    public void Goto(int i)
    {
        targetStop = Stops[i].position;
        body.velocity = Vector3.Normalize(targetStop - transform.position) * Speed;
        OnStart.Invoke();
        lastStop = i;
    }

    public void Stop()
    {
        body.velocity = Vector3.zero;

        OnStop.Invoke();
    }

    public void Goto_Next()
    {
        Goto(++lastStop);
    }

    public void Goto_Prev()
    {
        Goto(--lastStop);
    }
}

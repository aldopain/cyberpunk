using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableObject : MonoBehaviour {
    static float gravity = 9.8f;

	// Use this for initialization
	void Start () {
		
	}

    public void Throw (Vector3 TargetPosition, float FiringAngle, Vector3 Offset) {
        StartCoroutine (SimulateProjectile(TargetPosition, FiringAngle, Offset));
    }

    public void Throw (Vector3 TargetPosition, float FiringAngle) {
        StartCoroutine (SimulateProjectile(TargetPosition, FiringAngle, new Vector3 (0f, 0f, 0f)));
    }

	private IEnumerator SimulateProjectile(Vector3 TargetPosition, float FiringAngle, Vector3 Offset)
    {
        // Move projectile to the position of throwing object + add some offset if needed.
        transform.position += Offset;
       
        // Calculate distance to target
        float target_Distance = Vector3.Distance(transform.position, TargetPosition);
 
        // Calculate the velocity needed to throw the object to the target at specified angle.
        float Velocity = target_Distance / (Mathf.Sin(2 * FiringAngle * Mathf.Deg2Rad) / gravity);
 
        // Extract the X  Y componenent of the velocity
        float Vx = Mathf.Sqrt(Velocity) * Mathf.Cos(FiringAngle * Mathf.Deg2Rad);
        float Vy = Mathf.Sqrt(Velocity) * Mathf.Sin(FiringAngle * Mathf.Deg2Rad);
 
        // Calculate flight time.
        float flightDuration = target_Distance / Vx;
   
        // Rotate projectile to face the target.
        transform.rotation = Quaternion.LookRotation(TargetPosition - transform.position);
       
        float elapse_time = 0;
 
        while (elapse_time < flightDuration)
        {
            transform.Translate(0, (Vy - (gravity * elapse_time)) * Time.deltaTime, Vx * Time.deltaTime);
           
            elapse_time += Time.deltaTime;
 
            yield return null;
        }
    }

	// Update is called once per frame
	void Update () {
		
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flame : MonoBehaviour {
    public int Damage = 1;
    public float DamageDelay = .7f;
    public int Duration = 2;
    private HealthController hc;

    void OnEnable()
    {
        if(GetComponent<HealthController>() != null)
        {
            hc = GetComponent<HealthController>();
            Destroy(this, Duration);
            StartCoroutine(DealDamage());
        }
    }

    IEnumerator DealDamage()
    {
        while (true)
        {
            yield return new WaitForSeconds(DamageDelay);
            hc.ChangeHealth(-Mathf.Abs(Damage));
        }
    }
}

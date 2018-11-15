using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class Bullet : MonoBehaviour
{
    public float ShootingVelocityModifier;
    public int DamageModifier;
    private float Range;
    public float AngleDeflectionAbs;
    public string[] IgnoredTags;
    public string[] TransmittedComponents;
    private int Damage;
    public void Shoot(int _damage, float _range, float _velocity)
    {
        if (Time.timeScale != 0f) {
            Damage = DamageModifier + _damage;
            Range = _range;

            transform.Rotate (new Vector3 (0, Random.Range(-AngleDeflectionAbs, AngleDeflectionAbs), 0));
            GetComponent<Rigidbody>().velocity = _velocity * ShootingVelocityModifier * transform.forward;
            Destroy(this.gameObject, Range / (_velocity * ShootingVelocityModifier));
        }
    }

    bool isIgnoredTag(string s)
    {
        foreach (string tag in IgnoredTags)
        {
            if (s == tag)
            {
                return true;
            }
        }
        return false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!isIgnoredTag(other.tag))
        {
            if (other.GetComponent<HealthController>() != null)
            {
                other.GetComponent<HealthController>().ChangeHealth(-Mathf.Abs(Damage));
                other.GetComponent<HealthController>().OnHit.Invoke();
                TransmitComponents(other.gameObject);
            }

            Destroy(gameObject);
        }

    }

    void TransmitComponents(GameObject other)
    {
        foreach (string s in TransmittedComponents)
        {
            System.Type type = System.Type.GetType(s);
            other.AddComponent(type);
        }
    }
}

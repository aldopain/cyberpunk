using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour {
    [Header("Settings")]
    [SerializeField]
    private int MaxHealth;
    [SerializeField]
    private int CurrentHealth;
    public bool StartWithCustomHealth;

    private bool _isDead;
	// Use this for initialization
	void Start () {
        if (!StartWithCustomHealth)
        {
            CurrentHealth = MaxHealth;
        }		
	}
	
    void Death()
    {
        _isDead = true;
    }

    public int GetMaxHealth()
    {
        return MaxHealth;
    }

    public int GetHealth()
    {
        return CurrentHealth;
    }

    public void SetHealth(int v)
    {
        if(v > MaxHealth)
        {
            CurrentHealth = MaxHealth;
        }else
        {
            if(v <= 0)
            {
                Death();
            }else
            {
                CurrentHealth = v;
            }
        }
    }

    public void ChangeHealth(int v)
    {
        if(CurrentHealth + v > MaxHealth)
        {
            CurrentHealth = MaxHealth;
        }else
        {
            if(CurrentHealth + v <= 0)
            {
                CurrentHealth = 0;
                Death();
            }else
            {
                CurrentHealth += v;
            }
        }
    }
}

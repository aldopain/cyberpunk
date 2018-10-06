using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_HearingSystem : MonoBehaviour {
    [Header("Settings")]
    public float DetectionRadius;                           // Radius of the Overlap Sphere
    public LayerMask DetectionMask;                         // 
    public bool isDeaf;                                     // Does this agent recieve sound

    List<GameObject> RealSounds = new List<GameObject>();       
    List<GameObject> PseudoSounds = new List<GameObject>();

    AI_SensorySystem SensorySystem;

    void Start () {
        SensorySystem = GetComponent<AI_SensorySystem>();
        StartCoroutine(SendSounds());
	}

    IEnumerator SendSounds()
    {
        while (true)
        {
            yield return new WaitForSeconds(SensorySystem.UpdateRate);
            SearchSounds();
            SensorySystem.RecieveSounds(RealSounds, PseudoSounds);
        }
    }

    //TODO: REWRITE WITH MORE EFFICIENT SOLUTION
    void SearchSounds()
    {
        Collider[] tmpSounds = Physics.OverlapSphere(transform.position, DetectionRadius, DetectionMask);
        AI_Sound tmp;
        RealSounds.Clear();
        PseudoSounds.Clear();
        foreach(Collider c in tmpSounds)
        {
            if((tmp = c.GetComponent<AI_Sound>()) != null)
            {
                if(tmp.Type == AI_Sound.SoundTypes.Real)
                {
                    if (!RealSounds.Contains(c.gameObject))
                    {
                        RealSounds.Add(c.gameObject);
                    }               
                }else
                {
                    if (!PseudoSounds.Contains(c.gameObject))
                    {
                        PseudoSounds.Add(c.gameObject);
                    }
                }
                
            }
        }
    }
}

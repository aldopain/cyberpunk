using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class AI_Sound : MonoBehaviour {
    public string OwnerName;
    public enum SoundTypes
    {
        Real,
        Pseudo
    }

    public SoundTypes Type;
}

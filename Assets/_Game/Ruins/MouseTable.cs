using UnityEngine;

public sealed class MouseTable : Usable
{
    [SerializeField]
    private AudioSource _sfx;
    
    public override void Use()
    {
        _sfx.Play();
    }
}

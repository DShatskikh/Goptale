using System;
using UnityEngine;

public sealed class Trigger : MonoBehaviour
{
    public event Action Event;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.GetComponent<Fedya>())
            return;
        
        Event?.Invoke();
    }
}

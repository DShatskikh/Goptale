using System;
using System.Collections;
using UnityEngine;

public sealed class GopnikShard_1 : MonoBehaviour
{
    [SerializeField]
    private GameObject _parent;

    private bool _isActive;
    
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(1);
        _isActive = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!_isActive)
            return;
        
        if (!other.GetComponent<Heart>())
            return;
        
        Heart.Instance.Damage(3);
        Destroy(_parent);
    }
}

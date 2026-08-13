using System.Collections;
using UnityEngine;

public sealed class GopnikAttack_3 : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _shards;

    [SerializeField]
    private float _duration = 0.5f;
    
    private IEnumerator Start()
    {
        foreach (var shard in _shards)
        {
            shard.SetActive(true);
            yield return new WaitForSeconds(_duration);
        }
        
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }
}

using System.Collections;
using UnityEngine;

public sealed class DJNikolay_Attack_3 : MonoBehaviour
{
    [SerializeField]
    private Transform[] _shards;
    
    private IEnumerator Start()
    {
        foreach (var shard in _shards)
        {
            shard.gameObject.SetActive(true);
            yield return new WaitForSeconds(2f); 
        }
        
        yield return new WaitForSeconds(2f); 
        Destroy(gameObject);
    }
}

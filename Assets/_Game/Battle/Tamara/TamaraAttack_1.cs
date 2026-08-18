using System.Collections;
using UnityEngine;

public sealed class TamaraAttack_1 : MonoBehaviour
{
    [SerializeField]
    private TamaraShard_1 _shard1;
    
    [SerializeField]
    private TamaraShard_3 _shard3;
    
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(0.5f);
        
        for (int i = 0; i < 28; i++)
        {
            var shard = Instantiate(_shard1, new Vector3(0f, 0f, 0), Quaternion.identity, transform);
            shard.Init(false, 4 - i * 0.1f);
            
            shard = Instantiate(_shard1, new Vector3(0f, 0f, 0), Quaternion.identity, transform);
            shard.Init(true, 4 - i * 0.1f);
            
            if (i % 3 == 0)
                Instantiate(_shard3, new Vector3(Random.Range(0, 2) == 1 ? -2.71f : 2.71f, 0f, 0), Quaternion.identity, transform);
            
            yield return new WaitForSeconds(0.2f);
        }
    }
}

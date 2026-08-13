using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public sealed class AlkashAttack : MonoBehaviour
{
    [SerializeField]
    private AlkashShard _alkashShard;
    
    private IEnumerator Start()
    {
        var isHealSpawn = (BattleManager.Instance.Enemies[0] as Alkash).IsHealSpawn;
        var count = 8;
        
        for (int i = 0; i < 7; i++)
        {
            var shard = Instantiate(_alkashShard, new Vector3(-2.79999995f,-1.20000005f,0), Quaternion.identity, transform);
            shard.Direction = 1;
            shard.IsHeal = isHealSpawn && Random.Range(0, count) == 0;
            
            if (shard.IsHeal)
                isHealSpawn = false;
            
            if (count > 0)
                count--;
            
            shard = Instantiate(_alkashShard, new Vector3(2.79999995f,-2.70000005f,0), Quaternion.identity, transform);
            shard.Direction = -1;
            shard.IsHeal = isHealSpawn && Random.Range(0, count) == 0;
            
            if (shard.IsHeal)
                isHealSpawn = false;
            
            if (count > 0)
                count--;
            
            yield return new WaitForSeconds(1.5f);
        }
    }
}

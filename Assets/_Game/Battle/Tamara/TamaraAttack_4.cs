using System.Collections;
using UnityEngine;

public sealed class TamaraAttack_4 : MonoBehaviour
{
    [SerializeField]
    private TamaraShard_5[] _shardsUp;
    
    [SerializeField]
    private TamaraShard_2 _shards2;
    
    private IEnumerator Start()
    {
        StartCoroutine(AwaitHands());
        StartCoroutine(AwaitShardsUp());

        yield return new WaitForSeconds(3);
        
        foreach (var shard in _shardsUp)
        {
            if (shard == null)
                continue;
            
            shard.enabled = true;
            yield return new WaitForSeconds(0.3f);
        }
        
        yield return null;
    }
    
    private IEnumerator AwaitShardsUp()
    {
        foreach (var shard in _shardsUp)
        {
            shard.gameObject.SetActive(true);
            shard.enabled = false;
            yield return new WaitForSeconds(0.6f);
        }
    }
    
    private IEnumerator AwaitHands()
    {
        var speed = 3;
        
        while (true)
        {
            if (_shards2 && _shards2.transform.localPosition.x < 0.667f)
                _shards2.transform.position += new Vector3(1, 0, 0) * Time.deltaTime * speed;
            
            yield return null;
        }
    }
}

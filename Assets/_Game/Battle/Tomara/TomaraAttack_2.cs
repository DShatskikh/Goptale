using System.Collections;
using UnityEngine;

public sealed class TomaraAttack_2 : MonoBehaviour
{
    [SerializeField]
    private GopnikShard_2[] _shardsUp;

    [SerializeField]
    private GopnikShard_2[] _shardsDown;
    
    [SerializeField]
    private TomaraShard_2[] _shards2;
    
    private IEnumerator Start()
    {
        StartCoroutine(AwaitHands());
        StartCoroutine(AwaitShardsUp());
        StartCoroutine(AwaitShardsDown());

        yield return new WaitForSeconds(3);
        
        foreach (var shard in _shardsUp)
        {
            if (shard == null)
                continue;
            
            shard.enabled = true;
            shard.SetDirection(new Vector2(0, 0));
            yield return new WaitForSeconds(0.3f);
        }
        
        yield return new WaitForSeconds(1f);
        
        foreach (var shard in _shardsDown)
        {
            if (shard == null)
                continue;
            
            shard.enabled = true;
            shard.SetDirection(new Vector2(0, 0));
            yield return new WaitForSeconds(0.2f);
        }
        
        yield return null;
    }
    
    private IEnumerator AwaitShardsUp()
    {
        foreach (var shard in _shardsUp)
        {
            shard.gameObject.SetActive(true);
            shard.enabled = false;
            shard.SetDirection(new Vector2(0, 0));
            shard.Speed = 3;
            yield return new WaitForSeconds(0.8f);
        }
    }
    
    private IEnumerator AwaitShardsDown()
    {
        foreach (var shard in _shardsDown)
        {
            shard.gameObject.SetActive(true);
            shard.enabled = false;
            shard.SetDirection(new Vector2(0, 0));
            shard.Speed = 3;
            yield return new WaitForSeconds(0.8f);
        }
    }
    
    private IEnumerator AwaitHands()
    {
        var speed = 2;
        
        while (true)
        {
            if (_shards2[0] && _shards2[0].transform.localPosition.x < 0.667f)
                _shards2[0].transform.position += new Vector3(1, 0, 0) * Time.deltaTime * speed;
            
            if (_shards2[1] && _shards2[1].transform.localPosition.x > -0.691f)
                _shards2[1].transform.position -= new Vector3(1, 0, 0) * Time.deltaTime * speed;
            
            yield return null;
        }
    }
}

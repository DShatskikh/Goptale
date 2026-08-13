using System.Collections;
using UnityEngine;

public class DJNikolay_Attack_2 : MonoBehaviour
{
    [SerializeField]
    private DJNikolay_Shard_2 _prefab;
    
    private IEnumerator Start()
    {
        for (int i = 0; i < 18; i++)
        {
            var positionX = Random.Range(0, 2) == 1 ? Random.Range(-1.56f, 1.56f) : Heart.Instance.transform.position.x;
            Instantiate(_prefab, new Vector3(positionX, 0.17f), Quaternion.identity, transform);
            yield return new WaitForSeconds(0.3f);
        }
        
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }
}

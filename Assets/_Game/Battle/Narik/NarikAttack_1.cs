using System.Collections;
using UnityEngine;

public sealed class NarikAttack_1 : MonoBehaviour
{
    [SerializeField]
    private GameObject _aim, _pistol;
    
    [SerializeField]
    private NarikShard_1 _shardPrefab;

    private IEnumerator Start()
    {
        _aim.gameObject.SetActive(true);
        
        for (int i = 0; i < 6; i++)
        {
            yield return new WaitForSeconds(1f);
            
            var position = Heart.Instance.transform.position;

            while (_aim.transform.position != position)
            {
                _aim.transform.position = Vector2.MoveTowards(_aim.transform.position, position, 5 * Time.deltaTime);
                yield return null;
            }
            
            var angle = _pistol.transform.rotation;
            //yield return new WaitForSeconds(0.5f);
            Instantiate(_shardPrefab, transform.position, angle, transform);
        }
        
        Destroy(gameObject);
    }

    private void Update()
    {
        Vector3 direction = _aim.transform.position - _pistol.transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        _pistol.transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}

using System.Collections;
using UnityEngine;

public sealed class TamaraAttack_3 : MonoBehaviour
{
    [SerializeField]
    private TamaraShard_4 _prefab;

    private IEnumerator Start()
    {
        for (int i = 0; i < 20; i++)
        {
            Instantiate(_prefab, new Vector3(5f + Random.Range(-1f, 1f), 0f, 0), Quaternion.identity, transform);
            Instantiate(_prefab, new Vector3(-5f + Random.Range(-1f, 1f), 0f, 0), Quaternion.identity, transform);
            yield return new WaitForSeconds(0.5f);
        }
    }
}

using System.Collections;
using UnityEngine;

public sealed class ArrowsHint : MonoBehaviour
{
    [SerializeField]
    private GameObject _container;
    
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(2);
        _container.SetActive(false);
        yield return new WaitForSeconds(1);
        _container.SetActive(true);
        yield return new WaitForSeconds(2);
        _container.SetActive(false);
        gameObject.SetActive(false);
    }
}

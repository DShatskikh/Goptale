using UnityEngine;

public sealed class GenocideSwitcher : MonoBehaviour
{
    [SerializeField]
    private GameObject _normal, _genocide;

    private void Start()
    {
        if (Stats.Instance.IsGenocide)
        {
            if (_normal)
                _normal.SetActive(false);
            
            if (_genocide)
                _genocide.SetActive(true);
        }
        else
        {
            if (_normal)
                _normal.SetActive(true);
            
            if (_genocide)
                _genocide.SetActive(false);
        }
    }
}

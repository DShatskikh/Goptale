using UnityEngine;

public sealed class Level15_2_1 : MonoBehaviour
{
    [SerializeField]
    private GameObject _belash;
    
    private void Start()
    {
        if (Stats.Instance.TomaraCutscene >= 12)
        {
            _belash.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}

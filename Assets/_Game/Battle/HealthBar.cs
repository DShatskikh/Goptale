using TMPro;
using UnityEngine;

public sealed class HealthBar : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _label;

    [SerializeField]
    private Transform _bar;
    
    private void Update()
    {
        var hp = (float)Stats.Instance.HP;

        if (hp < 0)
            hp = 0;
        
        var progress = 1 - hp / Stats.Instance.MaxHP;
        
        _label.text = $"{hp}/{Stats.Instance.MaxHP}";
        _bar.localPosition = new Vector3(Mathf.Lerp(0, -0.5f, progress), _bar.localPosition.y); // -0.5
        _bar.localScale = new Vector3(Mathf.Lerp(1, 0f, progress), _bar.localScale.y); // 0
    }
}

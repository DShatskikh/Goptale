using UnityEngine;

public sealed class BattleButton : MonoBehaviour
{
    [SerializeField]
    private Sprite _selected;
    
    [SerializeField]
    private Sprite _unselected;

    [SerializeField]
    private int _currentIndex;
    
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        _spriteRenderer.color = Color.white;
        var items = Stats.Instance.Items;
        var isEmptyItems = true;

        foreach (var item in items)
        {
            if (!string.IsNullOrEmpty(item))
            {
                isEmptyItems = false;
            }
        }
        
        if (BattleManager.Instance.IsSelectMain && BattleManager.Instance.SelectMainButtonIndex == _currentIndex)
        {
            _spriteRenderer.sprite = _selected;
        }
        else
        {
            _spriteRenderer.sprite = _unselected;
            
            if (BattleManager.Instance.IsProhibitionSelectMain || (isEmptyItems && _currentIndex == 2))
            {
                _spriteRenderer.color = new Color32(130, 130, 130, 255);
            }
        }
    }
}

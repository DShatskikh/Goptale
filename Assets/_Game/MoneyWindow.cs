using TMPro;
using UnityEngine;

public sealed class MoneyWindow : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _label;
    
    public static MoneyWindow Open()
    {
        var moneyWindow = Instantiate(Resources.Load<MoneyWindow>("Money Window"),
            new Vector3(Camera.main.transform.position.x + 4.804f, Camera.main.transform.position.y - 0.3424f), Camera.main.transform.rotation);

        var itemCount = 0;

        foreach (var item in Stats.Instance.Items)
        {
            if (!string.IsNullOrEmpty(item))
            {
                itemCount++;
            }
        }
        
        moneyWindow._label.text = $"$-{Stats.Instance.RUB}РУБ\nМЕСТО-{itemCount}/8";
        return moneyWindow;
    }
}

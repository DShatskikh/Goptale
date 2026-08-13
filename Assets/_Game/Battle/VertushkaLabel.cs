using TMPro;
using UnityEngine;

public sealed class VertushkaLabel : MonoBehaviour
{
    public TMP_Text Label;
    public TMP_Text Star;
    public Transform HealthSlide;

    public void SetHealth(int health, int maxHealth, bool isYellow)
    {
        Label.color = isYellow ? Color.yellow : Color.white;
        Star.color = isYellow ? Color.yellow : Color.white;
        HealthSlide.localScale = new Vector3(Mathf.Lerp(0, 1, health / ((float)maxHealth)), 1, 1);
        HealthSlide.localPosition = new Vector2((1 - HealthSlide.localScale.x) / -2, 0);
    }
}

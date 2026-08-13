using System.Collections;
using TMPro;
using UnityEngine;

public sealed class DamageIndicator : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _label;

    [SerializeField]
    private Transform _healthBar;
    
    public IEnumerator Init(Enemy enemy, int damage)
    {
        transform.position = enemy.transform.position + new Vector3(0, 1.83f);
        _label.text = damage.ToString();
        _label.color = Color.red;
        
        if (damage <= 0)
        {
            _label.text = "ПРОМАХ";
            _label.color = Color.white;
        }

        StartCoroutine(AwaitLabelAnimation());
        yield return StartCoroutine(AwaitInit(enemy, damage));
    }

    private IEnumerator AwaitInit(Enemy enemy, int damage)
    {
        var health = (float)enemy.Health;
        var startHealth = health;
        var targetHealth = Mathf.Max(0, startHealth - damage);
    
        float animationSpeed = startHealth - targetHealth;
        
        while (health > targetHealth)
        {
            yield return null;
            health -= animationSpeed * Time.deltaTime;
        
            health = Mathf.Max(health, targetHealth);
        
            float progress = health / enemy.MaxHealth;
            _healthBar.localScale = new Vector3(Mathf.Lerp(0, 1, progress), 1, 1);
            _healthBar.localPosition = new Vector2((1 - _healthBar.localScale.x) / -2, 0);
        }
        
        if (damage <= 0)
        {
            yield return new WaitForSeconds(1);
        }
        else
        {
            yield return new WaitForSeconds(0.5f);
        }
    }

    private IEnumerator AwaitLabelAnimation()
    {
        while (_label.transform.localPosition != new Vector3(0, 1.17f))
        {
            yield return null;
            _label.transform.localPosition = Vector2.MoveTowards(_label.transform.localPosition, 
                new Vector3(0, 1.17f), Time.deltaTime / 0.5f);
        }
        
        while (_label.transform.localPosition != new Vector3(0, 0.71f))
        {
            yield return null;
            _label.transform.localPosition = Vector2.MoveTowards(_label.transform.localPosition, 
                new Vector3(0, 0.71f), Time.deltaTime / 0.75f);
        }
    }
}

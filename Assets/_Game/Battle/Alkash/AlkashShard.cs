using UnityEngine;

public sealed class AlkashShard : MonoBehaviour
{
    [SerializeField]
    private float _speed = 2f;
    
    public float Direction = 1;
    public bool IsHeal;
    
    private void Update()
    {
        if (IsHeal)
            GetComponent<SpriteRenderer>().color = Color.green;
        
        transform.position += new Vector3(Direction * _speed * Time.deltaTime, 0, 0);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.GetComponent<Heart>())
            return;

        if (IsHeal)
        {
            Heart.Instance.Heal(1);
            BattleManager.Instance.Enemies[0].Relationship += 5;
        }
        else
            Heart.Instance.Damage(3);
        
        Destroy(gameObject);
    }
}

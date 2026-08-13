using UnityEngine;

public sealed class TomaraShard_2 : MonoBehaviour
{
    // рука
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.GetComponent<Heart>())
            return;
        
        Heart.Instance.Damage(3);
        (BattleManager.Instance.Enemies[0] as Tomara).NextTurnTimer = 0;
    }
}

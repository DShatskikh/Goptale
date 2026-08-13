using System.Collections;
using UnityEngine;

public sealed class EnemyTest : Enemy
{
    [SerializeField]
    private GameObject _attack;
    
    private IEnumerator Start()
    {
        BattleManager.Instance.Enemies.Add(this);
        
        BattleManager.Instance.IsSkipPlayerTurn = true;
        BattleManager.Instance.TargetSizeFrame = new Vector3(3f, 1.75f, 0);
        
        var attack = Instantiate(_attack);
        var NextTurnTimer = 10f;
        
        while (NextTurnTimer > 0)
        {
            NextTurnTimer -= Time.deltaTime;
            yield return null;
        }
            
        Destroy(attack);
    }
}

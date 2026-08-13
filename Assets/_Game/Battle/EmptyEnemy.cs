using System.Collections;
using UnityEngine;

public sealed class EmptyEnemy : Enemy
{
    private IEnumerator Start()
    {
        MusicManager.Instance.Stop();
        BattleManager.MainText = "Но никто не пришел...";
        BattleManager.Instance.SelectMainButtonIndex = -1;
        Heart.Instance.GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitUntil(() => Input.GetButtonDown("Submit"));
        yield return BattleManager.Instance.AwaitExit();
    }
}

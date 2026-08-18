using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class German : Enemy
{
    [SerializeField]
    private AudioClip _theme;
    
    private TextBubble _textBubble;
    private Animator _animator;
    
    private IEnumerator Start()
    {
        _animator = GetComponent<Animator>();
        
        BattleManager.Instance.Enemies.Add(this);
        Relationship = -1;
        
        Actions = new List<string>
        {
            "Чекнуть",
            "Базар"
        };

        ActionAnswers = new List<string[]>
        {
            new[] {"Герман - 0 АТК 0 ЗЩТ*Он держит <color=\"yellow\">АНТИПОХМЕЛИН</color>."},
            new[] {"Вы базарите с ГЕРМАНОМ.*#...", "Но это просто Герман.", "ТАМАРА выглядит такой счастливой."}
        };
        
        // Герман выглядит так, как будто собирается упасть.
            
        // Вы говорите с манекеном
        // ...
            
        // Кажется, он не расположен к разговору.
            
        // ТОРИЭЛЬ выглядит такой счастливой.
            
        // ВЫ ПОБЕДИЛИ!
        // Вы получили 0 ОП и 0 М.
        
        MusicManager.Instance.Play(_theme);
        BattleManager.MainText = "Вы столкнулись с Германом.";
        BattleManager.AdditionalExitText = new[]{"Вы получили <color=\"yellow\">АНТИПОХМЕЛИН</color>."};
        
        StartCoroutine(AwaitExit());
        
        while (true)
        {
            yield return new WaitUntil(() => BattleManager.Instance.IsEnemyTurn);
            
            _textBubble = Instantiate(Resources.Load<TextBubble>("TextBubble"));
            _textBubble.transform.position = transform.position + new Vector3(3.95f, 0.4400001f, 0);
            
            _textBubble.SetText(".....");
            _textBubble.Duration = 0.3f;
            yield return new WaitUntil(() => _textBubble == null);
            BattleManager.MainText = "Герман втыкает.";
            
            if (BattleManager.Instance.SelectMainButtonIndex == 0)
            {
                BattleManager.Instance.IsEnemyTurn = false;
            }
            else if (BattleManager.Instance.SelectMainButtonIndex == 1)
            {
                if (BattleManager.Instance.SelectBazarIndex == 0)
                {
                    BattleManager.Instance.IsEnemyTurn = false;
                }
                else if (BattleManager.Instance.SelectBazarIndex == 1)
                {
                    StartCoroutine(BattleManager.Instance.AwaitExitMessage());
                    yield break;
                }
            }
            else if (BattleManager.Instance.SelectMainButtonIndex == 2)
            {
                BattleManager.Instance.IsEnemyTurn = false;
            }
            else if (BattleManager.Instance.SelectMainButtonIndex == 3)
            {
                BattleManager.Instance.IsEnemyTurn = false;

                if (BattleManager.Instance.CountTurn >= 4)
                {
                    StartCoroutine(BattleManager.Instance.AwaitExitMessage());
                    yield break;
                }
            }
        }
    }

    private IEnumerator AwaitExit()
    {
        yield return new WaitUntil(() => BattleManager.Instance.IsEndBattle);
        Stats.Instance.TomaraCutscene = 2;
        
        if (Health <= 0)
        {
            Stats.Instance.GermanState = 3;
            _animator.Play("Death");
        }
        else
        {
            Stats.Instance.GermanState = 2;
            _animator.Play("Mercy");
        }

        Stats.Instance.TryAddItem(Constants.ANTIPOHMELIN);
    }
}

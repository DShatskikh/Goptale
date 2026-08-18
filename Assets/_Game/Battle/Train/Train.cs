using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Train : Enemy
{
    [SerializeField]
    private AudioClip _theme;
    
    private IEnumerator Start()
    {
        BattleManager.Instance.Enemies.Add(this);
        Relationship = -1;
        BattleManager.Instance.IsRun = false;
        
        Actions = new List<string>
        {
            "Оценить",
            "Лечь на землю",
            "Материться",
            "Наехать",
        };

        ActionAnswers = new List<string[]>
        {
            new[] {"Поезд - ??? АТК ??? ЗЩТ*Несётся прямо на вас."},
            new[] {"Вы вспомнили уроки <color=\"yellow\">ОБЖ</color> и легли на землю."},
            new[] {"Вы материтесь.*Но это ничего не дало.", "Кажется вам пизда."},
            new[] {"Вы попытались наехать на поезд.*Это был самый тупой поступок в вашей жизни."},
        };

        if (BattleManager.Instance.IsEnemySelected(this))
        {
            MusicManager.Instance.Play(_theme);
            BattleManager.MainText = "Поезд стремительно несётся прямо на вас.*Самое время вспомнить уроки ОБЖ";
        }
        
        yield return new WaitUntil(() => BattleManager.Instance.IsEnemyTurn);
        yield return new WaitForSeconds(1);

        if (BattleManager.Instance.SelectMainButtonIndex == 1 && BattleManager.Instance.SelectBazarIndex == 1)
        {
            Stats.Instance.TomaraCutscene = 8;
            StartCoroutine(AwaitAnimation());
            CoroutineRunner.Instance.StartCoroutine(BattleManager.Instance.AwaitExitMessage());
        }
        else
        {
            yield return AwaitAnimation();

            GameOver.Script = "Train";
            GameOver.Message = $"{Stats.Instance.Name} сохраняй четкость!\nИ не забывай уроки ОБЖ!";
            Heart.Instance.Damage(999);
        }
    }

    private IEnumerator AwaitAnimation()
    {
        var process = 0f;
        var startY = transform.position.y;
        var localScale = transform.localScale;
        
        while (process < 1)
        {
            process += Time.deltaTime / 2;
            transform.position = new Vector3(transform.position.x, Mathf.Lerp(startY, -1.45f, process), transform.position.z);
            transform.localScale = Vector3.Lerp(localScale, Vector2.one * 2, process);
            yield return null;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Narik : Enemy
{
    [SerializeField]
    private AudioClip _theme;

    [SerializeField]
    private GameObject[] _attacks;
    
    private Animator _animator;
    private TextBubble _textBubble;
    private bool _isAddedDefeatedEnemiesID;
    
    private IEnumerator Start()
    {
        _animator = GetComponent<Animator>();
        BattleManager.Instance.Enemies.Add(this);
        MusicManager.Instance.Play(_theme);
        
        Actions = new List<string>
        {
            "Чекнуть",
            "Утешить",
            "Послать",
        };

        ActionAnswers = new List<string[]>
        {
            new[] {"Нарик - АТК 3 ЗЩТ 4*Просто живёт эту жизнь."},
            new[] {"Вы утешили Нарика*Он раскурился."}, // Неплохо!... Прям ништяк!... Ахрененно! Прям не найс... Чекатище...
            new[] {"Вы пслали Нарика*Нарик этого не оценил*Кажется он достаёт револьвер"} // Меньше пиздежа, больше мозговой активности, Павел.
        };

        yield return null;
        
        if (BattleManager.Instance.IsEnemySelected(this))
            BattleManager.MainText = "Вы встретили Нарика.";
        // Сразу пощажен
        
        while (true)
        {
            yield return new WaitUntil(() => BattleManager.Instance.IsEnemyTurn);
            
            if (!IsActive)
                yield break; 
            
            BattleManager.MainText = "Вы встретили Нарика.";
            
            _textBubble = Instantiate(Resources.Load<TextBubble>("TextBubbleMini"));
            _textBubble.transform.position = transform.position + new Vector3(1.3f, 0.44f, 0); // 3.95f, 0.4400001f
            
            if (BattleManager.Instance.IsEnemySelected(this)
                && (BattleManager.Instance.SelectMainButtonIndex == 1 && BattleManager.Instance.SelectBazarIndex != 0))
            {
                if (BattleManager.Instance.SelectBazarIndex == 1)
                {
                    _textBubble.SetText("Неплохо!... Прям ништяк!... Ахрененно! Прям не найс... Чекатище...");
                }
                else if (BattleManager.Instance.SelectBazarIndex == 2)
                {
                    _textBubble.SetText("Валера настало твоё время!");
                }
            }
            
            if (!_textBubble.IsInit)
                _textBubble.SetText("Меньше пиздежа, больше мозговой активности");
            
            yield return new WaitUntil(() => _textBubble == null);

            if (BattleManager.Instance.IsEnemySelected(this))
            {
                var y = BattleManager.Instance.GetFrame.transform.position.y;
                BattleManager.Instance.GetFrame.transform.position = new Vector3(0f, -1.34f, 0);
                BattleManager.Instance.TargetSizeFrame = new Vector3(2.5f, 2.5f, 0);

                Instantiate(_attacks[0]);

                yield return new WaitForSeconds(10);

                BattleManager.Instance.GetFrame.transform.position = new Vector3(0f, y, 0);
                BattleManager.Instance.IsEnemyTurn = false;
            }
            else
            {
                yield return new WaitUntil(() => !BattleManager.Instance.IsEnemyTurn);
            }
        }
    }
    
    private void Update()
    {
        if (_isAddedDefeatedEnemiesID)
            return;
        
        if (Health <= 0)
        {
            _isAddedDefeatedEnemiesID = true;
            RandomEncounters.DefeatedEnemiesID.Add(Name);
            _animator.Play("Death");
        }
        else if (!IsActive)
        {
            _animator.Play("Mercy");
        }
    }
}


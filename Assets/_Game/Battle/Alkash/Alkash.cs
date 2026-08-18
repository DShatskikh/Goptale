using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Alkash : Enemy
{
    [SerializeField]
    private AudioClip _theme;

    [SerializeField]
    private GameObject _attack;
    
    private Animator _animator;
    private TextBubble _textBubble;
    private bool _isAddedDefeatedEnemiesID;

    public bool IsHealSpawn;
    
    private IEnumerator Start()
    {
        _animator = GetComponent<Animator>();
        
        BattleManager.Instance.Enemies.Add(this);
        MusicManager.Instance.Play(_theme);
        
        // Враг поддержки
        
        Actions = new List<string>
        {
            "Чекнуть",
            "Бухать",
            "Не бухать",
        };

        ActionAnswers = new List<string[]>
        {
            new[] {$"Алкаш - АТК 7 ЗЩТ 5*Хочет с вами выпить."},
            new[] {"Вы наливаете себе стопку...*(Ловите зелённые бутылки)."},
            new[] {"Вы говорите что не пьёте*Вам не поверили."}
        };

        BattleManager.MainText = "Алкаш культурно отдыхает.";
        
        while (true)
        {
            yield return new WaitUntil(() => BattleManager.Instance.IsEnemyTurn);

            IsHealSpawn = false;
            BattleManager.MainText = "Алкаш культурно отдыхает.";
            
            _textBubble = Instantiate(Resources.Load<TextBubble>("TextBubbleMini"));
            var y = 1.37f;
            _textBubble.transform.position = transform.position + new Vector3(1.3f, y, 0); // 3.95f, 0.4400001f
            
            if (BattleManager.Instance.IsEnemySelected(this)
                && (BattleManager.Instance.SelectMainButtonIndex == 1 && BattleManager.Instance.SelectBazarIndex != 0))
            {
                if (BattleManager.Instance.SelectBazarIndex == 1)
                {
                    _textBubble.SetText("Вот это я понимаю");
                    IsHealSpawn = true;
                }
                else if (BattleManager.Instance.SelectBazarIndex == 2)
                {
                    _textBubble.SetText("Никогда в жизни в это не поверю");
                }
            }

            if (!_textBubble.IsInit)
                _textBubble.SetText("Ты меня ува- жаешь?");
            
            Relationship++;
            yield return new WaitUntil(() => _textBubble == null);

            var attack = Instantiate(_attack);
            yield return new WaitForSeconds(10);
            Destroy(attack);
            
            if (Relationship >= 0)
                BattleManager.MainText = "Алкаш вас щадит.";
            
            BattleManager.Instance.IsEnemyTurn = false;
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

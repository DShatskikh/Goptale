using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Tamara : Enemy
{
    [SerializeField]
    private AudioClip _theme;

    [SerializeField]
    private GameObject[] _attacks;

    [SerializeField]
    private SpriteRenderer _view;
    
    [SerializeField]
    private Sprite[] _emotions;
    
    private int _mercy;
    private int _speakerCount;
    private Animator _animator;
    
    public float NextTurnTimer;
    public int Mercy => _mercy;

    private IEnumerator Start()
    {
        _animator = GetComponent<Animator>();
        TamaraCutscene_9.IsRunning = true;
        
        BattleManager.Instance.Enemies.Add(this);
        MusicManager.Instance.Play(_theme);
        
        Actions = new List<string>
        {
            "Чекнуть",
            "Базар"
        };

        ActionAnswers = new List<string[]>
        {
            new[] {"Тётя Тома - АТК 88 ЗЩТ 88.*Всегда знает, что для вас лучше."},
            new[] {"Вы не можете придумать что сказать."}
        };

        if (_mercy < 8)
        {
            BattleManager.MainText = "Тётя Тома преграждает дорогу!";
        }
        else if (_mercy < 16)
        {
            BattleManager.MainText = "Тётя Тома готовит магическую атаку.";
        }
        else
        {
            BattleManager.MainText = "...";
            MusicManager.Instance.Stop();
        }
        
        while (true)
        {
            yield return new WaitUntil(() => BattleManager.Instance.IsEnemyTurn);

            BattleManager.MainText = "Тётя Тома смотрит осуждающим взглядом.";
            
            var attackIndex = 0;
                
            BattleManager.Instance.TargetSizeFrame = new Vector3(3f, 1.75f, 0);
            
            if (BattleManager.Instance.SelectMainButtonIndex == 1)
            {
                if (BattleManager.Instance.SelectBazarIndex == 1)
                {
                    
                }
                else if (BattleManager.Instance.SelectBazarIndex == 2)
                {
                    if (_speakerCount == 1)
                    {
                        ActionAnswers[1] = new[]{"Вы снова пытаетесь придумать, что сказать, но..."};
                    } 
                    else if (_speakerCount == 2)
                    {
                        ActionAnswers[1] = new[]{"Иронично, но, похоже, разговор не является решением в данной ситуации."};
                        BattleManager.MainText = "Тётя Тома действует отстранённо.";
                    }
                    
                    _speakerCount++;
                }
            }
            else if (BattleManager.Instance.SelectMainButtonIndex == 3)
            {
                if (_mercy == 1)
                {
                    // смотрит в сторону
                    _view.sprite = _emotions[0];
                }
                else if (_mercy == 11)
                {
                    // смотрит в сторону и подняла брови
                    _view.sprite = _emotions[1];
                }
                else if (_mercy == 12)
                {
                    // смотрит на нас и подняла брови
                    _view.sprite = _emotions[2];
                }
                else if (_mercy == 15)
                {
                    // смотрит на нас и подняла брови и улыбнулась
                    _view.sprite = _emotions[3];
                }
                else if (_mercy == 16)
                {
                    // смотрит на нас и подняла брови
                    _view.sprite = _emotions[2];
                }
                else if (_mercy == 17)
                {
                    // смотрит в сторону и подняла брови
                    _view.sprite = _emotions[1];
                }
                else if (_mercy == 18)
                {
                    // смотрит в сторону и подняла брови и недовольная
                    _view.sprite = _emotions[4];
                }
                else if (_mercy == 19)
                {
                    // смотрит в сторону и подняла брови и улыбнулась
                    _view.sprite = _emotions[5];
                }
                else if (_mercy == 21)
                {
                    // смотрит на нас и подняла брови и улыбнулась
                    _view.sprite = _emotions[3];
                }
                else if (_mercy == 23)
                {
                    // смотрит в сторону и подняла брови
                    _view.sprite = _emotions[1];
                }
                else if (_mercy == 24)
                {
                    // смотрит на нас разочарование
                    _view.sprite = _emotions[6];
                }

                var y = 1.42f;
                var _textBubble = Instantiate(Resources.Load<TextBubble>("TextBubbleMini"));
                _textBubble.transform.position = transform.position + new Vector3(1.3f, y, 0);
                _textBubble.SetText(_mercy switch
                {
                    0 => ".....",
                    1 => ".....\n.....",
                    2 => ".....\n.....\n.....",
                    3 => "?..",
                    4 => "Ты дурачок?",
                    5 => "Я тебя всё равно не пущу!",
                    6 => "Ты дурачок или как?",
                    7 => "Просто уйди.",
                    8 => "Ты не пройдёшь!",
                    9 => "Хватит скулить.",
                    10 => "Уйди!",
                    11 => "...",
                    12 => "...\n...",
                    13 => "знаю, ты хочешь домой, но...",
                    14 => "Но пожалуйста, иди наверх...",
                    15 => "Обещаю что хорошо позабочусьо тебе.",
                    16 => "Знаю, у нас многого нет, но...",
                    17 => "У нас может быть хорошая жизнь тут.",
                    18 => "Почему ты всё усложняешь?",
                    19 => "Пожалуйста иди наверх.",
                    20 => ".....",
                    21 => "ха-ха..",
                    22 => "Жалко, не правда ли? Я не могу спасти даже одного пацанчика.",
                    23 => "...",
                    24 => "Тебя можно понимать.",
                    _ => "..."
                });

                yield return new WaitUntil(() => _textBubble == null);
                
                if (_mercy == 24)
                {
                    _textBubble = Instantiate(Resources.Load<TextBubble>("TextBubbleMini"));
                    _textBubble.transform.position = transform.position + new Vector3(1.3f, y, 0);
                    _textBubble.SetText("Ты просто будешь несчастлив застрять здесь внизу.");
                    yield return new WaitUntil(() => _textBubble == null);
                    
                    _textBubble = Instantiate(Resources.Load<TextBubble>("TextBubbleMini"));
                    _textBubble.transform.position = transform.position + new Vector3(1.3f, y, 0);
                    _textBubble.SetText("В РУИНИНО ничего нет, кроме бутылок и ловушек.");
                    yield return new WaitUntil(() => _textBubble == null);
                    
                    _textBubble = Instantiate(Resources.Load<TextBubble>("TextBubbleMini"));
                    _textBubble.transform.position = transform.position + new Vector3(1.3f, y, 0);
                    _textBubble.SetText("Будет неправильно жить вместе подобному этому.");
                    yield return new WaitUntil(() => _textBubble == null);
                    
                    _textBubble = Instantiate(Resources.Load<TextBubble>("TextBubbleMini"));
                    _textBubble.transform.position = transform.position + new Vector3(1.3f, y, 0);
                    _textBubble.SetText("Моё ожидание.\nМоё одиночество.\nМои страхи.");
                    yield return new WaitUntil(() => _textBubble == null);
                    
                    _textBubble = Instantiate(Resources.Load<TextBubble>("TextBubbleMini"));
                    _textBubble.transform.position = transform.position + new Vector3(1.3f, y, 0);
                    _textBubble.SetText("Ради тебя пацанчик.\nЯ отодвину их назад.");
                    yield return new WaitUntil(() => _textBubble == null);

                    Stats.Instance.IsGenocide = false;
                    TamaraCutscene_9.IsRunning = false;
                    Stats.Instance.TomaraCutscene = 15;
                    
                    yield return  BattleManager.Instance.AwaitExit();
                    yield break;
                }
                
                _mercy++;
            }

            if (_mercy >= 2 && _mercy < 14) // и мало хп не атакуем игрока в полную силу
            {
                
            }
            else if (_mercy >= 14) // вообще перестает атаковать
            {
                
            }
            
            NextTurnTimer = 10f;
            
            if (_mercy >= 12)
            {
                attackIndex = 3;
                NextTurnTimer = 5;
            }
            else if (BattleManager.Instance.CountTurn % 3 == 1)
            {
                attackIndex = 1;

                if (Stats.Instance.HP <= 5)
                {
                    attackIndex = 0;
                }
            }
            else if (BattleManager.Instance.CountTurn % 3 == 2)
            {
                attackIndex = 2;

                if (Stats.Instance.HP <= 5)
                {
                    attackIndex = 0;
                }
            }
            else
            {
                attackIndex = 0;
            }

            if (_mercy < 15)
            {
                var attack = Instantiate(_attacks[attackIndex]);

                while (NextTurnTimer > 0)
                {
                    NextTurnTimer -= Time.deltaTime;
                    yield return null;
                }
            
                Destroy(attack);
            }
            
            BattleManager.Instance.IsEnemyTurn = false;
        }
    }
    
    private void Update()
    {
        Debug.Log(Stats.Instance.IsGenocide);
        
        if (Health <= 0)
        {
            _animator.Play("Death");
            Stats.Instance.IsTomaraDead = true;
            Stats.Instance.TomaraCutscene = 15;
        }
        else if (!IsActive)
        {
            // _animator.Play("Mercy");
        }
    }
}

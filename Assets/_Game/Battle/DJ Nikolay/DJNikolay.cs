using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class DJNikolay : Enemy
{
    private const string START_TEXT = "На сцену выходит DJ Николай.*Он убит в хлам после тусы.";

    [SerializeField]
    private AudioClip _theme;

    [SerializeField]
    private AudioSource _sfxDrink;
    
    [SerializeField]
    private GameObject[] _attacks;

    private TextBubble _textBubble;
    private Animator _animator;
    private int _state;

    private IEnumerator Start()
    {
        BattleManager.Instance.Enemies.Add(this);
        BattleManager.Instance.IsRun = false;
        BattleManager.MainText = "...";
        
        Actions = new List<string>
        {
            "Чекнуть",
            "Фанатеть",
            "Угрожать", 
            "Танцевать"
        };

        ActionAnswers = new List<string[]>
        {
            new[] {$"{Name} - 10 АТК 10 ЗЩ*Главный диджей Подзёмкино."},
            new[] {"Вы говорите что вы его фанат и выкрикиваете случайное название песни.", "Вы не угадали.*Теперь DJ Николай считает вас позером.", "Но ему всё-равно приятно."}, 
            // хе... // Выглядет немного лучше // Я просто отягощая тебя // Выглядет немного лучше
            new[] {"Вы угрожайте Томарой.*Кажется это не работает."}, 
            new[] {"Вы делаете четкие пацанские движения.*DJ Николай это оценил."} // Вы рассказали Напстаблуку шутку. хе хе... // Аплодисменты, кажется, снова улучшили настроение Напстаблука.
            // Напстаблук хочет кое-что показать вам. // дай попробую ...
            // я называю это "опрятно блук"
            // тебе нравится?..
            
            // Напстаблук с нетерпением ожидает вашего ответа.
            
            // ой ей...
        };

        MusicManager.Instance.Play(_theme);
        BattleManager.MainText = START_TEXT;

        while (true)
        {
            yield return new WaitUntil(() => BattleManager.Instance.IsEnemyTurn);

            if (BattleManager.Instance.CountTurn != 2)
            {
                _textBubble = Instantiate(Resources.Load<TextBubble>("TextBubbleMini"));
                _textBubble.transform.position = transform.position + new Vector3(1.3f, 0.44f, 0);
            }
            
            BattleManager.MainText = START_TEXT;

            if (BattleManager.Instance.SelectMainButtonIndex == 1)
            {
                if (BattleManager.Instance.SelectBazarIndex == 1)
                {
                    BattleManager.MainText = "DJ Николай рад любым фанатам.";
                    Relationship++;

                    if (_state == 2)
                    {
                        yield return BattleManager.Instance.AwaitExitMessage();
                    }
                }
                else if (BattleManager.Instance.SelectBazarIndex == 2)
                {
                    BattleManager.MainText = "Кажется он не знает кто такая Томара.";
                    Relationship = -3;
                    
                    if (_state == 2)
                    {
                        _state = 0;
                        Relationship = -3;
                    }
                }
                else if (BattleManager.Instance.SelectBazarIndex == 3)
                {
                    BattleManager.MainText = "DJ Николай оценил ваши движения.";
                    Relationship += 2;
                    
                    if (_state == 2)
                    {
                        yield return BattleManager.Instance.AwaitExitMessage();
                    }
                }
            }
            else if (BattleManager.Instance.SelectMainButtonIndex == 3)
            {
                Relationship++;
            }
            else if (BattleManager.Instance.SelectMainButtonIndex == 0)
            {
                Relationship--;
                
                if (_state == 2)
                {
                    _state = 0;
                    Relationship = -3;
                }
            }
            
            if (Relationship >= 0)
            {
                Relationship = -1;
            }
            
            if (BattleManager.Instance.CountTurn == 1)
            {
                _textBubble.SetText("ааа... Я убит в хлам");
            }
            else if (BattleManager.Instance.CountTurn == 2)
            {
                // Я забыл как делать свой ход
            }
            else if (BattleManager.Instance.CountTurn == 3)
            {
                _textBubble.SetText("ты ваще кто?");
            }
            else if (BattleManager.Instance.CountTurn == 4)
            {
                _textBubble.SetText("как бошка трещит");
            }
            else if (BattleManager.Instance.CountTurn >= 5 && _state == 0 && Relationship >= -1)
            {
                _state = 1;
                _textBubble.SetText("где моё пивко?");
                BattleManager.MainText = "DJ Николай хочет кое-что вам показать.";
            }
            else if (Relationship >= -1 && _state == 1)
            {
                _state = 2;
                _textBubble.SetText("фух... я нашел пивко"); // 
                yield return new WaitUntil(() => _textBubble == null);
                
                // _textBubble = Instantiate(Resources.Load<TextBubble>("TextBubbleMini"));
                // _textBubble.transform.position = transform.position + new Vector3(1.3f, 0.44f, 0);
                // _textBubble.SetText("лан, зацени прикол");
                // yield return new WaitUntil(() => _textBubble == null);
                
                GetComponent<Animator>().Play("Dancin");
                yield return new WaitForSeconds(4.5f);
                
                _textBubble = Instantiate(Resources.Load<TextBubble>("TextBubbleMini"));
                _textBubble.transform.position = transform.position + new Vector3(1.3f, 0.44f, 0);
                _textBubble.SetText("фух... %отпустило\n...");
                yield return new WaitUntil(() => _textBubble == null);
                
                _textBubble = Instantiate(Resources.Load<TextBubble>("TextBubbleMini"));
                _textBubble.transform.position = transform.position + new Vector3(1.3f, 0.44f, 0);
                _textBubble.SetText("Знаешь что это значит?");
                yield return new WaitUntil(() => _textBubble == null);
                
                _textBubble = Instantiate(Resources.Load<TextBubble>("TextBubbleMini"));
                _textBubble.transform.position = transform.position + new Vector3(1.3f, 0.44f, 0);
                _textBubble.SetText("Время продолжить кутить!");
                yield return new WaitUntil(() => _textBubble == null);
                
                GetComponent<Animator>().Play("Dancin 1");
                yield return new WaitForSeconds(2.5f);
                
                yield return BattleManager.Instance.AwaitExitMessage();
                yield break;
            }
            else
            {
                _textBubble.SetText("тяжело быть знаменитым");
            }
            
            if (BattleManager.Instance.SelectMainButtonIndex != 2)
                yield return new WaitUntil(() => _textBubble == null);

            if (Relationship < -1 || _state == 0)
            {
                yield return new WaitForSeconds(1);
                
                if (BattleManager.Instance.CountTurn == 2)
                {
                    Instantiate(_attacks[0]);
                    yield return new WaitForSeconds(7);
                }
                else
                {
                    if (BattleManager.Instance.CountTurn == 1)
                    {
                        Instantiate(_attacks[1]);
                    }
                    else if (BattleManager.Instance.CountTurn == 3)
                    {
                        Instantiate(_attacks[2]);
                    }
                    else
                    {
                        Instantiate(_attacks[Random.Range(1, _attacks.Length)]);
                    }
                    
                    yield return new WaitForSeconds(8);
                }
            }
            
            BattleManager.Instance.IsEnemyTurn = false;
        }
    }

    private void Update()
    {
        if (Health <= 0)
        {
            Stats.Instance.DJNikolayState = 2;
        }
        else if (IsActive)
        {
            Stats.Instance.DJNikolayState = 1;
        }
    }

    private void Drink()
    {
        _sfxDrink.Play();
    }
}

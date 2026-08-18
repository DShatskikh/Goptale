using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public sealed class Gopnik : Enemy
{
    [SerializeField]
    private AudioClip _theme;

    [SerializeField]
    private GopnikAttack_3 _gopnikAttack1;
    
    [SerializeField]
    private GopnikAttack_3 _gopnikAttack2;
    
    [SerializeField]
    private GopnikAttack_3 _gopnikAttack3;
    
    [SerializeField]
    private GopnikAttack_3 _gopnikNarikAttack;
    
    private TextBubble _textBubble;
    private Animator _animator;
    private bool _isNotGenocide;
    private bool _isAddedDefeatedEnemiesID;
    private bool _isTrio;
    private int _rub;
    
    private IEnumerator Start()
    {
        _animator = GetComponent<Animator>();
        BattleManager.Instance.Enemies.Add(this);
        Relationship = -2;
        _isTrio = BattleManager.Instance.Enemies.Count == 3;

        if (_isTrio)
        {
            BattleManager.Instance.IsRun = false;
        }
        
        Actions = new List<string>
        {
            "Чекнуть",
            "Базар",
            "Угрожать",
            "Откупиться\n(5 РУБ)"
        };

        ActionAnswers = new List<string[]>
        {
            new[] {"Гопник - 4 АТК 5 ЗЩТ.*Хочет забрать у вас все предметы."},
            new[] {"Вы базарите с Гопником по понятиям.", "Кажется он проникся вашей речью.*Но еще сомневается."},
            new[] {"Вы угрожайте Тамарой.*Кажется это не работает."},
            new[] {"Вы откупились.*Гопник больше не хочет драться."}
        };

        yield return null;
        
        if (BattleManager.Instance.IsEnemySelected(this))
        {
            MusicManager.Instance.Play(_theme);
            BattleManager.MainText = "Вас гопстопнули.";

            if (BattleManager.Instance.Enemies.Count == 2 && BattleManager.Instance.Enemies[1].Name == "Нарик")
            {
                BattleManager.MainText = "Вас гопстопнули.*Вы встретили Нарика.";
            }
        }
        
        _isTrio = BattleManager.Instance.Enemies.Count == 3;
        var isSeveral = BattleManager.Instance.Enemies.Count > 1;
        
        if (_isTrio)
        {
            ActionAnswers[1] = new[] { "Вы пытаетесь базарить с Гопником по понятиям.", "Но из-за плохой компании он не проникся вашей речью." };
            ActionAnswers[2] = new[] { "Вы угрожайте Тамарой.*Кажется это не работает." };
            ActionAnswers[3] = new[] { "Вы попытались откупиться.*Но у вас мало денег."};
        }
        
        while (true)
        {
            yield return new WaitUntil(() => BattleManager.Instance.IsEnemyTurn);
            if (Actions.Count == 3 && Relationship >= 0)
            {
                Actions.Remove(Actions[3]);
            }
            
            if (!IsActive)
                yield break;
            
            _textBubble = Instantiate(Resources.Load<TextBubble>("TextBubbleMini"));
            _textBubble.transform.position = transform.position + new Vector3(1.3f, 0.44f, 0); // 3.95f, 0.4400001f

            if (BattleManager.Instance.SelectMainButtonIndex != 0)
            {
                _isNotGenocide = true;
            }
            else
            {
                _isNotGenocide = false;
            }
            
            var actives = 0;
            var kills = 0;

            for (int i = 0; i < BattleManager.Instance.Enemies.Count; i++)
            {
                if (BattleManager.Instance.Enemies[i].IsActive)
                    actives++;
                else if (BattleManager.Instance.Enemies[i].Health <= 0)
                    kills++;
            }

            if (isSeveral)
            {
                if (actives == 1 && kills > 0)
                {
                    ActionAnswers[0] = new[] { "Гопник - 4 АТК 5 ЗЩТ.*Больше не хочет с вами драться." };
                    ActionAnswers[1] = new[] { "Вы базарите с Гопником по понятиям.", "Он с вами согласен и не хочет драться." };
                    ActionAnswers[2] = new[] { "Вы угрожайте Тамарой.*Кажется это работает." };
                    ActionAnswers[3] = new[] { "Вы попытались откупиться.*Гопник не хочет брать у вас деньги."};
                    Relationship = 10;
                }
            }
            
            if (BattleManager.Instance.IsEnemyDead)
            {
                _textBubble.SetText("Земля тебе бетоном...");
                BattleManager.MainText = "Гопники грустят что вы ушатали их друга.*Их защита понижена.";
                Defence = -10;
            }
            else if (BattleManager.Instance.IsEnemySelected(this) 
                     && (BattleManager.Instance.SelectMainButtonIndex == 1 && BattleManager.Instance.SelectBazarIndex != 0))
            {
                if (BattleManager.Instance.SelectBazarIndex == 1)
                {
                    if (!_isTrio)
                    {
                        _textBubble.SetText("Ну так-то да");
                        ActionAnswers[1] = new[] { "Вы базарите с Гопником по понятиям.", "Кажется он проникся вашей речью." };
                    }
                    else
                    {
                        if (actives == 1)
                        {
                            _textBubble.SetText("Ты прав отпусти пожалуйста");
                        }
                        else
                        {
                            _textBubble.SetText("Ну так-то да, но пацаны не оценят");
                        }
                    }
                    
                    BattleManager.MainText = "Гопник ждёт дальнейшего хода.";
                        
                    if (!_isTrio)
                        Relationship++;
                }
                else if (BattleManager.Instance.SelectBazarIndex == 2)
                {
                    if (_isTrio && actives == 1)
                    {
                        _textBubble.SetText("Мне очень страшно");
                    }
                    else
                    {
                        _textBubble.SetText("Эээ... да я её разнесу"); 
                    }
                    
                    BattleManager.MainText = "Гопник храбрится.";
                }
                else if (BattleManager.Instance.SelectBazarIndex == 3)
                {
                    if (_isTrio)
                    {
                        if (actives == 1)
                        {
                            _textBubble.SetText("Не надо денег лучше отпусти");
                        }
                        else
                        {
                            _textBubble.SetText("Хуле так мало?!");
                            ActionAnswers[3] = new[] { "Вы не смогли откупиться." };
                        }
                    }
                    else
                    {
                        Relationship = 10;

                        if (Relationship < 0)
                        {
                            _rub += RUB;
                        
                            if (Stats.Instance.RUB >= 5)
                            {
                                _textBubble.SetText("Береги себя");
                                ActionAnswers[3] = new[] { "Вы отдали гопнику 5РУБ.*Он больше не хочет драться." };
                                _rub += 5;
                                RUB = 0;
                            }
                            else if (Stats.Instance.RUB != 0)
                            {
                                _textBubble.SetText("Береги себя");
                                ActionAnswers[3] = new[] { "Вы отдали гопнику все деньги.*Он больше не хочет драться." };
                                _rub += Stats.Instance.RUB;
                                RUB = 0;
                            }
                            else
                            {
                                _textBubble.SetText("Нищеброд");
                                ActionAnswers[3] = new[] { "Вы отдали гопнику все деньги.*Но у вас ничего нету.", "Гопник больше не хочет драться." };
                                RUB = 0;
                            }
                        
                            Stats.Instance.RUB -= 5;

                            if (Stats.Instance.RUB < 0)
                                Stats.Instance.RUB = 0;

                            // yield return BattleManager.Instance.AwaitExitMessage();
                            // yield break;
                        }
                        else
                        {
                            _textBubble.SetText("Иди давай");
                            ActionAnswers[3] = new[] { "Ничего не произошло.*Гопник не хочет драться." };
                            RUB = 0;
                        }
                    }
                }
            }
            else
            {
                if (BattleManager.Instance.IsEnemySelected(this))
                {
                    BattleManager.MainText = "Вас гопстопнули.";
                }

                if (actives == 1 && _isTrio)
                {
                    _textBubble.SetText("Не бей лучше обосы");
                }
                else
                {
                    _textBubble.SetText(new[]{"Какого цвета у тебя подкладка в кошельке дай позырить?", "Пятки поднимаешь, пацанов не уважаешь"}[Random.Range(0, 2)]);
                }
            }

            if (Relationship >= 0 && IsActive)
                BattleManager.MainText = "Гопник не хочет с вами драться.";
            
            yield return new WaitUntil(() => _textBubble == null);

            if (BattleManager.Instance.CountTurn >= 5)
            {
                Relationship = 0;
            }
            
            if (BattleManager.Instance.CountTurn >= 3 
                && BattleManager.Instance.Enemies.Count == 3
                && BattleManager.Instance.Enemies[0].IsActive 
                && BattleManager.Instance.Enemies[1].IsActive
                && BattleManager.Instance.Enemies[2].IsActive
                && _isNotGenocide)
            {
                Debug.Log("FFFF");

                if (BattleManager.Instance.IsEnemySelected(this))
                {
                    Debug.Log("Spawn Tomara");
                    var tomara = Instantiate(Resources.Load<GameObject>("Tomara Disapprove"));
                    tomara.transform.position = new Vector3(10f, tomara.transform.position.y, 0);
                    MusicManager.Instance.Stop();
                    
                    var speed = 2;
                    
                    while (BattleManager.Instance.Enemies[0].transform.position != new Vector3(-5.22f, 2)
                           || BattleManager.Instance.Enemies[1].transform.position != new Vector3(-2.41f, 2)
                           || BattleManager.Instance.Enemies[2].transform.position != new Vector3(0.4000002f, 2))
                    {
                        BattleManager.Instance.Enemies[0].transform.position = Vector2.MoveTowards(
                            BattleManager.Instance.Enemies[0].transform.position, 
                            new Vector3(-5.22f, 2), Time.deltaTime * speed);
                        
                        BattleManager.Instance.Enemies[1].transform.position = Vector2.MoveTowards(
                            BattleManager.Instance.Enemies[1].transform.position, 
                            new Vector3(-2.41f, 2), Time.deltaTime * speed);
                        
                        BattleManager.Instance.Enemies[2].transform.position = Vector2.MoveTowards(
                            BattleManager.Instance.Enemies[2].transform.position, 
                            new Vector3(0.4000002f, 2), Time.deltaTime * speed);
                        
                        yield return null;
                    }
                    
                    while (tomara.transform.position != new Vector3(5.77f, -0.05000019f))
                    {
                        tomara.transform.position = Vector2.MoveTowards(tomara.transform.position, 
                            new Vector3(5.77f, -0.05000019f), Time.deltaTime);
                        
                        yield return null;
                    }
                    
                    MusicManager.Instance.Play(Resources.Load<AudioClip>("Fallen Down"));
                    
                    var textBox = Instantiate(Resources.Load<TextBubble>("TextBubble Left"));
                    textBox.transform.position = new Vector3(1.85f, 3.68f, 0);
                    textBox.SetText("Опять вы гопота мобилы отжимаете");
                    yield return new WaitUntil(() => textBox == null);
                    
                    textBox = Instantiate(Resources.Load<TextBubble>("TextBubble Left"));
                    textBox.transform.position = new Vector3(1.85f, 3.68f, 0);
                    textBox.GetComponent<SpriteRenderer>().flipX = true;
                    textBox.SetText("Я всё вашим мамкам расскажу");
                    yield return new WaitUntil(() => textBox == null);
                    
                    textBox = Instantiate(Resources.Load<TextBubble>("TextBubbleMini"));
                    textBox.transform.position = new Vector3(1.9f, 2.3f);
                    textBox.SetText("Не надо Тётя Тома");
                    yield return new WaitUntil(() => textBox == null);
                    
                    textBox = Instantiate(Resources.Load<TextBubble>("TextBubble Left"));
                    textBox.transform.position = new Vector3(1.85f, 3.68f, 0);
                    textBox.GetComponent<SpriteRenderer>().flipX = true;
                    textBox.SetText("А ну разбежались в страхе!");
                    yield return new WaitUntil(() => textBox == null);

                    speed = 5;
                    
                    while (BattleManager.Instance.Enemies[0].transform.position != new Vector3(-15.23f, 2))
                    {
                        BattleManager.Instance.Enemies[0].transform.position = Vector2.MoveTowards(
                            BattleManager.Instance.Enemies[0].transform.position, 
                            new Vector3(-15.23f, 2), Time.deltaTime * speed);
                        
                        BattleManager.Instance.Enemies[1].transform.position =  Vector2.MoveTowards(
                            BattleManager.Instance.Enemies[1].transform.position, 
                            new Vector3(-12.42f, 2), Time.deltaTime * speed);
                        
                        BattleManager.Instance.Enemies[2].transform.position = Vector2.MoveTowards(
                            BattleManager.Instance.Enemies[2].transform.position, 
                            new Vector3(-9.61f, 2), Time.deltaTime * speed);
                        
                        yield return null;
                    }
                    
                    tomara.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Toriel");
                    yield return new WaitForSeconds(1);
                    
                    while (tomara.transform.position != new Vector3(0f, -0.05000019f))
                    {
                        tomara.transform.position = Vector2.MoveTowards(tomara.transform.position, 
                            new Vector3(0f, -0.05000019f), Time.deltaTime * 2);
                        
                        yield return null;
                    }

                    foreach (var enemy in BattleManager.Instance.Enemies)
                    {
                        enemy.IsMercy = true;
                    }
                    
                    Stats.Instance.TomaraCutscene = 4;
                    yield return BattleManager.Instance.AwaitExit();
                }

                yield break;
            }
            else if (BattleManager.Instance.IsEnemySelected(this))
            {
                var activeCount = 0;

                foreach (var enemy in BattleManager.Instance.Enemies)
                {
                    if (enemy.IsActive)
                        activeCount++;
                }

                if (_isTrio && BattleManager.Instance.CountTurn == 1 && !Meta.Instance.IsCompleteTutorial)
                {
                    Instantiate(Resources.Load<GameObject>("Arrows Hint"));
                    yield return new WaitForSeconds(5);

                    Meta.Instance.IsCompleteTutorial = true;
                    SaveSystem.MetaSave();
                }
                
                if (activeCount == 1)
                {
                    if (!_isTrio)
                    {
                        Instantiate(_gopnikAttack1);
                        yield return new WaitForSeconds(4);
                    }
                }
                else if (activeCount == 2)
                {
                    if (BattleManager.Instance.Enemies[1].Name == "Нарик")
                    {
                        Instantiate(_gopnikNarikAttack);
                        yield return new WaitForSeconds(8);
                    }
                    else
                    {
                        Instantiate(_gopnikAttack2);
                        yield return new WaitForSeconds(5);
                    }
                }
                else
                {
                    Instantiate(_gopnikAttack3);
                    yield return new WaitForSeconds(8);
                }
            }
            else
            {
                yield return new WaitUntil(() => !BattleManager.Instance.IsEnemyTurn);
            }
            
            if (!_isTrio)
            {
                if (BattleManager.Instance.CountTurn > 3 || Health < MaxHealth / 2)
                {
                    Relationship = 5;
                }
            }
            
            if (BattleManager.Instance.IsEnemySelected(this))
            {
                BattleManager.Instance.IsEnemyTurn = false;
                Debug.Log(name);
            }
            
            yield return null;
        }
    }

    private void Update()
    {
        if (_isAddedDefeatedEnemiesID)
            return;
        
        if (Health <= 0)
        {
            _isAddedDefeatedEnemiesID = true;
            
            if (!_isTrio)
                RandomEncounters.DefeatedEnemiesID.Add(Name);
            
            _animator.Play("Death");
            
            if (BattleManager.Instance.Enemies.Count == 3
                && !BattleManager.Instance.Enemies[0].IsActive 
                && !BattleManager.Instance.Enemies[1].IsActive
                && !BattleManager.Instance.Enemies[2].IsActive)
            {
                Stats.Instance.TomaraCutscene = 4;
            }

            if (_rub != 0)
            {
                RUB += _rub;
                _rub = 0;
            }
        }
        else if (!IsActive)
        {
            _animator.Play("Mercy");
        }
    }
}

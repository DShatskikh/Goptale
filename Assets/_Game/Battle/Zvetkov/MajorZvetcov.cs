using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class MajorZvetcov : Enemy
{
    [SerializeField]
    private AudioClip _majorZvetcovVoice;

    [SerializeField]
    private AudioClip _tomaraVoice;

    [SerializeField]
    private AudioClip _tomaraTheme;

    [SerializeField]
    private AudioClip _theme;
    
    private TextBubble _textBubble;
    private Animator _animator;

    private IEnumerator Start()
    {
        BattleManager.Instance.Enemies.Add(this);
        MusicManager.Instance.Play(_theme);
        
        _animator = GetComponent<Animator>();
        
        BattleManager.MainText = "Майор Цветков просит показать ваши документы";
        BattleManager.Instance.TargetSizeFrame = BattleManager.Instance.AttackFrameSize_1;
        BattleManager.Instance.IsSkipPlayerTurn = true;
        
        Actions = new List<string>()
        {
            "Дать документы"
        };
        
        ActionAnswers = new List<string[]>()
        {
            new[] {"Вы дали паспорт четкого пацана"}
        };
        
        yield return new WaitForSeconds(1);

        TextBubble.AudioClip = _majorZvetcovVoice;
        _textBubble = Instantiate(Resources.Load<TextBubble>("TextBubble"));
        _textBubble.SetText("Гражданин\n%гражданин!");

        yield return new WaitUntil(() => _textBubble == null);
        
        TextBubble.AudioClip = _majorZvetcovVoice;
        _textBubble = Instantiate(Resources.Load<TextBubble>("TextBubble"));
        _textBubble.SetText("Я участковый Майор Цветков");
        
        yield return new WaitUntil(() => _textBubble == null);
        
        TextBubble.AudioClip = _majorZvetcovVoice;
        _textBubble = Instantiate(Resources.Load<TextBubble>("TextBubble"));
        _textBubble.SetText("Лежим в неположенном месте");
        
        yield return new WaitUntil(() => _textBubble == null);
        
        TextBubble.AudioClip = _majorZvetcovVoice;
        _textBubble = Instantiate(Resources.Load<TextBubble>("TextBubble"));
        _textBubble.SetText("Ваши документы");
        
        yield return new WaitUntil(() => _textBubble == null);
        
        BattleManager.Instance.IsEnemyTurn = false;
        
        BattleManager.Instance.IsProhibitionSelectMain = true;
        BattleManager.Instance.SelectMainButtonIndex = 1;
        
        yield return new WaitUntil(() => BattleManager.Instance.IsEnemyTurn);
        
        Heart.Instance.gameObject.SetActive(true);
        
        TextBubble.AudioClip = _majorZvetcovVoice;
        _textBubble = Instantiate(Resources.Load<TextBubble>("TextBubble"));
        _textBubble.SetText("Ага %значит вы еще и незаконный мигрант");
        
        yield return new WaitUntil(() => _textBubble == null);
        
        TextBubble.AudioClip = _majorZvetcovVoice;
        _textBubble = Instantiate(Resources.Load<TextBubble>("TextBubble"));
        _textBubble.SetText("Пройдемте-ка в отделение");
        
        yield return new WaitUntil(() => _textBubble == null);
        
        yield return new WaitForSeconds(2);

        TextBubble.AudioClip = _tomaraVoice;
        var textBubble = Instantiate(Resources.Load<TextBubble>("TextBubble Left"));
        textBubble.transform.position = new Vector3(4.96f, 3.68f);
        textBubble.SetText("Эээй! %это я тут крышую %ПОНЯЛ?!");
        
        yield return new WaitUntil(() => textBubble == null);
        _animator.Play("MojorZvetcovCutscene_1");
        
        yield return new WaitForSeconds(2);
        MusicManager.Instance.Play(_tomaraTheme);
        
        yield return new WaitUntil(() => _animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1);
        yield return new WaitForSeconds(2);

        CoroutineRunner.Instance.StartCoroutine(BattleManager.Instance.AwaitExit());
        Debug.Log("Конец");
    }

    private void Update()
    {
        if (_textBubble && _textBubble.IsAnimated)
        {
            _animator.SetBool("IsSpeak", true);
        }
        else
        {
            _animator.SetBool("IsSpeak", false);
        }
    }

    public void DamagePlay()
    {
        GetComponent<AudioSource>().Play();
    }
}

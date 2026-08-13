using System.Collections;
using UnityEngine;

public sealed class TomaraArcher : Usable
{
    [SerializeField]
    private GameObject _armchair1;
    
    [SerializeField]
    private GameObject _tomara;

    private int _select;
    
    public override void Use()
    {
        Fedya.Instance.enabled = false;
        CoroutineRunner.Instance.StartCoroutine(AwaitUse());
    }
    
    private IEnumerator AwaitUse()
    { 
        SelectionWindow selectDialogue = null;
        
        if (_select == 0)
        {
            yield return DialogueWindow.StartDialogue(new [] {
                "\\T1Уже проснулся?",
                "\\T1Я так рада что ты здесь со мной.",
            });
            
            _select = 1;
        }

        if (_select == 1)
        {
            selectDialogue = SelectionWindow.StartDialogue($"В чём дело?", "Ничего", "Когда я могу пойти домой?");
            yield return new WaitUntil(() => selectDialogue == null);
            
            if (!SelectionWindow.IsRight)
            {
                yield return DialogueWindow.StartDialogue(new [] {
                    "\\T1Вот и хорошо.",
                });
                
                Fedya.Instance.enabled = true;
                yield break;
            }
            else
            {
                _select = 2;
            }
        }

        if (_select == 2)
        {
            yield return DialogueWindow.StartDialogue(new [] {
                "\\T1Что?*Теперь... ЭТО твой дом.",
                "\\T1Эм... хочешь послушать о чём я сейчас читаю?",
            });

            _select = 3;
        }

        if (_select == 3)
        {
            selectDialogue = SelectionWindow.StartDialogue($"Хочешь\nпослушать?", "Конечно", "Как покинуть РУИНИНО?");
            yield return new WaitUntil(() => selectDialogue == null);
            
            if (!SelectionWindow.IsRight)
            {
                yield return DialogueWindow.StartDialogue(new[]
                {
                    "\\T1Это книжка про улиток.",
                    "\\T1Улитки могут поглощать воду не только ртом, но и всеми клетками своего тела."
                });
                
                Fedya.Instance.enabled = true;
                yield break;
            }
            else
            {
                _select = 4;
            }
        }

        yield return DialogueWindow.StartDialogue(new[]
        {
            "\\T1Э-эм...",
            "\\T1Как насчет удивительного факта об улитках?",
        });

        selectDialogue = SelectionWindow.StartDialogue($"Интересно.", "Ага", "Как покинуть РУИНИНО");

        yield return new WaitUntil(() => selectDialogue == null);

        if (!SelectionWindow.IsRight)
        {
            yield return DialogueWindow.StartDialogue(new[]
            {
                "\\T1Улитки способны тащить за собой груз, который в 10 раз тяжелее их собственного веса."
            });
            
            Fedya.Instance.enabled = true;
            yield break;
        }
        else
        {
            yield return DialogueWindow.StartDialogue(new[]
            {
                "\\T1... Мне нужно кое-что сделать.*Побудь здесь."
            });
        }

        _armchair1.SetActive(true);
        _tomara.SetActive(true);
        _tomara.transform.localPosition = new Vector3(1.49f, -0.18f, 0);
        GetComponent<SpriteRenderer>().enabled = false;
        
        yield return new WaitForSeconds(1f);
        
        _tomara.GetComponent<Animator>().enabled = true;
        _tomara.GetComponent<Animator>().Play("Tomara Right Move");
        var END_X = 7.73f;
        var SPEED = 5;
        
        while (_tomara.transform.localPosition.x < END_X)
        {
            _tomara.transform.localPosition = Vector2.MoveTowards(_tomara.transform.localPosition,
                new Vector2(END_X, _tomara.transform.localPosition.y), Time.deltaTime * SPEED);

            yield return null;
        }
        
        _tomara.GetComponent<Animator>().Play("Tomara Down Move");
        var END_Y = -6.82f;
        
        while (_tomara.transform.localPosition.y != END_Y)
        {
            _tomara.transform.localPosition = Vector2.MoveTowards(_tomara.transform.localPosition,
                new Vector2(_tomara.transform.localPosition.x, END_Y), Time.deltaTime * SPEED);

            yield return null;
        }
        
        _tomara.GetComponent<Animator>().enabled = false;
        
        Fedya.Instance.enabled = true;
        Stats.Instance.TomaraCutscene = 13;
        
        while (_tomara.GetComponent<SpriteRenderer>().color.a > 0)
        {
            var color = _tomara.GetComponent<SpriteRenderer>().color;
            color.a -= Time.deltaTime;
            _tomara.GetComponent<SpriteRenderer>().color = color;
            
            yield return null;
        }
        
        _tomara.SetActive(false);
        gameObject.SetActive(false);
    }
}

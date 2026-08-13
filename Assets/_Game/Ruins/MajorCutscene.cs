using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class MajorCutscene : MonoBehaviour
{
    private static float _startTime;
    
    [SerializeField]
    private GameObject _zvetkov, _attack;
    
    [SerializeField]
    private SpriteRenderer _blacksmith;
   
    private bool _isStartSecretCutscene;
    
    private IEnumerator Start()
    {
        if (Stats.Instance.TomaraCutscene > 16)
        {
            _zvetkov.SetActive(false);
            GetComponent<Collider2D>().enabled = false;
        }
        
        yield return null;
    }

    private void Update()
    {
        if (_isStartSecretCutscene)
            return;
        
        if (Stats.Instance.TomaraCutscene == 17 && _startTime + 300 < GameTimer.Instance.Timer && !Stats.Instance.IsGenocide)
        {
            _isStartSecretCutscene = true;
            StartCoroutine(AwaitSecret());
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.GetComponent<Fedya>())
            return;
        
        StartCoroutine(AwaitTrigger());
    }

    private IEnumerator AwaitSecret()
    {
        Fedya.Instance.enabled = false;
        
        yield return DialogueWindow.StartDialogue(new [] {
            "\\Z1Вот я вернулся."
        });
        
        SceneManager.LoadScene(35);
    }

    private IEnumerator AwaitTrigger()
    {
        Fedya.Instance.enabled = false;
        
        if (!Stats.Instance.IsGenocide)
        {
            yield return DialogueWindow.StartDialogue(new [] {
                "\\Z1Гражданин\n%гражданин!",
                "\\Z1Наконец-то я вас поймал.",
                "\\Z1Вы обвиняйтесь по статьям 318 и 313 УК РФ.",
                "\\Z1Соучастие в нападении на полицейского и...",
                "\\Z1...сокрытие с места преступления.",
                "\\Z1Вы задержаны!%*И будете доставленны в отделение!",
                "\\Z4.%.%.",
                "\\Z4Кажется я забыл своё удостоверение.%*.%.%.",
                "\\Z4Я не могу вас задержать пока не покажу удостоверение.",
                "\\Z4Постойте тут я сейчас вернусь.",
            });
            
            _zvetkov.GetComponent<Animator>().Play("Down Move");
            var END_Y = 0.25f;
            var SPEED = 5;
        
            while (_zvetkov.transform.localPosition.y != END_Y)
            {
                _zvetkov.transform.localPosition = Vector2.MoveTowards(_zvetkov.transform.localPosition,
                    new Vector2(_zvetkov.transform.localPosition.x, END_Y), Time.deltaTime * SPEED);

                yield return null;
            }
        
            _zvetkov.GetComponent<Animator>().Play("Right Move");
            var END_X = 1.49f;
            SPEED = 3;
        
            while (_zvetkov.transform.localPosition.x != END_X)
            {
                _zvetkov.transform.localPosition = Vector2.MoveTowards(_zvetkov.transform.localPosition,
                    new Vector2(END_X, _zvetkov.transform.localPosition.y), Time.deltaTime * SPEED);

                yield return null;
            }
        
            _zvetkov.GetComponent<Animator>().Play("Down Move");
            END_Y = -6.02f;
            SPEED = 7;
        
            while (_zvetkov.transform.localPosition.y != END_Y)
            {
                _zvetkov.transform.localPosition = Vector2.MoveTowards(_zvetkov.transform.localPosition,
                    new Vector2(_zvetkov.transform.localPosition.x, END_Y), Time.deltaTime * SPEED);

                yield return null;
            }

            _startTime = GameTimer.Instance.Timer;
            Stats.Instance.TomaraCutscene = 17;
            Fedya.Instance.enabled = true;
            _zvetkov.SetActive(false);
            GetComponent<Collider2D>().enabled = false;
        }
        else
        {
            yield return DialogueWindow.StartDialogue(new [] {
                "\\Z1Наконец-то я вас поймал, гражданин Чирик!",
                "\\Z121 эпизод нападения, а так же...",
                "\\Z1соучастие в нападении на полицейского и...",
                "\\Z1...сокрытие с места преступления.",
                "\\Z1Вы задержаны!%*И будете доставленны в отделение!",
                "\\Z1Стойте где стоите.%*Сейчас я надену на вас наручники.",
            });
            
            _blacksmith.gameObject.SetActive(true);
            _zvetkov.transform.rotation = Quaternion.Euler(0f, 0f, 90f);
            yield return new WaitForSeconds(1f);
            _attack.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            _attack.SetActive(false);
            yield return new WaitForSeconds(0.5f);

            var alpha = 1f;

            while (alpha > 0)
            {
                alpha -= Time.deltaTime;
                yield return null;
                _blacksmith.color = new Color(0f, 0f, 0f, alpha);
            }
            
            _blacksmith.gameObject.SetActive(false);
            Fedya.Instance.enabled = true;
            Stats.Instance.TomaraCutscene = 17;
            GetComponent<Collider2D>().enabled = false;
        }
    }
}

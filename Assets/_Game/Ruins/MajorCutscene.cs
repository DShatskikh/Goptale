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

    [SerializeField]
    private AudioClip _music;
    
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
        MusicManager.Instance.Play(_music);
        
        if (!Stats.Instance.IsGenocide)
        {
            yield return DialogueWindow.StartDialogue(new [] {
                "\\Z1Гражданин\n%гражданин!",
                "\\Z1Я участковый Майор Цветков.",
                "\\Z1Вы обвиняйтесь по статьям 318 и 313 УК Подзёмкино.",
                "\\Z1Соучастие в нападении на полицейского.",
                "\\Z1А также в сокрытии с места преступления.",
                "\\Z1Вы задержаны!",
                "\\Z1.%.%.",
                "\\Z4Блять.",
                "\\Z4Я забыл своё удостоверение.",
                "\\Z4По протоколу я не могу вас задержать пока не покажу удостоверение.",
                "\\Z1Постойте пожалуйста тут я сейчас вернусь.",
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
            MusicManager.Instance.Stop();
        }
        else
        {
            yield return DialogueWindow.StartDialogue(new [] {
                "\\Z1Я участковый Майор Цветков.",
                "\\Z1Вот вы и попались гражданин Чирик!",
                "\\Z1Вы обвиняйтесь по статьям 318, 313 и 162 УК Подзёмкино.",
                "\\Z1Соучастие в нападении на полицейского.",
                "\\Z1А также в сокрытии с места преступления.",
                "\\Z1И 13 эпизодах нападения.",
                "\\Z1Вы задержаны!",
                "\\Z1За вас мне дадут подполковника!%*Нет полковника!",
                "\\Z1Стойте где стоите.%*Сейчас я надену на вас наручники.",
            });
            
            MusicManager.Instance.Stop();
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

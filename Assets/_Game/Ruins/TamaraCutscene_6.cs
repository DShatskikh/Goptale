using System.Collections;
using UnityEngine;

public sealed class TamaraCutscene_6 : MonoBehaviour
{
    [SerializeField]
    private GameObject _tomara, _warning, _ladder;

    [SerializeField]
    private Sprite _down, _up;
    
    [SerializeField]
    private AudioClip[] _audioClips;
    
    private IEnumerator Start()
    {
        if (Stats.Instance.TomaraCutscene >= 10)
        {
            gameObject.SetActive(false);
            yield break;
        }
        
        Fedya.Instance.SetDirection(new Vector2(0, 1));
        yield return new WaitUntil(() => Fedya.Instance.enabled);
        Fedya.Instance.enabled = false;
        Fedya.Instance.transform.position = new Vector3(0, -2.4f);

        var themeTime = MusicManager.Instance.GetTime;
        MusicManager.Instance.Stop();
        
        var END_X = 3.65f;
        var SPEED = 3;
        
        if (Stats.Instance.IsGenocide)
        {
            yield return DialogueWindow.StartDialogue(new [] {
                "\\T2Я на 5 минут вышла, опять всё засрали.",
                "\\T1А где все?",
            }, true);
        }
        else
        {
            yield return DialogueWindow.StartDialogue(new [] {
                "\\T2Я на 5 минут вышла, опять всё засрали.",
                "\\T2А ну иди сюда!",
            }, true);
        
            _tomara.GetComponent<Animator>().enabled = true;
            _tomara.GetComponent<Animator>().Play("Tomara Right Move");
            
            END_X = 3.65f;
            SPEED = 3;
            while (_tomara.transform.position.x != END_X)
            {
                _tomara.transform.position = Vector2.MoveTowards(_tomara.transform.position,
                    new Vector2(END_X, _tomara.transform.position.y), Time.deltaTime * SPEED);

                yield return null;
            }
        
            _tomara.GetComponent<Animator>().Play("Tomara Right");
            _ladder.SetActive(true);

            while (_ladder.transform.eulerAngles.z != 0)
            {
                _ladder.transform.eulerAngles = Vector3.MoveTowards(_ladder.transform.eulerAngles, 
                    new Vector3(0, 0, 0), Time.deltaTime * 200);
            
                yield return null;
            }
        
            _ladder.GetComponent<AudioSource>().Play();
        
            yield return new WaitForSeconds(0.5f);
        
            yield return DialogueWindow.StartDialogue(new [] {
                "\\G1Ай блять!",
                "\\T2Так будет с каждым кто будет кидать мусор!",
            }, true);
        
            _ladder.SetActive(false);
        }
        
        // Разворачивается
        _tomara.GetComponent<Animator>().enabled = true;
        _tomara.GetComponent<Animator>().Play("Tomara Down");
        yield return new WaitForSeconds(0.5f);
        
        // Знак над головой
        _warning.SetActive(true);
        yield return new WaitForSeconds(1);
        _warning.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        
        MusicManager.Instance.Play(_audioClips[0]);

        var END_Y = 0.16f;
        
        if (!Stats.Instance.IsGenocide)
        {
            // Идёт к Феде
            _tomara.GetComponent<Animator>().Play("Tomara Down Move");
            END_X = 0f;

            while (_tomara.transform.position.y > END_Y && _tomara.transform.position.x > END_X)
            {
                _tomara.transform.position = Vector2.MoveTowards(_tomara.transform.position,
                    new Vector2(END_X, END_Y), Time.deltaTime * SPEED);

                yield return null;
            }
        }

        _tomara.GetComponent<Animator>().Play("Tomara Down");
        
        yield return DialogueWindow.StartDialogue(new [] {
            "\\T1Боже! Ты уже тут?!",
            "\\T1Я только нашла стремянку и пошла тебя спасать.",
            "\\T1Прости что так долго.*Мне пришлось немного задержаться.",
            "\\T2Я проводила воспитательную беседу.",
            "\\T1Как у тебя получилось выбраться?*Где ты был?",
            "\\T1А не важно.",
            "\\T1Пойдём со мной.*У меня для тебя сюрприз.",
        });
        
        // Идёт к гопнику
        yield return DialogueWindow.StartDialogue(new [] {
            "\\T1...", // Осуждающий взгляд
        });
        
        // Уходит
        _tomara.GetComponent<Animator>().Play("Tomara Up Move");

        SPEED = 5;
        END_Y = 4.66f;
        while (_tomara.transform.position.y < END_Y)
        {
            _tomara.transform.position = Vector2.MoveTowards(_tomara.transform.position,
                new Vector2(_tomara.transform.position.x, END_Y), Time.deltaTime * SPEED);

            yield return null;
        }
        
        _tomara.GetComponent<Animator>().Play("Tomara Right Move");
        
        SPEED = 3;
        END_X = 1.69f;
        while (_tomara.transform.position.x != END_X)
        {
            _tomara.transform.position = Vector2.MoveTowards(_tomara.transform.position,
                new Vector2(END_X, _tomara.transform.position.y), Time.deltaTime * SPEED);

            yield return null;
        }
        
        _tomara.GetComponent<Animator>().Play("Tomara Up Move");
        
        SPEED = 5;
        END_Y = 11.63f;
        while (_tomara.transform.position.y < END_Y)
        {
            _tomara.transform.position = Vector2.MoveTowards(_tomara.transform.position,
                new Vector2(_tomara.transform.position.x, END_Y), Time.deltaTime * SPEED);

            yield return null;
        }

        if (Stats.Instance.IsGenocide)
        {
            MusicManager.Instance.Play(_audioClips[2]);
        }
        else
        {
            MusicManager.Instance.Play(_audioClips[1]);
        }
        
        MusicManager.Instance.SetTime(themeTime);
        
        Fedya.Instance.enabled = true;
        gameObject.SetActive(false);

        Stats.Instance.TomaraCutscene = 10;
    }
}

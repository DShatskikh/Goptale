using System.Collections;
using UnityEngine;

public sealed class TomaraCutscene_4 : MonoBehaviour
{
    [SerializeField]
    private GameObject _tomara;
    
    [SerializeField]
    private CameraLimit _cameraLimit;
    
    [SerializeField]
    private GameObject _jaguar;
    
    [SerializeField]
    private Sprite _tomaraRight, _tomaraDown;

    [SerializeField]
    private Transform _up, _down;
    
    private IEnumerator Start()
    {
        if (Stats.Instance.TomaraCutscene >= 7)
        {
            gameObject.SetActive(false);
            
            yield break;
        }
        
        if (Stats.Instance.TomaraCutscene >= 6)
        {
            _tomara.SetActive(true);
            _tomara.transform.position = new Vector2(20, _tomara.transform.position.y);
            StartCoroutine(AwaitCutscene_6());
            yield break;
        }

        Fedya.Instance.transform.position = new Vector3(-4.27f, 1.34000003f, 0);
        
        yield return new WaitUntil(() => Fedya.Instance.enabled);
        Fedya.Instance.enabled = false;
        
        var END_X = 20f;
        var SPEED = 2f;
        _cameraLimit._rightLimit = 16;
        _tomara.SetActive(true);
        Fedya.Instance.transform.position = new Vector3(-4.27f, 1.34000003f, 0);
        StartCoroutine(Await());

        yield return null;
        Fedya.Instance.enabled = false;
        _tomara.GetComponent<Animator>().Play("Tomara Right Move");
        Fedya.Instance.GetComponentInChildren<Animator>().SetFloat("Speed", 1);
        
        while (_tomara.transform.position.x < END_X)
        {
            _tomara.transform.position = Vector2.MoveTowards(_tomara.transform.position,
                new Vector2(END_X, _tomara.transform.position.y), Time.deltaTime * SPEED);

            Fedya.Instance.transform.position = Vector2.MoveTowards(Fedya.Instance.transform.position,
                new Vector2(END_X, Fedya.Instance.transform.position.y), Time.deltaTime * SPEED);
            
            yield return null;
        }
        
        Fedya.Instance.GetComponentInChildren<Animator>().SetFloat("Speed", 0);
        _tomara.GetComponent<Animator>().SetTrigger("Stop");
        
        yield return new WaitForSeconds(1);
        
        Fedya.Instance.SetDirection(new Vector2(0, 1));
        
        yield return new WaitForSeconds(1);
        
        _tomara.GetComponent<Animator>().Play("Tomara Down");
        _tomara.GetComponent<SpriteRenderer>().sprite = _tomaraDown;
        
        yield return new WaitForSeconds(0.5f);
        
        yield return DialogueWindow.StartDialogue(new [] {
            "\\T1Ладно-ладно, %я понимаю.",
            "\\T1Могли выйти уже 5 частей %и даже перезапуск франшизы.",
            "\\T1Стой!%*Смотри!",
        }, true);

        _tomara.GetComponent<Animator>().Play("Tomara Right");
        yield return new WaitForSeconds(0.5f);
        Fedya.Instance.SetDirection(new Vector2(1, 0));
        yield return new WaitForSeconds(1);
        
        var LIMIT_X = 20;

        while (_cameraLimit._rightLimit < LIMIT_X)
        {
            yield return null;
            _cameraLimit._rightLimit += Time.deltaTime * 2;

            if (_cameraLimit._rightLimit > LIMIT_X)
            {
                _cameraLimit._rightLimit = LIMIT_X;
            }
        }
        
        yield return DialogueWindow.StartDialogue(new [] {
            "\\T1Это ловушка!",
            "\\T1Руины полны ловушек.",
            "\\T1Чтобы пройти через Руины %ты должен научиться решать их.",
            "\\T1Здесь %я пометила нужную бутылку.",
            "\\T1Тебе нужно только взять её.",
        }, true);
        
        Fedya.Instance.enabled = true;
        Stats.Instance.TomaraCutscene = 6;
        StartCoroutine(AwaitCutscene_6());
    }

    private IEnumerator AwaitCutscene_6()
    {
        yield return new WaitUntil(() => _jaguar == null);
        
        yield return DialogueWindow.StartDialogue(new []
        {
            $"Вы получили <color=\"yellow\">{Constants.JAGUAR}</color>."
        }); 
        
        // yield return new WaitUntil(() => Fedya.Instance.enabled);
        // Fedya.Instance.enabled = false;
        
        var progress = 0f;
        var upStartPosition = _up.localPosition;
        var upStartScale = _up.localScale;
        var downStartPosition = _down.localPosition;
        var downStartScale = _down.localScale;
        
        while (progress < 1)
        {
            yield return null;
            progress += Time.deltaTime;
            _up.localPosition = Vector3.Lerp(upStartPosition, new Vector3(upStartPosition.x, 0.4235f, upStartPosition.z), progress);
            _up.localScale = Vector3.Lerp(upStartScale, new Vector3(_up.localScale.x, 0), progress);
            
            _down.localPosition = Vector3.Lerp(downStartPosition, new Vector3(downStartPosition.x, -2.8834f, downStartPosition.z), progress);
            _down.localScale = Vector3.Lerp(downStartScale, new Vector3(_down.localScale.x, 0), progress);
        }
        
        _up.gameObject.SetActive(false);
        _down.gameObject.SetActive(false);
        
        yield return DialogueWindow.StartDialogue(new [] {
            "\\T1Молодец!%*Пошли дальше!",
        }, true);
        
        _tomara.GetComponent<Collider2D>().enabled = false;
        _tomara.GetComponent<Animator>().enabled = true;
        _tomara.GetComponent<Animator>().Play("Tomara Right Move");
        
        var END_X = 28.45731f;
        var SPEED = 5;
        while (_tomara.transform.position.x < END_X)
        {
            _tomara.transform.position = Vector2.MoveTowards(_tomara.transform.position,
                new Vector2(END_X, _tomara.transform.position.y), Time.deltaTime * SPEED);

            yield return null;
        }
        
        _tomara.GetComponent<Animator>().SetTrigger("Stop");
        
        while (_tomara.GetComponent<SpriteRenderer>().color.a > 0)
        {
            var color = _tomara.GetComponent<SpriteRenderer>().color;
            color.a -= Time.deltaTime;
            _tomara.GetComponent<SpriteRenderer>().color = color;
            
            yield return null;
        }
        
        Stats.Instance.TomaraCutscene = 7;
        Fedya.Instance.enabled = true;
    }
    
    private IEnumerator Await()
    {
        yield return new WaitForSeconds(2);
        
        StartCoroutine(DialogueWindow.StartDialogue(new [] {
            "\\T1Слушай,% ты же с поверхности?",
            "\\T1Можешь сказать, %а вторая часть бумера уже вышла?",
        }, true));
    }
}

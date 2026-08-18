using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class TamaraCutscene_5 : MonoBehaviour
{
    [SerializeField]
    private GameObject _tomara;
    
    [SerializeField]
    private CameraLimit _cameraLimit;

    [SerializeField]
    private GameObject _pit, _pit2;
    
    [SerializeField]
    private Transform _train;

    [SerializeField]
    private AudioSource _fallSFX, _noiseSFX, _trainSFX;
    
    private void Start()
    {
        if (Stats.Instance.TomaraCutscene < 8)
        {
            CoroutineRunner.Instance.StartCoroutine(Await());
        }
        else if (Stats.Instance.TomaraCutscene == 8)
        {
            Stats.Instance.TomaraCutscene = 9;
            StartCoroutine(AwaitLay());
        }
        else
        {
            gameObject.SetActive(false);
            _pit2.SetActive(true);
        }
    }
    
    private IEnumerator AwaitLay()
    {
        MusicManager.Instance.Stop();
        Fedya.Instance.transform.position = new Vector3(-4.51999998f + 0.5f, -16.69f, 0);
        Fedya.Instance.transform.eulerAngles = new Vector3(0, 0, 90);
        //yield return new WaitUntil(() => Fedya.Instance.enabled);
        Fedya.Instance.enabled = false;
        yield return new WaitForSeconds(1);
        Fedya.Instance.transform.eulerAngles = new Vector3(0, 0, 0);
        Fedya.Instance.transform.position = new Vector3(-4.51999998f, -16.69f, 0);
        Fedya.Instance.enabled = true;
        _noiseSFX.Play();
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
    }
    
    private IEnumerator Await()
    {
        Fedya.Instance.transform.position = new Vector3(-4.51999998f, -0.779999971f, 0);
        yield return new WaitUntil(() => Fedya.Instance.enabled);
        Fedya.Instance.enabled = false;
        
        _tomara.GetComponent<Animator>().Play("Tomara Right");
        
        yield return DialogueWindow.StartDialogue(new [] {
            "\\T1Хм-м^0.3.^0.3.^0.3.",
            "\\T1Ты наверное слишком мал чтобы решать эту загадку %самостоятельно.",
            "\\T1Я решу вместо тебя.",
        });
        
        _cameraLimit._rightLimit = -3;
        var LIMIT_X = -9.7f;

        while (_cameraLimit._rightLimit > LIMIT_X)
        {
            yield return null;
            _cameraLimit._rightLimit -= Time.deltaTime * 2;

            if (_cameraLimit._rightLimit < LIMIT_X)
            {
                _cameraLimit._rightLimit = LIMIT_X;
            }
        }
        
        _tomara.GetComponent<Animator>().Play("Tomara Right Move");
        
        var END_X = -0.1f;
        var SPEED = 2;
        while (_tomara.transform.position.x < END_X)
        {
            _tomara.transform.position = Vector2.MoveTowards(_tomara.transform.position,
                new Vector2(END_X, _tomara.transform.position.y), Time.deltaTime * SPEED);

            yield return null;
        }
        
        _noiseSFX.Play();
        yield return new WaitForSeconds(1f);
        
        yield return DialogueWindow.StartDialogue(new [] {
            "\\T1Ах!*Ой!",
            "\\T1По привычке не тот стул.",
        });

        _pit.SetActive(true);
        // yield return new WaitForSeconds(0.5f);
        
        Fedya.Instance.GetComponent<Collider2D>().enabled = false;
        Fedya.Instance.GetComponentInChildren<Animator>().Play("PlayerFall");
        _fallSFX.Play();
        
        var fallY = -16.69f;
        SPEED = 5;
        while (Fedya.Instance.transform.position.y > fallY)
        {
            Fedya.Instance.transform.position = Vector2.MoveTowards(Fedya.Instance.transform.position,
                new Vector2(Fedya.Instance.transform.position.x, fallY), Time.deltaTime * SPEED);
            
            yield return null;
        }
        
        Fedya.Instance.GetComponentInChildren<Animator>().Play("Movement");
        
        LIMIT_X = -3; // 24

        while (_cameraLimit._rightLimit < LIMIT_X)
        {
            yield return null;
            _cameraLimit._rightLimit += Time.deltaTime * 5;

            if (_cameraLimit._rightLimit > LIMIT_X)
            {
                _cameraLimit._rightLimit = LIMIT_X;
            }
        }
        
        yield return DialogueWindow.StartDialogue(new [] {
            "\\T1Эй! %ты там впорядке?",
            "\\T1Ты упал в тонели метрополитена.",
            "\\T1Понимаешь инженеры совсем не считаются с тем...",
            "\\T1...что здесь заперты монстры.",
            "\\T1Они замкнули кольцевую линию прямо через Снежино.", // Сноудин
            "\\T1Сейчас я слезу отсюда.%*Спущусь вниз %и вытащу тебя.",
        });
        
        _train.gameObject.SetActive(true);
        _trainSFX.Play();
        
        // 17.32
        // -3.14
        END_X = -3.14f;
        SPEED = 10;
        while (_train.position.x > END_X)
        {
            _train.position = Vector2.MoveTowards(_train.position,
                new Vector2(END_X, _train.position.y), Time.deltaTime * SPEED);

            yield return null;
        }
        
        _trainSFX.Stop();
        Stats.Instance.TomaraCutscene = 9;
        
        var startBattleScreen = Instantiate(Resources.Load<StartBattleScreen>("StartBattleScreen"), 
            new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y), Quaternion.identity);
        yield return new WaitUntil(() => startBattleScreen.IsEnd);
        yield return  SceneManager.UnloadSceneAsync(Stats.Instance.LevelName, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
        yield return  SceneManager.LoadSceneAsync("Battle",  LoadSceneMode.Additive);
        yield return  SceneManager.UnloadSceneAsync("Overworld", UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
        var train = Instantiate(Resources.Load<GameObject>("Train"));
        SceneManager.MoveGameObjectToScene(train, SceneManager.GetSceneByName("Battle"));
        var background = Instantiate(Resources.Load<GameObject>("Enemy Background"));
        SceneManager.MoveGameObjectToScene(background, SceneManager.GetSceneByName("Battle"));
    }
}

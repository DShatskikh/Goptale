using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public sealed class TomaraCutscene_9 : MonoBehaviour
{
    public static bool IsRunning;
    
    [SerializeField]
    private GameObject _tomara, _barier;

    [SerializeField]
    private Sprite _up, _right, _down;

    [SerializeField]
    private AudioSource _sfxUp;
    
    [SerializeField]
    private Tilemap _tilemap;

    [SerializeField]
    private GameObject[] _otherObjects;
    
    private IEnumerator Start()
    {
        if (Stats.Instance.IsGenocide && Stats.Instance.Kills < 12)
        {
            Stats.Instance.IsGenocide = false;
        }
        
        if (Stats.Instance.TomaraCutscene > 15)
        {
            gameObject.SetActive(false);
            _barier.SetActive(true);
            yield break;
        }

        if (Stats.Instance.TomaraCutscene == 15) // Победили мирно
        {
            Stats.Instance.TomaraCutscene = 16;
            GetComponent<BoxCollider2D>().enabled = false;
                
            Fedya.Instance.enabled = false;
            Fedya.Instance.SetDirection(new Vector2(0, 1));
            Fedya.Instance.transform.position = new Vector3(6.09f,12.56f,0);

            if (Stats.Instance.IsTomaraDead)
            {
                _tomara.transform.rotation = Quaternion.Euler(0, 0, 90);
                
                yield return new WaitForSeconds(1f);
                
                yield return DialogueWindow.StartDialogue(new [] {
                    "(.%.%.)",
                }, true);
                
                yield return new WaitForSeconds(0.5f);
                
                _sfxUp.Play();
                _tomara.transform.rotation = Quaternion.Euler(0, 0, 0);
                
                yield return new WaitForSeconds(1f);
                
                _tomara.GetComponent<Animator>().enabled = true;
                _tomara.GetComponent<Animator>().Play("Tomara Down");
                
                yield return DialogueWindow.StartDialogue(new [] {
                    "\\T1Ты и вправду можешь за себя постоять.",
                    "\\T1Я не могу тебя больше удерживать.",
                    "\\T1Ты можешь идти.",
                }, true);
            }
            else
            {
                _tomara.GetComponent<SpriteRenderer>().sprite = _up;
                
                yield return DialogueWindow.StartDialogue(new [] {
                    "\\T1Если ты и правда хочешь покинуть РУИНИНО...",
                    "\\T1Я не буду тебя останавливать.",
                    "\\T1Однако, %когда ты уйдешь...",
                    "\\T1Пожалуйста, %не возвращайся.",
                    "\\T1Надеюсь, ты понимаешь.",
                }, true);
            }
            
            // Обнимает
            _tomara.GetComponent<Animator>().enabled = true;
            _tomara.GetComponent<Animator>().Play("Hug");
            Fedya.Instance.gameObject.SetActive(false);

            yield return new WaitForSeconds(4f);
                
                _tomara.GetComponent<Animator>().enabled = false;
                _tomara.GetComponent<SpriteRenderer>().sprite = _down;
                
                Fedya.Instance.gameObject.SetActive(true);
                Fedya.Instance.SetDirection(new Vector2(0, 1));
                yield return DialogueWindow.StartDialogue(new [] {
                    "\\T1Прощай, бумер.",
                }, true);
                
                // Уходит назад
                _tomara.GetComponent<Animator>().enabled = true;
                _tomara.GetComponent<Animator>().Play("Tomara Right Move");
                
                var SPEED = 3f;
                var END_X = 1.86f;
            
                while (_tomara.transform.localPosition.x != END_X)
                {
                    _tomara.transform.localPosition = Vector2.MoveTowards(_tomara.transform.localPosition,
                        new Vector2(END_X, _tomara.transform.localPosition.y), Time.deltaTime * SPEED);

                    yield return null;
                } 
                
                _tomara.GetComponent<Animator>().Play("Tomara Down Move");
                
                var END_Y = -0.77f;
            
                while (_tomara.transform.localPosition.y != END_Y)
                {
                    _tomara.transform.localPosition = Vector2.MoveTowards(_tomara.transform.localPosition,
                        new Vector2(_tomara.transform.localPosition.x,END_Y), Time.deltaTime * SPEED);

                    yield return null;
                } 
                
                _tomara.GetComponent<Animator>().Play("Tomara Right Move");
                _tomara.GetComponent<SpriteRenderer>().flipX = true;

                END_X = -0.06f;
                
                while (_tomara.transform.localPosition.x != END_X)
                {
                    _tomara.transform.localPosition = Vector2.MoveTowards(_tomara.transform.localPosition,
                        new Vector2(END_X, _tomara.transform.localPosition.y), Time.deltaTime * SPEED);

                    yield return null;
                } 
                
                _tomara.GetComponent<Animator>().Play("Tomara Down Move");
                _tomara.GetComponent<SpriteRenderer>().flipX = false;

                END_Y = -3.71f;
            
                while (_tomara.transform.localPosition.y != END_Y)
                {
                    _tomara.transform.localPosition = Vector2.MoveTowards(_tomara.transform.localPosition,
                        new Vector2(_tomara.transform.localPosition.x,END_Y), Time.deltaTime * SPEED);

                    yield return null;
                } 
                
                _tomara.GetComponent<Animator>().enabled = false;
                
                // Оборачивается
                Fedya.Instance.SetDirection(new Vector2(0, -1));
                
                yield return new WaitForSeconds(1f);
                _tomara.GetComponent<SpriteRenderer>().sprite = _right;
                
                yield return new WaitForSeconds(0.5f);
                _tomara.GetComponent<SpriteRenderer>().sprite = _up;
                
                yield return new WaitForSeconds(2f);
                
                _tomara.GetComponent<SpriteRenderer>().sprite = _down;
                
                yield return new WaitForSeconds(0.5f);
                
                _tomara.GetComponent<Animator>().enabled = true;
                _tomara.GetComponent<Animator>().Play("Tomara Down Move");
                
                END_Y = -10.97f;
                
                while (_tomara.transform.localPosition.y != END_Y)
                {
                    _tomara.transform.localPosition = Vector2.MoveTowards(_tomara.transform.localPosition,
                        new Vector2(_tomara.transform.localPosition.x,END_Y), Time.deltaTime * SPEED);

                    yield return null;
                } 
                
                Fedya.Instance.enabled = true;
                gameObject.SetActive(false);
                _barier.SetActive(true);
                yield break;
        }

        if (IsRunning)
        {
            yield return DialogueWindow.StartDialogue(new [] {
                "\\T1Верно.",
                "\\T1Ноднимись наверх.",
            }, true);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.GetComponent<Fedya>())
            return;
        
        CoroutineRunner.Instance.StartCoroutine(AwaitTrigger());
    }
    
    private IEnumerator AwaitTrigger()
    {
        Fedya.Instance.enabled = false;
        _tomara.GetComponent<Animator>().enabled = true;
        _tomara.GetComponent<Animator>().Play("Tomara Down");
        
        if (IsRunning)
        {
            yield return DialogueWindow.StartDialogue(new [] {
                "\\T1Уже?",
                "\\T1Что заставить тебя выучить твой урок?",
            });
        }
        else
        {
            yield return DialogueWindow.StartDialogue(new [] {
                "\\T1Ты так сильно хочешь уйти?",
                "\\T1Хм.",
                "\\T1Ты такой же, %как и остальные.",
                "\\T1Есть только одно решение этой ситуации.",
                "\\T1Докажи...",
                "\\T1Докажи мне, %что ты достаточно силён, %чтобы выжить.",
            }, true);
            
            _sfxUp.Play();
            _tilemap.color = new Color(0.66f, 0.66f, 0.66f, 1);
            
            yield return new WaitForSeconds(1f);
            
            _sfxUp.Play();
            _tilemap.color = new Color(0.33f, 0.33f, 0.33f, 1);
            
            yield return new WaitForSeconds(1f);
            
            _sfxUp.Play();
            _tilemap.color = new Color(0f, 0f, 0f, 1);
            _otherObjects[0].SetActive(false);
            _otherObjects[1].SetActive(false);
            
            yield return new WaitForSeconds(0.5f);
        }
        
        var startBattleScreen = Instantiate(Resources.Load<StartBattleScreen>("StartBattleScreen"), 
            new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y), Quaternion.identity);
        yield return new WaitUntil(() => startBattleScreen.IsEnd);
        yield return  SceneManager.UnloadSceneAsync(Stats.Instance.LevelName, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
        yield return  SceneManager.LoadSceneAsync("Battle",  LoadSceneMode.Additive);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Battle"));
        yield return  SceneManager.UnloadSceneAsync("Overworld", UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
        var german = Instantiate(Resources.Load<GameObject>("Tomara"));
        SceneManager.MoveGameObjectToScene(german, SceneManager.GetSceneByName("Battle"));
        var background = Instantiate(Resources.Load<GameObject>("Boss Background"));
        SceneManager.MoveGameObjectToScene(background, SceneManager.GetSceneByName("Battle"));
    }
}

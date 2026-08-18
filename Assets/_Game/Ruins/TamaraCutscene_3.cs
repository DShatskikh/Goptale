using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class TamaraCutscene_3 : MonoBehaviour
{
    [SerializeField]
    private GameObject _container;

    [SerializeField]
    private GameObject _tomara;
    
    [SerializeField]
    private GameObject _gopnikDeadContainer, _gopnikDead_1, _gopnikDead_2, _gopnikDead_3;
    
    private void Start()
    {
        // 3 впервые пригшли, 4 после боя, 5 ушли с локации
        if (Stats.Instance.TomaraCutscene >= 5)
        {
            gameObject.SetActive(false);
            return;
        }
        
        if (Stats.Instance.TomaraCutscene == 4)
        {
            Fedya.Instance.transform.position = new Vector3(18.8799992f, 1.99000001f, 0);
            StartCoroutine(AwaitTomaraCutscene_4());
            return;
        }
        
        GetComponent<BoxCollider2D>().enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.GetComponent<Fedya>())
            return;

        CoroutineRunner.Instance.StartCoroutine(Await());
    }

    private IEnumerator AwaitTomaraCutscene_4()
    {
        Fedya.Instance.enabled = false;
        Fedya.Instance.SetDirection(new Vector2(-1f, 0));
        _container.SetActive(false);
        _tomara.SetActive(true);
        _tomara.GetComponent<Animator>().Play("Tomara Right");
        
        if (Stats.Instance.LV == 2) // Всех троих
        {
            _gopnikDeadContainer.SetActive(true);
            _gopnikDead_1.SetActive(true);
            _gopnikDead_2.SetActive(true);
            _gopnikDead_3.SetActive(true);
            
            Fedya.Instance.enabled = false;
            yield return DialogueWindow.StartDialogue(new [] {
                "\\T1Глазам не верю!%*Ты расскидал эту гопоту!",
                "\\T1Пошли дальше."
            });
        }
        else if (Stats.Instance.EXP > 0) // Не всех
        {
            _gopnikDeadContainer.SetActive(true);
            _gopnikDead_1.SetActive(true);
            
            if (Stats.Instance.EXP > 3)
            {
                _gopnikDead_2.SetActive(true);
            }
            
            Fedya.Instance.enabled = false;
            yield return DialogueWindow.StartDialogue(new [] {
                "\\T1Че очканул?*Это была проверка.",
                "\\T2Постарайся больше никого не бить.",
                "\\T1Пошли дальше.",
            });
        }
        else // Пацифист
        {
            Fedya.Instance.enabled = false;
            yield return DialogueWindow.StartDialogue(new [] {
                "\\T1Че очканул?*Это была проверка.",
                "\\T1Молодец что дождался меня.",
                "\\T1Пошли дальше.",
            });
        }
        
        Fedya.Instance.enabled = false;
        
        _tomara.GetComponent<Animator>().Play("Tomara Right Move");

        var TARGET_POSITION = new Vector3(5.15999985f, -0.0359999985f);
        
        while (_tomara.transform.localPosition != TARGET_POSITION)
        {
            _tomara.transform.localPosition = Vector2.MoveTowards(_tomara.transform.localPosition,
                TARGET_POSITION, Time.deltaTime * 4f);

            yield return null;
        }

        _tomara.GetComponent<Animator>().SetTrigger("Stop");
        
        Stats.Instance.TomaraCutscene = 5;
        Fedya.Instance.enabled = true;
        
        while (_tomara.GetComponent<SpriteRenderer>().color.a > 0)
        {
            var color = _tomara.GetComponent<SpriteRenderer>().color;
            color.a -= Time.deltaTime;
            _tomara.GetComponent<SpriteRenderer>().color = color;
            
            yield return null;
        }
    }

    private IEnumerator Await() 
    {
        Fedya.Instance.enabled = false;
        yield return DialogueWindow.StartDialogue("\\G1Эээ... Слышь.*Дай мобилу звякнуть.");
        
        var selected = SelectionWindow.StartDialogue("Отдать мобилу?", "У меня её нет", "Послать");
        yield return new WaitUntil(() => selected == null);
        
        if (!SelectionWindow.IsRight)
        {
            yield return DialogueWindow.StartDialogue(new [] {
                "Вы сказали что у вас нету мобилы.%*Кажется вам не поверили.",
                "\\G1А если найду?"
            });
        }
        else
        {
            yield return DialogueWindow.StartDialogue(new [] {
                "Вы послали гопоту.",
                "\\G1Эээ... ты че бесмертный?"
            });
        }
        
        var startBattleScreen = Instantiate(Resources.Load<StartBattleScreen>("StartBattleScreen"), 
           new Vector3(Camera.main.transform.position.x,  Camera.main.transform.position.y), Quaternion.identity);
        yield return new WaitUntil(() => startBattleScreen.IsEnd);
        
        yield return  SceneManager.UnloadSceneAsync(Stats.Instance.LevelName, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
        yield return  SceneManager.LoadSceneAsync("Battle",  LoadSceneMode.Additive);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Battle"));
        yield return  SceneManager.UnloadSceneAsync("Overworld", UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
        
        var gopnik_1 = Instantiate(Resources.Load<GameObject>("Gopnik"));
        SceneManager.MoveGameObjectToScene(gopnik_1, SceneManager.GetSceneByName("Battle"));
        gopnik_1.transform.position = new Vector3(-5f - 1.11f, gopnik_1.transform.position.y);
        
        var gopnik_2 = Instantiate(Resources.Load<GameObject>("Gopnik"));
        SceneManager.MoveGameObjectToScene(gopnik_2, SceneManager.GetSceneByName("Battle"));
        gopnik_2.transform.position = new Vector3(-1.11f, gopnik_2.transform.position.y);
        
        var gopnik_3 = Instantiate(Resources.Load<GameObject>("Gopnik"));
        SceneManager.MoveGameObjectToScene(gopnik_3, SceneManager.GetSceneByName("Battle"));
        gopnik_3.transform.position = new Vector3(5f - 1.11f, gopnik_3.transform.position.y);
        
        var background = Instantiate(Resources.Load<GameObject>("Enemy Background"));
        SceneManager.MoveGameObjectToScene(background, SceneManager.GetSceneByName("Battle"));
    }
}

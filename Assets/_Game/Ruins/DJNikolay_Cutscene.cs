using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class DJNikolay_Cutscene : Usable
{
    private bool _triedMove;
    
    private IEnumerator Start()
    {
        if (Stats.Instance.DJNikolayState == 0 && Stats.Instance.IsGenocide)
        {
            var alpha = 1f;

            while (alpha > 0f)
            {
                alpha -= Time.deltaTime;
                GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, alpha);
                yield return null;
            }

            gameObject.SetActive(false);
            Stats.Instance.DJNikolayState = 4;
            
            Debug.Log("1 Эксперимент не был провальным");

            yield return null;
            
            Debug.Log("2 Эксперимент был провальным");
            
            yield break;
        }
        
        if (Stats.Instance.DJNikolayState == 1) // победили мирно но не поговорили
        {
            Fedya.Instance.enabled = false;
            
            yield return DialogueWindow.StartDialogue(new [] {
                "капец %куда меня занесло...%*я так набухался что не помню что вчера было...",
                "наверное это была легендарная туса...",
                "ладно-ладно %я уйду с твоего пути...",
                "я полетел дальше кутить...",
            });
            
            var alpha = 1f;

            while (alpha > 0f)
            {
                alpha -= Time.deltaTime;
                GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, alpha);
                yield return null;
            }
            
            yield return DialogueWindow.StartDialogue(new [] {
                "DJ Николай рассосался...",
            });
            
            gameObject.SetActive(false);
            Stats.Instance.DJNikolayState = 3;
            Fedya.Instance.enabled = true;
        }
        else if (Stats.Instance.DJNikolayState == 2) // убили
        {
            Fedya.Instance.enabled = false;
            
            yield return DialogueWindow.StartDialogue(new [] {
                "хочешь прикол?..",
                "на самом деле я притворялся что ты мне наносишь урон...",
                "я ведь призрак%меня нельзя коснуться...",
                "лан, я полетел дальше кутить...",
            });
            
            var alpha = 1f;

            while (alpha > 0f)
            {
                alpha -= Time.deltaTime;
                GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, alpha);
                yield return null;
            }
            
            yield return DialogueWindow.StartDialogue(new [] {
                "DJ Николай рассосался...",
            });
            
            gameObject.SetActive(false);
            Stats.Instance.DJNikolayState = 4;
            Fedya.Instance.enabled = true;
        }
        else if (Stats.Instance.DJNikolayState == 3 || Stats.Instance.DJNikolayState == 4)
        {
            gameObject.SetActive(false);
        }
    }

    public override void Use()
    {
        Fedya.Instance.enabled = false;
        CoroutineRunner.Instance.StartCoroutine(AwaitUse());
        // Разве вы пропустите это? Паучья распродажа выпечки вниз и направо.
        // Заходите и отведайте еды, приготовленной пауками для пауков из пауков!
        
        // Купить ... 18М? Да Нет
        // 7
        // У вас недостаточно РУБ
        // Нищеброд
        
        // хр-р-р-р-р-р-р...
        // хр-р-р-р-р-... Ёпт...
        
        // хр-р-р-р-р...
        // (как же башка с бодуна болит)
        
        // хр-р-р-р-р-р-р...
        // (он уже ушел?)
        // хр-р-р-р-р...
        
        // (Призрак продолжает повторять \"хр-р\" вслух, делая вид, что спит.)
        // Подвинуть его силой? Да Нет
        
        // На сцену выходит DJ Николай.
        // Оценить Флиртовать Угрожать Поддержать
    }
    
    private IEnumerator AwaitUse()
    {
        if (!_triedMove)
        {
            yield return DialogueWindow.StartDialogue(new [] {
                "хр-р-р-р-р...*(ааа! башка после бухича болит.)",
                "хр-р-р-р-р-р-р...*(капец он еще тут?)",
                "(DJ Николай продолжает повторять \"хр-р\" вслух, делая вид, что кимарит.)",
            });
        }
        
        var selectDialogue = SelectionWindow.StartDialogue($"Подвинуть его силой?", $"Да", "Нет");
        yield return new WaitUntil(() => selectDialogue == null);

        if (SelectionWindow.IsRight)
        {
            _triedMove = true;
            
            yield return DialogueWindow.StartDialogue(new [] {
                "хр-р-р-р-р...*(от души бро.)"
            });
            
            Fedya.Instance.enabled = true;
            yield break;
        }
        
        var startBattleScreen = Instantiate(Resources.Load<StartBattleScreen>("StartBattleScreen"), 
            new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y), Quaternion.identity);
        yield return new WaitUntil(() => startBattleScreen.IsEnd);
        yield return  SceneManager.UnloadSceneAsync(Stats.Instance.LevelName, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
        yield return  SceneManager.LoadSceneAsync("Battle",  LoadSceneMode.Additive);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Battle"));
        yield return  SceneManager.UnloadSceneAsync("Overworld", UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
        var djNikolay = Instantiate(Resources.Load<GameObject>("DJ Nikolay"));
        SceneManager.MoveGameObjectToScene(djNikolay, SceneManager.GetSceneByName("Battle"));
        var background = Instantiate(Resources.Load<GameObject>("Boss Background"));
        SceneManager.MoveGameObjectToScene(background, SceneManager.GetSceneByName("Battle"));
        
        // обычно я прихожу в РУИНЫб потому что здесь никого нет...
        // но сегодня я встретил кое-кого славного...
        // ...
        // ох, я опять говорю невпопад
        // я уйду с твоего пути
    }
}

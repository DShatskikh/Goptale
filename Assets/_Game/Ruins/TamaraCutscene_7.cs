using System.Collections;
using UnityEngine;

public sealed class TamaraCutscene_7 : MonoBehaviour
{
    [SerializeField]
    private GameObject _tomara;
    
    private IEnumerator Start()
    {
        if (Stats.Instance.TomaraCutscene >= 11)
        {
            gameObject.SetActive(false);

            yield break;
        }
        
        Fedya.Instance.SetDirection(new Vector2(0f, 1f));
        yield return new WaitUntil(() => Fedya.Instance.enabled);
        Fedya.Instance.enabled = false;
        
        _tomara.GetComponent<Animator>().enabled = true;
        _tomara.GetComponent<Animator>().Play("Tomara Down");
        
        yield return DialogueWindow.StartDialogue(new [] {
            "\\T1Сюрприз!%*Ты можешь жить тут.",
            "\\T1Правда у меня нету отдельной комнаты.",
            "\\T1Но ты можешь спать на диване.",
            "\\T1Ты наверно проголодался?",
            "\\T1Отдохни пока, а я приготовлю беляши.",
        });
        
        // Vector3(0.00999999978,4.67999983,0)
        
        _tomara.GetComponent<Animator>().Play("Tomara Up Move");
        var SPEED = 5f;
        var END_Y = 4.67999983f;
        
        while (_tomara.transform.localPosition.y != END_Y)
        {
            _tomara.transform.localPosition = Vector2.MoveTowards(_tomara.transform.localPosition,
                new Vector2(_tomara.transform.localPosition.x, END_Y), Time.deltaTime * SPEED);

            yield return null;
        }
        
        // Vector3(-10.5699997,4.67999983,0)
        _tomara.GetComponent<Animator>().Play("Tomara Right Move");
        _tomara.GetComponent<SpriteRenderer>().flipX = true;
        var END_X = -10.5699997f;
        
        while (_tomara.transform.localPosition.x != END_X)
        {
            _tomara.transform.localPosition = Vector2.MoveTowards(_tomara.transform.localPosition,
                new Vector2(END_X, _tomara.transform.localPosition.y), Time.deltaTime * SPEED);

            yield return null;
        }
        
        // Vector3(-10.5699997,8.31000042,0)
        _tomara.GetComponent<SpriteRenderer>().flipX = false;
        _tomara.GetComponent<Animator>().Play("Tomara Up Move");
        END_Y = 11.31000042f;
        
        while (_tomara.transform.localPosition.y != END_Y)
        {
            _tomara.transform.localPosition = Vector2.MoveTowards(_tomara.transform.localPosition,
                new Vector2(_tomara.transform.localPosition.x, END_Y), Time.deltaTime * SPEED);

            yield return null;
        }
        
        Stats.Instance.TomaraCutscene = 11;
        gameObject.SetActive(false);
        Fedya.Instance.enabled = true;
    }
}

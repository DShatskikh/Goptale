using System.Collections;
using UnityEngine;

public sealed class TamaraCutscene_8 : MonoBehaviour
{
    [SerializeField]
    private GameObject _tomara;

    private IEnumerator Start()
    {
        if (Stats.Instance.TomaraCutscene > 13)
        {
            gameObject.SetActive(false);
            yield break;
        }
    } 

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.GetComponent<Fedya>())
            return;
        
        StartCoroutine(AwaitTrigger());
    }
    
    private IEnumerator AwaitTrigger()
    {
        Fedya.Instance.enabled = false;
        _tomara.GetComponent<Animator>().enabled = true;
        _tomara.GetComponent<Animator>().Play("Tomara Right");
        
        yield return DialogueWindow.StartDialogue(new [] {
            "\\T1Ты хочешь узнать, как вернуться \"Домой\"?",
            "\\T1Впереди нас находится единственный выход из РУИНИНО.",
            "\\T1Я собираюсь разрушить его.",
            "\\T1И тогда никто не сможет выбраться снова.",
            "\\T1Теперь будь хорошим пацанчиком и поднимись наверх.",
            "\\T1Ты очень наивен...*Если ты покинешь РУИНИНО...",
            "\\T1Ты свяжешься с плохой компанией.*И сядешь в тюрьму.",
            "\\T1Я лишь защищаю тебя, ты это понимаешь?",
            "\\T1... иди на вверх.",
            "\\T1Не пытайся меня остановить.",
            "\\T1Это последнее предупреждение.",
        });

        // Он...*<color="red">ПРЕЗИДЕНТ А.А.ДРИМУРР</color>...*Убьёт тебя.
        
        _tomara.GetComponent<Animator>().Play("Tomara Right Move");
        var END_X = 4.9000001f;
        var SPEED = 5;
        
        while (_tomara.transform.localPosition.x < END_X)
        {
            _tomara.transform.localPosition = Vector2.MoveTowards(_tomara.transform.localPosition,
                new Vector2(END_X, _tomara.transform.localPosition.y), Time.deltaTime * SPEED);

            yield return null;
        }
        
        _tomara.GetComponent<Animator>().Play("Tomara Up Move");
        var END_Y = 10.23999977f;
        
        while (_tomara.transform.localPosition.y != END_Y)
        {
            _tomara.transform.localPosition = Vector2.MoveTowards(_tomara.transform.localPosition,
                new Vector2(_tomara.transform.localPosition.x, END_Y), Time.deltaTime * SPEED);

            yield return null;
        }
        
        Fedya.Instance.enabled = true;
        Stats.Instance.TomaraCutscene = 14;
        gameObject.SetActive(false);
    }
}

using System.Collections;
using UnityEngine;

public sealed class TamaraCutscene_2 : MonoBehaviour
{
    [SerializeField]
    private GameObject _head;

    [SerializeField]
    private GameObject _tomara;

    [SerializeField]
    private Sprite _downTomara;
    
    [SerializeField]
    private GameObject _switchingLevel;

    [SerializeField]
    private Sprite _upTomara;

    [SerializeField]
    private Transform _german;
    
    [SerializeField]
    private GameObject _bottle;
    
    private IEnumerator Start()
    {
        if (Stats.Instance.TomaraCutscene == 1 && Stats.Instance.GermanState == 1)
        {
            _tomara.transform.position = new Vector3(-0.06f, 3.54f);
            _tomara.GetComponent<Animator>().enabled = false;
            _tomara.GetComponent<SpriteRenderer>().sprite = _downTomara;
        }
        
        if (Stats.Instance.GermanState == 3)
        {
            _german.transform.position = new Vector3(_german.transform.position.x, 1.2f);
            _german.eulerAngles = new Vector3(0, 0, -90);
        }

        if (Stats.Instance.TomaraCutscene >= 2)
        {
            _bottle.SetActive(false);
        }

        if (Stats.Instance.TomaraCutscene >= 3)
        {
            _tomara.SetActive(false);
            yield break;
        }
        
        yield return new WaitUntil(() => Fedya.Instance.enabled);
        Fedya.Instance.enabled = false;
        Fedya.Instance.SetFlex(true);
        
        if (Stats.Instance.TomaraCutscene < 2)
        {
            _switchingLevel.SetActive(false);

            // Дитя, знай, на человек. живущего в ПОДЗЕМЕЛЬЕ могут нападать монстры.
            // И тебе необходимо Быть готовым к такоя ситуации.
            // Однако,волноватьсяне стоит? Процесс прост.
            // Когда ты встретишь монстра ты вступишь БИТВУ.
            // Пока ты в БИТвЕ. постарайся завязать дружескую беседу.
            // Тяни время. И я приду. чтобы уладить конфликт.
            // Попробуя поговорить манекеном.

            // Поцанчик, знай, на человека, живущего в ПОДЗЁМКИНО, может напасть гопота.
            // И тебе необходимо быть готовым к такоя ситуации.
            // Однако, не ссы!*Всё просто.
            // Когда начнется МАХАЧ - начни базарить.
            // Тяни время, пока я не приду, чтобы разрулить ситуацию.
            // Попробуй побазарить с Германом.

            if (Stats.Instance.GermanState == 0)
            {
                _tomara.GetComponent<Animator>().Play("Tomara Right");
                
                yield return DialogueWindow.StartDialogue(new []
                {
                    "\\T1Пацанчик, знай в ПОДЗЁМКИНО на тебя может напасть гопота.",
                    "\\T1И тебе необходимо быть готовым к такой ситуации.",
                    "\\T1Однако, не ссы!*Всё просто.",
                    "\\T1Когда начнется МАХАЧ - начни БАЗАР.",
                    "\\T1Тяни время, пока я не приду, чтобы разрулить ситуацию.",
                    "\\T1Попробуй побазарить с Германом.",
                });
                
                _tomara.GetComponent<SpriteRenderer>().flipX = false;
                _tomara.GetComponent<Animator>().Play("Tomara Right Move");

                while (_tomara.transform.position != new Vector3(-0.06f, 3.54f))
                {
                    _tomara.transform.position = Vector2.MoveTowards(_tomara.transform.position,
                        new Vector3(-0.06f, 3.54f), Time.deltaTime * 5f);

                    yield return null;
                }

                //_tomara.GetComponent<Animator>().SetTrigger("Stop");
                _tomara.GetComponent<Animator>().Play("Tomara Down");
                Stats.Instance.GermanState = 1;
            }
            
            //_tomara.GetComponent<Animator>().enabled = false;
            //_tomara.GetComponent<SpriteRenderer>().sprite = _downTomara;
        }
        else if (Stats.Instance.TomaraCutscene < 3)
        {
            _tomara.transform.position = new Vector3(-0.06f, 3.54f);
            Fedya.Instance.transform.position = new Vector3(1.64999998f,0.930000007f,0);
            
            _tomara.GetComponent<Animator>().Play("Tomara Down");
            
            if (Stats.Instance.GermanState != 3)
            {
                yield return DialogueWindow.StartDialogue(new[]
                {
                    "\\T1Молодец.*Пошли дальше."
                });
            }
            else
            {
                yield return DialogueWindow.StartDialogue(new[]
                {
                    "\\T1ГЕРМАН!*НЕЕЕТ!",
                    $"\\T1{Stats.Instance.Name} неужели у тебя настолько сильное похмелье?",
                    "\\T2Постарайся больше никого не бить.",
                    // "\\T1Постарайся никого больше не бить.",
                    "\\T1Пошли дальше."
                });
            }
            
            _tomara.GetComponent<Animator>().enabled = false;
            _tomara.GetComponent<SpriteRenderer>().sprite = _upTomara;
        
            while (_tomara.GetComponent<SpriteRenderer>().color.a > 0)
            {
                var color = _tomara.GetComponent<SpriteRenderer>().color;
                color.a -= Time.deltaTime;
                _tomara.GetComponent<SpriteRenderer>().color = color;
            
                yield return null;
            }
        
            _tomara.SetActive(false);
            Stats.Instance.TomaraCutscene = 3;
        }
        
        Fedya.Instance.enabled = true;
    }
}

using System;
using System.Collections;
using UnityEngine;

public sealed class TomaraCutscene_1 : MonoBehaviour
{
    [SerializeField]
    private Transform _spawnPoint;
    
    [SerializeField]
    private GameObject _tomara;

    [SerializeField]
    private AudioClip _sfxTomara;

    [SerializeField]
    private GameObject _shards;
    
    [SerializeField]
    private AudioClip _sfxRuinsTheme, _sfxTomaraTheme;
    
    private IEnumerator Start()
    {
        if (Stats.Instance.TomaraCutscene != 0)
        {
            gameObject.SetActive(false);
            _shards.SetActive(true);
            MusicManager.Instance.Play(_sfxRuinsTheme);
            yield break;
        }
        
        MusicManager.Instance.Play(_sfxTomaraTheme);
        _tomara.GetComponent<Animator>().Play("Tomara Right");
        
        Fedya.Instance.transform.position = _spawnPoint.position;
        Fedya.Instance.enabled = false;
        Fedya.Instance.SetFlex(true);
        
        yield return new WaitForSeconds(1);

        // DialogueWindow.AudioClip = _sfxTomara;
        yield return DialogueWindow.StartDialogue("\\T1Ооо %бедный беззащитный пацанчик.");
        
        _tomara.GetComponent<Animator>().Play("Tomara Rozochka");
        yield return null;
        yield return new WaitUntil(() => _tomara.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1);
        
        // игрок получает розочку
        
        yield return DialogueWindow.StartDialogue(new []
        {
            "Вы получили <color=\"yellow\">РОЗОЧКУ</color>."
        });

        _tomara.GetComponent<Animator>().Play("Tomara Right");
        
        yield return DialogueWindow.StartDialogue(new []
        {
            "\\T1Вот теперь уже не беззащитный.",
            "\\T1Меня зовут тётя Тома.",
            "\\T1Что ты тут делаешь?*С похмелья валяешься?",
            "\\T1Ну ничего.%*Пойдем со мной.",
            "\\T1Я дам тебе <color=\"yellow\">АНТИПОХМЕЛИН</color>.",
        });
        
        // Stats.Instance.TryAddItem("РОЗОЧКА");
        _tomara.GetComponent<SpriteRenderer>().flipX = false;
        yield return new WaitForSeconds(1);
        _tomara.GetComponent<Animator>().Play("Tomara Right Move");
        
        while (_tomara.transform.position.x < 7.47f)
        {
            _tomara.transform.position = Vector2.MoveTowards(_tomara.transform.position,
                new Vector2(7.47f, _tomara.transform.position.y), Time.deltaTime * 3f);
            
            yield return null;
        }
        
        _tomara.GetComponent<Animator>().SetTrigger("Stop");
        
        Fedya.Instance.enabled = true;
        Stats.Instance.TomaraCutscene = 1;
        SaveSystem.Save();
        
        while (_tomara.GetComponent<SpriteRenderer>().color.a > 0)
        {
            var color = _tomara.GetComponent<SpriteRenderer>().color;
            color.a -= Time.deltaTime;
            _tomara.GetComponent<SpriteRenderer>().color = color;
            
            yield return null;
        }
    }
}

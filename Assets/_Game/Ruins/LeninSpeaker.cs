using System.Collections;
using UnityEngine;

public sealed class LeninSpeaker : MonoBehaviour
{
    [SerializeField]
    private SpikeManager _spikeManager;

    private bool _isProgress;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.GetComponent<Fedya>())
            return;

        if (_isProgress)
            return;
     
        if (_spikeManager.IsActivate)
            return;
        
        _isProgress = true;
        StartCoroutine(Await());
    }

    private IEnumerator Await()
    {
        Fedya.Instance.enabled = false;
        GetComponent<Lenin>().enabled = false;
        
        yield return DialogueWindow.StartDialogue(new [] {
            "Товарищь!%*Вы что себе позволяете?!",
            "Что?%*Вы хотите построить комунизм?",
            "Куда мне нужно двинуться?",
        });
        
        while (transform.position != new Vector3(2.44000006f, -1.84f))
        {
            transform.position = Vector2.MoveTowards(transform.position, 
                new Vector3(2.44000006f, -1.84f), Time.deltaTime * 2);
                        
            yield return null;
        }

        yield return new WaitForSeconds(1);
        
        yield return DialogueWindow.StartDialogue(new [] {
            "В другую сторону говорите?*Товарищь вы меня запутали",
        });
        
        while (transform.position != new Vector3(2.44000006f, -2.72600007f))
        {
            transform.position = Vector2.MoveTowards(transform.position, 
                new Vector3(2.44000006f, -2.72600007f), Time.deltaTime * 2);
                        
            yield return null;
        }
        
        yield return new WaitForSeconds(1);
        
        while (transform.position != new Vector3(4.51f, -2.72600007f))
        {
            transform.position = Vector2.MoveTowards(transform.position, 
                new Vector3(4.51f, -2.72600007f), Time.deltaTime * 2);
                        
            yield return null;
        }
        
        GetComponent<Lenin>().IsActivate = true;
        Fedya.Instance.enabled = true;
        
        yield return new WaitUntil(() => Fedya.Instance.transform.position.x > 6);
        Fedya.Instance.enabled = false;
        
        while (transform.position != new Vector3(3.52f, -2.72600007f))
        {
            transform.position = Vector2.MoveTowards(transform.position, 
                new Vector3(3.52f, -2.72600007f), Time.deltaTime * 2);
                        
            yield return null;
        }
        
        GetComponent<Lenin>().IsActivate = false;
        _spikeManager.Activate();
        
        yield return DialogueWindow.StartDialogue(new [] {
            "А?*Тут и оставаться?",
            "Вперед в КОМУНИЗМ!!!",
        });

        while (transform.position != new Vector3(4.51f, -2.72600007f))
        {
            transform.position = Vector2.MoveTowards(transform.position, 
                new Vector3(4.51f, -2.72600007f), Time.deltaTime * 2);
                        
            yield return null;
        }
        
        GetComponent<Lenin>().IsActivate = true;
        Fedya.Instance.enabled = true;
    }
}

using System.Collections;
using UnityEngine;

public sealed class Savepoint : Usable
{
    [SerializeField]
    private string _message;
    
    public override void Use()
    {
        Fedya.Instance.enabled = false;
        StartCoroutine(Await());
    }

    private IEnumerator Await()
    {
        Stats.Instance.HP = Stats.Instance.MaxHP;
        
        if (!Stats.Instance.IsGenocide)
        {
            yield return DialogueWindow.StartDialogue(_message);
        }
        else
        {
            var count = 12 - Stats.Instance.Kills;

            if (count > 0)
            {
                yield return DialogueWindow.StartDialogue($"<color=\"red\">Еще {count}");
            }
            else
            {
                yield return DialogueWindow.StartDialogue($"<color=\"red\">Четкость");
            }
        }
        
        Instantiate(Resources.Load<GameObject>("Save Window"));
    }
}

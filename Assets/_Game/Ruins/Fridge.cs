using System.Collections;
using UnityEngine;

public sealed class Fridge : Usable
{
    [SerializeField]
    private GameObject _head;
    
    public override void Use()
    {
        Fedya.Instance.enabled = false;
        StartCoroutine(Await());
    }

    private IEnumerator Await()
    {
        if (Stats.Instance.TomaraCutscene == 3)
        {
            yield return DialogueWindow.StartDialogue(new [] {
                "Кажется кто-то прячется в этом холодильнике.", 
                "\\T1^0.3.^0.3.^0.3."});
        }
        else
        {
            yield return DialogueWindow.StartDialogue(new [] {
                "Старый СОВЕТСКИЙ! холодильник.",
                "Сейчас таких не делают."
            });
        }
        
        Fedya.Instance.enabled = true;
    }
}

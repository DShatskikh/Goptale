using System.Collections;
using UnityEngine;

public sealed class Mirror : Usable
{
    public override void Use()
    {
        Fedya.Instance.enabled = false;
        CoroutineRunner.Instance.StartCoroutine(AwaitUse());
    }
    
    private IEnumerator AwaitUse()
    {
        yield return DialogueWindow.StartDialogue(new []
        {
            $"Это {Stats.Instance.Name}."
        });
            
        Fedya.Instance.enabled = true;
    }
}

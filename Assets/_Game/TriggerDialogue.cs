using System.Collections;
using UnityEngine;

public sealed class TriggerDialogue : MonoBehaviour
{
    [SerializeField]
    private string[] _dialogues;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!other.gameObject.GetComponent<Fedya>())
            return;
        
        Fedya.Instance.enabled = false;
        StartCoroutine(AwaitUse());
    }

    private IEnumerator AwaitUse()
    {
        yield return  DialogueWindow.StartDialogue(_dialogues);
        Fedya.Instance.enabled = true;
    }
}

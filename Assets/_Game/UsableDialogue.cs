using System.Collections;
using UnityEngine;

public sealed class UsableDialogue : Usable
{
    [SerializeField]
    [TextArea]
    private string[] _dialogues;
    
    [SerializeField]
    [TextArea]
    private string[] _alternativeDialogues;
    
    private bool _isUsable;
    
    public void SetDialogues(string[] dialogues)
    {
        _dialogues = dialogues;
    }

    public override void Use()
    {
        Fedya.Instance.enabled = false;
        StartCoroutine(AwaitUse());
    }

    private IEnumerator AwaitUse()
    {
        var dialogues = _isUsable && _alternativeDialogues.Length != 0 ? _alternativeDialogues : _dialogues;
        yield return  DialogueWindow.StartDialogue(dialogues);
        Fedya.Instance.enabled = true;
        _isUsable = true;
    }
}

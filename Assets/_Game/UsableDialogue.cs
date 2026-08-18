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
    
    [SerializeField]
    private bool _isDown;
    
    private bool _isUsable;
    
    public void SetDialogues(string[] dialogues)
    {
        _dialogues = dialogues;
    }

    public override void Use()
    {
        if (DialogueWindow.Instance)
            return;
        
        Fedya.Instance.enabled = false;
        StartCoroutine(AwaitUse());
    }

    private IEnumerator AwaitUse()
    {
        var dialogues = _isUsable && _alternativeDialogues.Length != 0 ? _alternativeDialogues : _dialogues;
        yield return  DialogueWindow.StartDialogue(dialogues, _isDown);
        Fedya.Instance.enabled = true;
        _isUsable = true;
    }
}

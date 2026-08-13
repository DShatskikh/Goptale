using System.Collections;
using UnityEngine;

public sealed class UsableDialoguePair : Usable
{
    [SerializeField]
    private string[] _dialogues;
    
    [SerializeField]
    private string[] _pairDialogues;

    [SerializeField]
    private UsableDialoguePair _pair;
    
    public bool IsActivate;
    
    public override void Use()
    {
        Fedya.Instance.enabled = false;
        StartCoroutine(AwaitUse());
    }

    private IEnumerator AwaitUse()
    {
        var dialogues = _pair.IsActivate ? _pairDialogues : _dialogues;
        yield return  DialogueWindow.StartDialogue(dialogues);
        Fedya.Instance.enabled = true;

        if (!_pair.IsActivate)
        {
            IsActivate = true;
        }
    }
}

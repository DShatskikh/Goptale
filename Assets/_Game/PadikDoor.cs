using System.Collections;
using UnityEngine;

public sealed class PadikDoor : Usable
{
    [SerializeField]
    private SwitchingLevel _padikSwitchingLevel, _podval;
    
    public override void Use()
    {
        Fedya.Instance.enabled = false;
        StartCoroutine(Await());
    }
    
    private IEnumerator Await()
    {
        var selectDialogue = SelectionWindow.StartDialogue($"Куда пойти?", "Подвал", $"Падик");
        yield return new WaitUntil(() => selectDialogue == null);

        if (!SelectionWindow.IsRight)
        {
            CoroutineRunner.Instance.StartCoroutine(_podval.AwaitSwitching());
        }
        else
        {
            CoroutineRunner.Instance.StartCoroutine(_padikSwitchingLevel.AwaitSwitching());
        }
    }
}

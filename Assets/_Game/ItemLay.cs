using System.Collections;
using UnityEngine;

public sealed class ItemLay : Usable
{
    [SerializeField]
    private string _name;

    [SerializeField]
    private string _id;

    private void Start()
    {
        if (Stats.Instance.LayItemIDs.Contains(_id))
        {
            Destroy(gameObject);
        }
    }

    public override void Use()
    {
        Fedya.Instance.enabled = false;
        CoroutineRunner.Instance.StartCoroutine(AwaitUse());
    }

    private IEnumerator AwaitUse()
    {
        yield return null;
        
        if (DialogueWindow.Instance)
            yield break;
        
        yield return DialogueWindow.StartDialogue(new []
        {
            $"Вы нашли {_name}."
        });

        if (Stats.Instance.TryAddItem(_name))
        {
            Stats.Instance.LayItemIDs.Add(_id);
            Destroy(gameObject);
        }
        else
        {
            yield return DialogueWindow.StartDialogue(new []
            {
                "У вас недостаточно места."
            });   
        }
        
        Fedya.Instance.enabled = true;
    }
}

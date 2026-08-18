using System.Collections;
using UnityEngine;

public class MashaJam : Usable
{
    private const int PRICE = 12;
    
    private void Start()
    {
        if (Stats.Instance.MashaShop[0])
            gameObject.SetActive(false);
    }
    
    public override void Use()
    {
        Fedya.Instance.enabled = false;
        CoroutineRunner.Instance.StartCoroutine(AwaitUse());
    }

    private IEnumerator AwaitUse()
    {
        yield return null;
        
        if (DialogueWindow.Instance != null)
            yield break;
        
        if (Stats.Instance.MashaShop[0])
        {
            yield return DialogueWindow.StartDialogue("Всё раскупили.", true);
            Fedya.Instance.enabled = true;
            yield break;
        }
        
        var moneyWindow = MoneyWindow.Open();
        var selectDialogue = SelectionWindow.StartDialogue($"Купить паучье варенье за {PRICE}РУБ?", $"Да", "Нет", true);
        yield return new WaitUntil(() => selectDialogue == null);
        Destroy(moneyWindow.gameObject);
        
        if (!SelectionWindow.IsRight) // купить
        {
            if (Stats.Instance.RUB >= PRICE)
            {
                if (Stats.Instance.TryAddItem($"{Constants.MASHA_JAM}"))
                {
                    Stats.Instance.RUB -= PRICE;
                
                    yield return DialogueWindow.StartDialogue(new[]
                    {
                        $"Вы купили паучье варенье."
                    }, true);

                    Stats.Instance.MashaShop[0] = true;
                    Destroy(gameObject);
                }
                else
                {
                    yield return DialogueWindow.StartDialogue(new[]
                    {
                        "У вас нет места."
                    }, true);
                }
            }
            else
            {
                yield return DialogueWindow.StartDialogue(new[]
                {
                    "Вам не хватает денег."
                }, true);
            }

            yield return null;
            Fedya.Instance.enabled = true;
        }
        else
        {
            Fedya.Instance.enabled = true;
        }
    }
}
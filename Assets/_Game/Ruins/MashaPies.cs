using System;
using System.Collections;
using UnityEngine;

public class MashaPies : Usable
{
    private const int PRICE = 6;

    private void Start()
    {
        if (Stats.Instance.MashaShop[1])
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
            
        if (Stats.Instance.MashaShop[1])
        {
            yield return DialogueWindow.StartDialogue("Всё раскупили.");
            Fedya.Instance.enabled = true;
            yield break;
        }

        var moneyWindow = MoneyWindow.Open();
        var selectDialogue = SelectionWindow.StartDialogue($"Купить {Constants.MASHA_PIES} {PRICE}РУБ?", $"Да", "Нет", true);
        yield return new WaitUntil(() => selectDialogue == null);
        Destroy(moneyWindow.gameObject);
        
        if (!SelectionWindow.IsRight) // купить
        {
            if (Stats.Instance.RUB >= PRICE)
            {
                if (Stats.Instance.TryAddItem($"{Constants.MASHA_PIES}"))
                {
                    Stats.Instance.RUB -= PRICE;
                
                    yield return DialogueWindow.StartDialogue(new[]
                    {
                        $"Вы получили <color=\"yellow\">{Constants.MASHA_PIES}</color>."
                    });

                    Stats.Instance.MashaShop[1] = true;
                    Destroy(gameObject);
                }
                else
                {
                    yield return DialogueWindow.StartDialogue(new[]
                    {
                        "У вас нет места."
                    });
                }
            }
            else
            {
                yield return DialogueWindow.StartDialogue(new[]
                {
                    "Вам не хватает денег."
                });
            }
            
            Fedya.Instance.enabled = true;
        }
        else
        {
            Fedya.Instance.enabled = true;
        }
    }
}
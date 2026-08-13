using System.Collections;
using UnityEngine;

public sealed class GasterEasterEgg : MonoBehaviour
{
    [SerializeField]
    private AudioSource _sfx;
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!other.gameObject.GetComponent<Fedya>())
            return;

        StartCoroutine(Await());
    }

    private IEnumerator Await()
    {
        Fedya.Instance.enabled = false;
        
        if (Stats.Instance.IsGasterEgg)
        {
            yield return DialogueWindow.StartDialogue("Тут больше никого нет.");
        }
        else
        {
            yield return DialogueWindow.StartDialogue(new []
            {
                "Вы встретили Гастера.%*Он налил вам настойку.",
                "Вы бухаете с Гастером.",
            });

            if (Stats.Instance.TryAddItem(Constants.NASTOYKA_GASTERA))
            {
                yield return DialogueWindow.StartDialogue(new []
                {
                    "Вы получили <color=\"yellow\">Настойку Гастера</color>.",
                });
                
                Stats.Instance.IsGasterEgg = true;  
            }
            
            _sfx.Play();

            yield return DialogueWindow.StartDialogue(new []
            {
                "Тут больше никого нет."
            });
        }
        
        Fedya.Instance.enabled = true;
    }
}

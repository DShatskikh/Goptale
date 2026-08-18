using UnityEngine;

public class TamaraSpeaker : MonoBehaviour
{
    private void Update()
    {
        GetComponent<Animator>().SetBool("IsSpeak", DialogueWindow.IsSpeak('T'));
    }
}

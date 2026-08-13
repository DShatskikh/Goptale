using UnityEngine;

public class TomaraSpeaker : MonoBehaviour
{
    private void Update()
    {
        GetComponent<Animator>().SetBool("IsSpeak", DialogueWindow.IsSpeak('T'));
    }
}

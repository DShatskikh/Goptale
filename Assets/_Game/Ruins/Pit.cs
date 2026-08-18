using System.Collections;
using UnityEngine;

public class Pit : MonoBehaviour
{
    [SerializeField]
    private AudioSource _fallSFX;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.GetComponent<Fedya>())
            return;

        StartCoroutine(Await());
    }

    private IEnumerator Await()
    {
        Fedya.Instance.enabled = false;
        Fedya.Instance.GetComponent<Collider2D>().enabled = false;
        Fedya.Instance.GetComponentInChildren<Animator>().Play("PlayerFall");
        _fallSFX.Play();
        
        var fallY = -16.69f;
        var SPEED = 5;
        while (Fedya.Instance.transform.position.y > fallY)
        {
            Fedya.Instance.transform.position = Vector2.MoveTowards(Fedya.Instance.transform.position,
                new Vector2(Fedya.Instance.transform.position.x, fallY), Time.deltaTime * SPEED);
            
            yield return null;
        }
        
        Fedya.Instance.GetComponentInChildren<Animator>().Play("Movement");
        Fedya.Instance.enabled = true;
        Fedya.Instance.GetComponent<Collider2D>().enabled = true;
    }
}

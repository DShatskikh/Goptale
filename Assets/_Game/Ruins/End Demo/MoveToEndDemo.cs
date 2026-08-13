using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class MoveToEndDemo : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer _background;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.GetComponent<Fedya>())
            return;
        
        Fedya.Instance.enabled = false;
        StartCoroutine(AwaitTrigger());
    }

    private IEnumerator AwaitTrigger()
    {
        MusicManager.Instance.Stop();
        _background.gameObject.SetActive(true);
        var alpha = 0f;

        while (alpha < 1f)
        {
            _background.color = new Color(1f, 1f, 1f, alpha);
            alpha += Time.deltaTime / 2;
            yield return null;
        }

        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(35);
    }
}

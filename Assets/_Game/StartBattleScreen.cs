using System.Collections;
using UnityEngine;

public sealed class StartBattleScreen : MonoBehaviour
{
    [SerializeField]
    private Transform _heart;

    [SerializeField]
    private AudioSource _noiseSFX;
    
    [SerializeField]
    private AudioSource _battleFallSFX;
    
    public bool IsEnd;
    
    private IEnumerator Start()
    {
        Fedya.Instance.GetComponentInChildren<SpriteRenderer>().sortingOrder = 19;
        _heart.position = Fedya.Instance.transform.position + new Vector3(0f, 0.5f);
        _heart.gameObject.SetActive(true);

        for (int i = 0; i < 3; i++)
        {
            yield return new WaitForSeconds(0.1f);
            _heart.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.1f);
            _heart.gameObject.SetActive(true);
            _noiseSFX.Play();
        }

        yield return new WaitForSeconds(0.1f);
        _battleFallSFX.Play();
        Fedya.Instance.gameObject.SetActive(false);

        var koef = 6.75f / 6;
        
        var endPosition = new Vector2(-6.83f * koef, -5.321f * koef);

        while (endPosition != (Vector2)_heart.localPosition)
        {
            yield return null;
            _heart.localPosition = Vector2.MoveTowards(_heart.localPosition, endPosition, Time.deltaTime * 9);
        }

        IsEnd = true;
    }
}

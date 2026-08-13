using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public sealed class TextBubble : MonoBehaviour
{
    public static AudioClip AudioClip;
    private const float START_DURATION = 0.05f;
    private const float START_TIMER = 7f;
    
    private static bool _isSkip;
    private static bool _isClose;
    private static float _timer = START_TIMER;
    private static List<TextBubble> _allBubbles = new();
    
    [SerializeField]
    private TMP_Text _label;

    [SerializeField]
    private AudioClip _defaultSFX;
    
    private string _text;
    public bool IsAnimated;
    public float Duration = START_DURATION;
    private bool _isFrameSkip = true;
    private bool _isEnd;
    private bool _isAlpha => _allBubbles.Count > 0 && this == _allBubbles[0];

    public bool IsInit => !string.IsNullOrEmpty(_text);

    private void Awake()
    {
        _timer = START_TIMER;
        _allBubbles.Add(this);
    }

    private void OnDestroy()
    {
        _allBubbles.Remove(this);
    }

    private void Update()
    {
        if (_isFrameSkip)
        {
            _isFrameSkip = false;
            return;
        }
        
        if (Input.GetButtonDown("Submit") && _isEnd)
        {
            _isClose = true;
        }
        
        if (Input.GetButtonDown("Cancel"))
        {
            _isSkip = true;
        }

        if (_isAlpha)
        {
            if (_isEnd)
                _timer -= Time.deltaTime;
            else
                _timer = START_TIMER;

            if (_timer < 0f)
            {
                _isClose = true;
            }
        }
    }

    public void SetText(string text)
    {
        _text = text;
        _label.text = string.Empty;
        _isSkip = false;
        _isClose = false;
        _isFrameSkip = true;

        StartCoroutine(AwaitWrite());
    }

    private IEnumerator AwaitWrite()
    {
        GetComponent<AudioSource>().clip = AudioClip;
        
        for (int i = 0; i < _text.Length; i++)
        {
            if (!_isSkip)
                yield return new WaitForSeconds(Duration);

            switch (_text[i])
            {
                case '\n':
                    _label.text += _text[i];
                    IsAnimated = true;
                    break;
                case '%':
                    if (!_isSkip)
                        yield return new WaitForSeconds(0.5f);
                    IsAnimated = false;
                    break;
                default:
                    _label.text += _text[i];
                    IsAnimated = true;
                    
                    if (!_isSkip && !GetComponent<AudioSource>().isPlaying)
                        GetComponent<AudioSource>().Play();
                    
                    break;
            }
        }
        
        IsAnimated = false;
        Duration = START_DURATION;
        _isEnd = true;
        yield return new WaitUntil(() => _isClose);
        
        foreach (var textBubble in FindObjectsOfType<TextBubble>())
        {
            Destroy(textBubble.gameObject);
        }

        AudioClip = _defaultSFX;
    }
}

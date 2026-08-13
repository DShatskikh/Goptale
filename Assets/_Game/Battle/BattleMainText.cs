using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public sealed class BattleMainText : MonoBehaviour
{
    private const float START_DURATION = 0.05f;
    
    private TMP_Text _label;
    private string[] _texts;
    private float _duration = START_DURATION;
    private bool _isEndTurn;
    private bool _isSkip;
    private List<GameObject> _additions = new();
    private Coroutine _coroutine;
    public bool IsSkip => _isSkip;
    
    private void Awake()
    {
        _label = GetComponent<TMP_Text>();
    }

    private void OnDisable()
    {
        
    }

    private void Update()
    {
        if (_isEndTurn && Input.GetButtonDown("Cancel"))
        {
            _isSkip = true;
        }
    }

    public void SetText(string text, bool isEndTurn)
    {
        SetText(new []{text}, isEndTurn);
    }

    public void SetText(string[] texts, bool isEndTurn)
    {
        for (int i = 0; i < _additions.Count; i++)
        {
            Destroy(_additions[i]);
        }
        
        _additions.Clear();
        _label = GetComponent<TMP_Text>();
        
        _texts = texts;
        _isSkip = false;
        gameObject.SetActive(true);
        _label.text = string.Empty;
        
        if (_coroutine != null)
            CoroutineRunner.Instance.StopCoroutine(_coroutine);
        
        _coroutine = CoroutineRunner.Instance.StartCoroutine(AwaitWrite());
        _isEndTurn = isEndTurn;
    }

    public void ClearText()
    {
        if (_coroutine != null)
            CoroutineRunner.Instance.StopCoroutine(_coroutine);
        
        gameObject.SetActive(false);
    }
    
    private IEnumerator AwaitWrite()
    {
        var j = 0;
        
        foreach (var text in _texts)
        {
            _isSkip = false;
            j++;
            
            for (int i = 0; i < text.Length; i++)
            {
                if (!_isSkip)
                    yield return new WaitForSeconds(_duration);

                switch (text[i])
                {
                    case '\n':
                        _label.text += text[i];

                        break;
                    case '%':
                        if (!_isSkip)
                            yield return new WaitForSeconds(0.5f);

                        break;
                    
                    case '#':
                        _duration = 0.2f;

                        break;
                    case '<':
                        while (text[i] != '>')
                        {
                            _label.text += text[i];
                            i++;
                        }
                        
                        _label.text += text[i];

                        break;
                    case '*':
                        var y = 0f;

                        if (_label == GetComponent<TMP_Text>())
                        {
                            if (_label.preferredHeight > 1)
                            {
                                y = -_label.preferredHeight / 2;
                            }
                            else
                            {
                                y = 0;
                            }
                        }
                        else
                        {
                            y = -0.97f;
                        }
                        
                        var actionLabel = Instantiate(Resources.Load<ActionLabel>("Action Label"),
                            _label.transform.position + new Vector3(0f, y, 0), transform.rotation, transform); // 0.1192f
                        
                        // _label.preferredHeight / 2
                        
                        actionLabel.GetComponent<RectTransform>().sizeDelta = new Vector2(12.81f, 0.7664f);
                        actionLabel.Star.transform.localPosition = new Vector3(-6.67f, 0f, 0);
                        _additions.Add(actionLabel.gameObject);
                        _label = actionLabel.Label;
                        _label.text = string.Empty;

                        break;
                    default:
                        _label.text += text[i];

                        break;
                }
            }

            if (j < _texts.Length)
            {
                yield return new WaitUntil(() => Input.GetButtonDown("Submit"));
                
                for (int i = 0; i < _additions.Count; i++)
                {
                    Destroy(_additions[i]);
                }
        
                _additions.Clear();
                _label = GetComponent<TMP_Text>();
                _label.text = string.Empty;
            }
            
            _duration = START_DURATION;
        }

        if (_isEndTurn)
        {
            yield return new WaitUntil(() => Input.GetButtonDown("Submit"));

            for (int i = 0; i < _additions.Count; i++)
            {
                Destroy(_additions[i]);
            }
        
            _additions.Clear();
            _label = GetComponent<TMP_Text>();
            
            gameObject.SetActive(false);
            
            BattleManager.Instance.TargetSizeFrame = BattleManager.Instance.AttackFrameSize_1;
            yield return null;
            //yield return new WaitForSeconds(3);
            yield return new WaitUntil(() => BattleManager.Instance.IsEndAnimationFrame);
            
            // BattleManager.Instance.IsEndAnimationFrame = false;
            BattleManager.Instance.IsEnemyTurn = true;
        }
        
        _isSkip = true;
    }
}

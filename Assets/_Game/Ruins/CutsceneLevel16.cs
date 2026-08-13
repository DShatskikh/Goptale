using System;
using System.Collections;
using UnityEngine;

public sealed class CutsceneLevel16 : MonoBehaviour
{
    [SerializeField]
    private SwitchingLevel _switchingLevel;

    [SerializeField]
    private Transform _tomaraHace, _tomara;

    private bool _isActivate;

    private void Start()
    {
        if (Stats.Instance.TomaraCutscene >= 13)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.GetComponent<Fedya>())
            return;

        if (_isActivate)
            return;
        
        _isActivate = true;
        StartCoroutine(AwaitTrigger());
    }

    private IEnumerator AwaitTrigger()
    {
        Fedya.Instance.enabled = false;
        _tomara.gameObject.SetActive(true);
        _tomara.gameObject.GetComponent<Animator>().enabled = true;
        _tomara.gameObject.GetComponent<Animator>().Play("Tomara Right Move");
        
        var END_X = -3.23f;
        var END_Y = -0.55f;
        var SPEED = 3;
        
        while (_tomara.transform.localPosition.x != END_X && _tomara.transform.localPosition.y != END_Y)
        {
            _tomara.transform.localPosition = Vector2.MoveTowards(_tomara.transform.localPosition,
                new Vector2(END_X, END_Y), Time.deltaTime * SPEED);

            yield return null;
        }
        
        END_X = -1.87f;
        
        while (_tomara.transform.localPosition.x != END_X)
        {
            _tomara.transform.localPosition = Vector2.MoveTowards(_tomara.transform.localPosition,
                new Vector2(END_X, END_Y), Time.deltaTime * SPEED);

            yield return null;
        }
        
        _tomara.gameObject.GetComponent<Animator>().Play("Tomara Right");
        
        yield return DialogueWindow.StartDialogue(new [] {
            "\\T1Идём отсюда.",
        });
        
        yield return new WaitForSeconds(1);
        _tomara.gameObject.SetActive(false);
        _tomaraHace.gameObject.SetActive(true);
        Fedya.Instance.gameObject.SetActive(false);
        
        // yield return new WaitForSeconds(1);
        Fedya.Instance.transform.position = new Vector3(-100, Fedya.Instance.transform.position.y);
        Fedya.Instance.gameObject.SetActive(true);
        
        CoroutineRunner.Instance.StartCoroutine(_switchingLevel.AwaitSwitching());
        
        END_X = -2.87f;
        
        while (_tomaraHace.transform.localPosition.x != END_X)
        {
            _tomaraHace.transform.localPosition = Vector2.MoveTowards(_tomaraHace.transform.localPosition,
                new Vector2(END_X, _tomaraHace.transform.localPosition.y), Time.deltaTime * SPEED);

            yield return null;
        }
        
        _tomaraHace.gameObject.GetComponent<Animator>().enabled = false;
    }
}

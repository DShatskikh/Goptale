using System;
using System.Collections;
using TMPro;
using UnityEngine;

public sealed class QuitingScreen : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _label;

    private Coroutine _coroutine;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (Camera.main)
        {
            if (BattleManager.Instance)
            {
                transform.position = new Vector3(Camera.main.transform.position.x + -5.61f, Camera.main.transform.position.y + 5.28f);
            }
            else
            {
                transform.position = new Vector3(Camera.main.transform.position.x + -6.67f, Camera.main.transform.position.y + 6.01f);
            }
        }
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _coroutine = StartCoroutine(AwaitQuitting());
        }

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (_coroutine != null)
            {
                _label.color = new Color(1, 1, 1, 0);
                StopCoroutine(_coroutine);
                _coroutine = null;
            }
        }
    }

    private IEnumerator AwaitQuitting()
    {
        _label.text = "Ну нахрен";
        var alpha = 0f;
        
        while (alpha < 1)
        {
            alpha += Time.deltaTime;
            _label.color = new Color(1, 1, 1, alpha);
            yield return null;
        }

        _label.text += '.';
        yield return new WaitForSeconds(0.3f);
        _label.text += '.';
        yield return new WaitForSeconds(0.3f);
        _label.text += '.';
        yield return new WaitForSeconds(0.3f);
        Application.Quit();
    }
}
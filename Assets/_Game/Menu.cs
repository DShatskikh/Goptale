using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class Menu : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _continue, _fullReset, _info;
    
    [SerializeField]
    private AudioSource _sfx;

    [SerializeField]
    private GameObject _default;
    
    private bool _isRight;

    private void Start()
    {
        var levelName = Stats.GetLevelName(Stats.Instance.LevelName);
        var seconds = (int)Stats.Instance.Time % 60;
        var minutes = (int)Stats.Instance.Time / 60;
        _info.text = $"{Stats.Instance.Name}    УР{Stats.Instance.LV}    {minutes}:{seconds:D2}\n{levelName}";
        
        if (Stats.Instance.IsGenocide)
            _default.SetActive(false);
    }

    private void Update()
    {
        _continue.color = _isRight ? Color.white : Color.yellow;
        _fullReset.color = !_isRight ? Color.white : Color.yellow;

        if (Input.GetButtonDown("Horizontal"))
        {
            if (Input.GetAxisRaw("Horizontal") > 0)
            {
                if (!_isRight)
                {
                    _isRight = true;
                    _sfx.Play();
                }
            }
            else if (Input.GetAxisRaw("Horizontal") < 0)
            {
                if (_isRight)
                {
                    _isRight = false;
                    _sfx.Play();
                }
            }
        }
        else if (Input.GetButtonDown("Submit"))
        {
            SceneManager.UnloadScene("Menu");
            
            if (!_isRight)
            {
                CoroutineRunner.Instance.StartCoroutine(AwaitLoad());
                enabled = false;
            }
            else
            {
                SaveSystem.DeleteSave();
                Stats.Instance = Stats.GetDefault();
                SceneManager.LoadSceneAsync("ScreenSaver",  LoadSceneMode.Single);
            }
        }
    }

    private IEnumerator AwaitLoad()
    {
        Stats.Instance = SaveSystem.Load();
        yield return SceneManager.LoadSceneAsync("Overworld",  LoadSceneMode.Single);
        Fedya.Instance.transform.position = Stats.Instance.Position;
        yield return SceneManager.LoadSceneAsync(Stats.Instance.LevelName,  LoadSceneMode.Additive);
        Fedya.IsLoad = true;
    }
}

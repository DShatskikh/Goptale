using TMPro;
using UnityEngine;

public sealed class SaveWindow : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _label;
    
    [SerializeField]
    private AudioSource _selectSFX;
    
    [SerializeField]
    private AudioSource _saveSFX;

    [SerializeField]
    private Transform _heart;

    [SerializeField]
    private GameObject _buttonContainer;
    
    private bool _isMain = true;
    private bool _isRight;

    private void Start()
    {
        var loadData = Stats.Instance.GetLoad();
        var levelName = Stats.GetLevelName(loadData.LevelName);
        var seconds = (int)loadData.Time % 60;
        var minutes = (int)loadData.Time / 60;
        _label.text = $"{loadData.Name}    УР{loadData.LV}    {minutes}:{seconds:D2}\n{levelName}";
        transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, transform.position.z);
    }

    private void Update()
    {
        if (_isMain)
        {
            if (Input.GetButtonDown("Horizontal"))
            {
                if (Input.GetAxisRaw("Horizontal") > 0 && !_isRight)
                {
                    _selectSFX.Play();
                    _isRight = true;
                }
                else if (Input.GetAxisRaw("Horizontal") < 0 && _isRight)
                {
                    _selectSFX.Play();
                    _isRight = false;
                }
            }
            else if (Input.GetButtonDown("Submit"))
            {
                if (_isRight)
                {
                    Destroy(gameObject);
                    Fedya.Instance.enabled = true;
                }
                else
                {
                    _saveSFX.Play();
                    _isMain = false;
                    _label.color = Color.yellow;
                    _heart.gameObject.SetActive(false);
                    _buttonContainer.SetActive(false);
                    Stats.Instance.Time = GameTimer.Instance.Timer;
                    var loadData = Stats.Instance;
                    var levelName = Stats.GetLevelName(loadData.LevelName);
                    var seconds = (int)loadData.Time % 60;
                    var minutes = (int)loadData.Time / 60;
                    _label.text = $"{loadData.Name}    УР{loadData.LV}    {minutes}:{seconds:D2}\n{levelName}\n\nИгра сохранена";
                }
            }

            if (_isRight)
            {
                _heart.localPosition = new Vector2(0.83f, -1.46f);
            }
            else
            {
                _heart.localPosition = new Vector2(-4.63f, -1.46f);
            }
        }
        else
        {
            if (Input.GetButtonDown("Submit"))
            {
                Destroy(gameObject);
                Fedya.Instance.enabled = true;
                SaveSystem.Save();
                SaveSystem.MetaSave();
            }
        }
    }
}

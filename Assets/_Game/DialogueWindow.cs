using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueWindow : MonoBehaviour
{
    public static DialogueWindow Instance;
    public static AudioClip AudioClip;

    [SerializeField]
    private Transform _headContainer;

    [SerializeField]
    private TMP_Text _dialogueLabel, _monologueLabel;

    [SerializeField]
    private AudioClip _sfxDefault;
    
    [SerializeField]
    private AudioClip _sfxTomara, _sfxZvetkov;

    public float Force;
    public float Force2;
    
    private List<ShakingSymbolData> _shakingSymbols = new List<ShakingSymbolData>();
    private string[] _replicas;
    private bool _isSkip;
    public bool IsAnimated;
    public char SpeakerID = '\0';
    public bool IsDown;
    private float _duration = 0.05f;
    private GameObject _head;
    private TMP_Text _currentLabel;

    public static IEnumerator StartDialogue(string replica, bool isDown = false)
    {
        yield return StartDialogue(new [] { replica }, isDown);
    }

    public static IEnumerator StartDialogue(string[] replicas, bool isDown = false)
    {
        var dialogueWindow = Instantiate(Resources.Load<DialogueWindow>("Dialogue Window"),
            new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y + (isDown ? -3f : 3.884f)),
            Camera.main.transform.rotation);
        
        dialogueWindow.IsDown = isDown;
        dialogueWindow._replicas = replicas;
        AudioClip = dialogueWindow._sfxDefault;
        Instance = dialogueWindow;
        yield return dialogueWindow.AwaitWrite();
    }
    
    private void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            _isSkip = true;
        }
        
        transform.position = new Vector3(Camera.main.transform.position.x,
            Camera.main.transform.position.y + (IsDown ? -3f : 3.884f));

        // Тряска<shaking>Причина?</shaking>F1FFF
        
        for (int j = 0; j < _shakingSymbols.Count; j++)
        {
            var config = _shakingSymbols[j];
            var dialogText = config.Label;
            
            dialogText.ForceMeshUpdate();
            var mesh = dialogText.mesh;
            var vertices = mesh.vertices;

            Debug.Log(config.StartIndex);
            Debug.Log(config.EndIndex);
            
            for (int i = config.StartIndex; i < config.EndIndex; i++)
            {
                TMP_CharacterInfo c = dialogText.textInfo.characterInfo[i];
                int index2 = c.vertexIndex; // Индекс первой вершины символа

                // 6.49
                // 0.18
                
                // 50
                // 0.015
                
                // Генерируем уникальное смещение для каждого символа
                var time = Time.time + i;
                Vector3 offset = new Vector2(Mathf.Sin(time * 50), Mathf.Cos(time * 50)) * 0.015f;
        
                // Применяем смещение к 4 вершинам символа
                vertices[index2] += offset;
                vertices[index2 + 1] += offset;
                vertices[index2 + 2] += offset;
                vertices[index2 + 3] += offset;
            }

            mesh.vertices = vertices;
            // dialogText.canvasRenderer.SetMesh(mesh);
        }
    }
    
    private IEnumerator AwaitWrite()
    {
        var createdLabels = new List<TMP_Text>();
        
        foreach (var replica1 in _replicas)
        {
            _isSkip = false;
            var replica = replica1;
            
            if (createdLabels.Count != 0)
            {
                foreach (var label in createdLabels)
                {
                    Destroy(label.gameObject);
                }
                
                createdLabels.Clear();
            }
            
            if (_head != null)
                Destroy(_head);
            
            _dialogueLabel.gameObject.SetActive(false);
            _currentLabel = _monologueLabel;
            _currentLabel.gameObject.SetActive(true);
            _currentLabel.text = string.Empty;
            
            // yield return null;
            // _dialogueLabel.gameObject.SetActive(false);
            // _currentLabel.gameObject.SetActive(true);
            // _currentLabel = _monologueLabel;

            SpeakerID = '\0';
            
            if (replica[0] == '\\')
            {
                var faceChar = replica[1];
                var count = replica[2];
                SpeakerID = faceChar;
                
                string headPath = null;

                if (faceChar == 'T')
                {
                    if (count == '1')
                    {
                        headPath = "Tomara Head";
                        AudioClip = _sfxTomara;
                    }
                    else if (count == '2')
                    {
                        headPath = "Tomara Head Disapprove";
                        AudioClip = _sfxTomara;
                    }
                }
                else if (faceChar == 'G')
                {
                    if (count == '1')
                    {
                        headPath = "Gopnik Head";
                    }
                }
                else if (faceChar == 'Z')
                {
                    if (count == '1')
                    {
                        headPath = "Major Zvetcov Head Normal";
                        AudioClip = _sfxZvetkov;
                    }
                    else if (count == '2')
                    {
                        headPath = "Major Zvetcov Head Flirt";
                        AudioClip = _sfxZvetkov;
                    }
                    else if (count == '3')
                    {
                        headPath = "Major Zvetcov Head Sarcasm";
                        AudioClip = _sfxZvetkov;
                    }
                    else if (count == '4')
                    {
                        headPath = "Major Zvetcov Head Sad";
                        AudioClip = _sfxZvetkov;
                    }
                    else if (count == '5')
                    {
                        headPath = "Major Zvetcov Head Angry";
                        AudioClip = _sfxZvetkov;
                    }
                    else if (count == '6')
                    {
                        headPath = "Major Zvetcov Head Evil";
                        AudioClip = _sfxZvetkov;
                    }
                }

                replica = replica1.Substring(3, replica1.Length - 3);
                
                _head = Instantiate(Resources.Load<GameObject>(headPath),
                    _headContainer.position, _headContainer.rotation, _headContainer);

                _monologueLabel.gameObject.SetActive(false);
                _currentLabel = _dialogueLabel;
                _currentLabel.gameObject.SetActive(true);
                _currentLabel.text = string.Empty;
            }
            
            GetComponent<AudioSource>().clip = AudioClip;
            
            for (int i = 0; i < replica.Length; i++)
            {
                if (!_isSkip)
                    yield return new WaitForSeconds(_duration);

                switch (replica[i])
                {
                    case '\n':
                        _dialogueLabel.text += replica[i];
                        IsAnimated = true;

                        break;
                    case '%':
                        if (!_isSkip)
                            yield return new WaitForSeconds(0.5f);

                        IsAnimated = false;

                        break;
                    case '*':
                        yield return null;
                        var previousLabel = _currentLabel;
                        _currentLabel = Instantiate(previousLabel, transform);
                        createdLabels.Add(_currentLabel);

                        _currentLabel.transform.position = new Vector3(previousLabel.transform.position.x,
                            previousLabel.transform.position.y - previousLabel.renderedHeight, previousLabel.transform.position.z); // -1
                        
                        _currentLabel.text = string.Empty;
                        IsAnimated = false;

                        break;
                    case ',':
                        _currentLabel.text += ',';
                        IsAnimated = false;

                        if (!_isSkip)
                            yield return new WaitForSeconds(0.5f);

                        break;
                    case '^': // Обработка команды паузы
                    {
                        IsAnimated = false;
                        string digital = "";
                        int currentIndex = i + 1;
                        bool hasDecimalPoint = false;
    
                        // Проверка на выход за границы
                        if (currentIndex >= replica.Length)
                            break;
    
                        // Собираем число
                        while (currentIndex < replica.Length)
                        {
                            char c = replica[currentIndex];
        
                            if (char.IsDigit(c))
                            {
                                digital += c;
                                currentIndex++;
                            }
                            else if (c == '.' && !hasDecimalPoint)
                            {
                                // Проверяем, что после точки есть цифра (это десятичный разделитель)
                                if (currentIndex + 1 < replica.Length && char.IsDigit(replica[currentIndex + 1]))
                                {
                                    digital += c;
                                    hasDecimalPoint = true;
                                    currentIndex++;
                                }
                                else
                                {
                                    // Это точка как знак препинания - останавливаемся
                                    break;
                                }
                            }
                            else
                            {
                                // Другой символ - останавливаем сбор числа
                                break;
                            }
                        }
    
                        if (!string.IsNullOrEmpty(digital))
                        {
                            if (float.TryParse(digital, 
                                    System.Globalization.NumberStyles.Float, 
                                    System.Globalization.CultureInfo.InvariantCulture, 
                                    out float value))
                            {
                                if (!_isSkip)
                                    yield return new WaitForSeconds(value);

                                i = currentIndex - 1; // Обновляем индекс
                            }
                            else
                            {
                                Debug.LogWarning($"Не удалось распарсить '{digital}'");
                            }
                        }
                        break;
                    }
                    case '<':
                        if (replica[i + 1] == 'c' || replica[i + 2] == 'c')
                        {
                            while (replica[i] != '>')
                            {
                                _currentLabel.text += replica[i];
                                i++;
                            }
                        
                            _currentLabel.text += replica[i];
                            break;
                        }
                        
                        if (replica[i + 1] != '/')
                        {
                            for (; i < replica.Length; i++)
                            {
                                if (replica[i] == '>')
                                {
                                    break;
                                }
                            }
                            
                            var symbol = new ShakingSymbolData(_currentLabel.text.Length, -1, _currentLabel, 1);
                            _shakingSymbols.Add(symbol);
                        }
                        else
                        {
                            for (; i < replica.Length; i++)
                            {
                                if (replica[i] == '>')
                                {
                                    break;
                                }
                            }
                            
                            var symbol = _shakingSymbols[^1];
                            symbol.EndIndex = _currentLabel.text.Length;
                            _shakingSymbols[^1] = symbol;
                            
                            Debug.Log($"Start Index: {_shakingSymbols[^1].StartIndex}");
                            Debug.Log($"End Index: {_shakingSymbols[^1].EndIndex}");
                            Debug.Log($"Message: {_currentLabel.text.Substring(_shakingSymbols[^1].StartIndex, _shakingSymbols[^1].EndIndex - _shakingSymbols[^1].StartIndex)}");
                        }
                        
                        // shaking
                        break;
                    default:
                        _currentLabel.text += replica[i];
                        IsAnimated = true;

                        if (!_isSkip && !GetComponent<AudioSource>().isPlaying)
                            GetComponent<AudioSource>().Play();
                        //Vector3(13.1300001,1.83000004,0)
                        break;
                }
            }

            IsAnimated = false;
            yield return new WaitUntil(() => Input.GetButtonDown("Submit"));
        }

        Destroy(gameObject);
    }

    public static bool IsSpeak(char speakerID)
    {
        if (!Instance)
            return false;
        
        if (speakerID == Instance.SpeakerID && Instance.IsAnimated)
            return true;
            
        return false;
    }
}

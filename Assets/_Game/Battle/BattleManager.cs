 using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class BattleManager : MonoBehaviour
{
    public static BattleManager Instance;
    public static string MainText;
    public static string[] AdditionalExitText;
    
    private const float SPEED_SIZE_FRAME = 7;
    
    public readonly Vector2 AttackFrameSize_1 = new Vector2(2.1f, 1.75f);
    public readonly Vector2 PlayerTurnFrameSize_1 = new Vector2(7.2f, 1.75f);
    
    public int CountTurn = 1;
    public int SelectMainButtonIndex;
    public int SelectSoryanIndex;
    public bool IsSelectMain = true;
    public bool IsProhibitionSelectMain;
    public bool IsSelectVertushkaSelected;
    public bool IsSelectBazarSelected;
    public bool IsSelectBazar;
    public bool IsSelectKarmani;
    public bool IsEndAnimationFrame = true;
    public Vector2 TargetSizeFrame;
    public List<ActionLabel> ActionLabels =  new();
    public List<ActionLabel> BazarSelectedLabels =  new();
    public SpriteRenderer GetFrame => _frame;
    public bool IsEnemyDead;
    public bool IsEnemyTurn;
    public bool IsSkipPlayerTurn;
    public int SelectKarmaniIndex;
    public bool IsEndBattle;
    public bool IsStartBlackout;
    public bool IsSelectSoryan;
    public int SelectBazarIndex;
    public int SelectBazarSelectedIndex;
    public int SelectVertushkaIndex;
    public bool IsKarmaniPage_2;
    public bool IsKarmaniSpawnedPage_2;
    public bool IsRun = true;
    public List<Enemy> Enemies = new List<Enemy>();
    public List<VertushkaLabel> VertushkaEnemyLabels = new List<VertushkaLabel>();
    public List<ActionLabel> KarmaniLabels = new List<ActionLabel>();
    public BattleMainText BattleMainText => _mainText;
    
    [SerializeField]
    private SpriteRenderer _frame;

    [SerializeField]
    private GameObject _vertuchkaSelectedScreen;

    [SerializeField]
    private GameObject _vertuchkaScreen;

    [SerializeField]
    private GameObject _bazarSelectedScreen;
    
    [SerializeField]
    private GameObject _bazarScreen;
    
    [SerializeField]
    private GameObject _karmaniScreen;
    
    [SerializeField]
    private GameObject _soryanScreen;
    
    [SerializeField]
    private GameObject _buttons;

    [SerializeField]
    private BattleMainText _mainText;
    
    [SerializeField]
    private GameObject _runText;
    
    [SerializeField]
    private ActionLabel _soryanText;

    [SerializeField]
    private AudioSource _selectSFX;

    [SerializeField]
    private AudioSource _squeakSFX;
    
    [SerializeField]
    private AudioSource _damageSFX;
    
    [SerializeField]
    private AudioSource _runSFX;
    
    [SerializeField]
    private AudioSource _spareSFX;

    [SerializeField]
    private AudioSource _lazSFX;
    
    [SerializeField]
    private AudioSource _levelUPSFX;
    
    [SerializeField]
    private AudioSource _healSFX;
    
    [SerializeField]
    private SpriteRenderer _blackout;

    [SerializeField]
    private GameObject _runHeart;

    [SerializeField]
    private Animator _targetAnimator;

    [SerializeField]
    private Animator _stickAnimator;
    
    [SerializeField]
    private TMP_Text _lvLabel;
    
    [SerializeField]
    private TMP_Text _nameLabel;
    
    [SerializeField]
    private ParticleSystem _mercyVFX;
    
    private bool _isInit;
    private bool _isFirstFramePlayerTurn;

    private void Awake()
    {
        Instance = GetComponent<BattleManager>();
        TargetSizeFrame = PlayerTurnFrameSize_1;
        AdditionalExitText = null;
        _lvLabel.text = $"УР {Stats.Instance.LV}";
        _nameLabel.text = $"{Stats.Instance.Name}";
        
        if (IsStartBlackout)
        {
            _blackout.color = Color.black;
            _blackout.gameObject.SetActive(true);
        }

        MainText = string.Empty;
    }

    private void Update()
    {
        if (!_isInit && !string.IsNullOrEmpty(MainText))
        {
            _mainText.SetText(MainText, false);
            _isInit = true;
        }
        
        if (IsStartBlackout)
        {
            if (_blackout.color.a > 0)
            {
                var alpha = _blackout.color.a;
                alpha -= Time.deltaTime;
                _blackout.color = new Color(0, 0, 0, alpha);
            }
            else
            {
                _blackout.gameObject.SetActive(false);
                IsStartBlackout = false;
            }
        }

        if (IsEndBattle)
        {
            return;
        }

        // if ()
        // {
        //     // TargetSizeFrame = _playerTurnFrameSize_1;
        //     
        //
        //     // if (!IsEnemyTurn && TargetSizeFrame != PlayerTurnFrameSize_1)
        //     //     TargetSizeFrame = PlayerTurnFrameSize_1;
        // }
        
        if (IsSkipPlayerTurn)
        {
            IsSkipPlayerTurn = false;
            // Heart.Instance.gameObject.SetActive(true);
            _buttons.SetActive(false);
            _frame.size = TargetSizeFrame;
            IsEndAnimationFrame = true;
            IsEnemyTurn = true;
            _mainText.ClearText();
        }
        
        if (_frame.size != TargetSizeFrame)
        {
            IsEndAnimationFrame = false;
            IsSelectMain = true;
            
            _buttons.SetActive(false);
            
            if (TargetSizeFrame == PlayerTurnFrameSize_1)
                Heart.Instance.gameObject.SetActive(false);
            
            _frame.size = Vector2.MoveTowards(_frame.size, TargetSizeFrame, Time.deltaTime * SPEED_SIZE_FRAME);
            // Heart.Instance.gameObject.SetActive(false);
            
            if (_frame.size == TargetSizeFrame)
            {
                IsEndAnimationFrame = true;

                if (TargetSizeFrame == PlayerTurnFrameSize_1)
                {
                    _mainText.SetText(MainText, false);
                    // Heart.Instance.gameObject.SetActive(true);
                }
                else
                {
                    Heart.Instance.transform.position = new Vector2(0f, -2f);
                }
            }
            
            return;
        }

        if (IsEnemyTurn)
        {
            Heart.Instance.gameObject.SetActive(true);
            _isFirstFramePlayerTurn = true;
            return;
        }

        TargetSizeFrame = PlayerTurnFrameSize_1;

        if (_frame.size != TargetSizeFrame)
        {
            return;
        }
        
        if (_isFirstFramePlayerTurn)
        {
            CountTurn++;
            _buttons.SetActive(false);
            _isFirstFramePlayerTurn = false;
            TargetSizeFrame = PlayerTurnFrameSize_1;
            IsEnemyDead = false;
            return;
        }
            
        _buttons.SetActive(true);

        if (IsSelectMain)
        {
            Heart.Instance.gameObject.SetActive(true);
            
            if (SelectMainButtonIndex == 0)
            {
                Heart.Instance.transform.position = new Vector2(-6.83f, -5.321f);
            }
            else if (SelectMainButtonIndex == 1)
            {
                Heart.Instance.transform.position = new Vector2(-2.982f, -5.321f);
            }
            else if (SelectMainButtonIndex == 2)
            {
                Heart.Instance.transform.position = new Vector2(0.963f, -5.321f);
            }
            else if (SelectMainButtonIndex == 3)
            {
                Heart.Instance.transform.position = new Vector2(4.876f, -5.321f);
            }

            if (IsStartBlackout)
            {
                return;
            }

            if (!IsProhibitionSelectMain)
            {
                var isEmptyItems = true;

                foreach (var item in Stats.Instance.Items)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        isEmptyItems = false;
                    }
                }

                if (isEmptyItems && SelectMainButtonIndex == 2)
                    SelectMainButtonIndex = 0;
                
                if (Input.GetButtonDown("Horizontal"))
                {
                    if (Input.GetAxisRaw("Horizontal") > 0)
                    {
                        if (SelectMainButtonIndex == 0)
                        {
                            SelectMainButtonIndex = 1;
                            _squeakSFX.Play();
                        }
                        else if (SelectMainButtonIndex == 1)
                        {
                            if (!isEmptyItems)
                                SelectMainButtonIndex = 2;
                            else
                                SelectMainButtonIndex = 3;
                            
                            _squeakSFX.Play();
                        }
                        else if (SelectMainButtonIndex == 2)
                        {
                            SelectMainButtonIndex = 3;
                            _squeakSFX.Play();
                        }
                    }
                    else if (Input.GetAxisRaw("Horizontal") < 0)
                    {
                        if (SelectMainButtonIndex == 3)
                        {
                            if (!isEmptyItems)
                                SelectMainButtonIndex = 2;
                            else
                                SelectMainButtonIndex = 1;
                            
                            _squeakSFX.Play();
                        }
                        else if (SelectMainButtonIndex == 2)
                        {
                            SelectMainButtonIndex = 1;
                            _squeakSFX.Play();
                        }
                        else if (SelectMainButtonIndex == 1)
                        {
                            SelectMainButtonIndex = 0;
                            _squeakSFX.Play();
                        }
                    }
                } 
            }
            
            if (Input.GetButtonDown("Submit"))
            {
                if (SelectMainButtonIndex == 0)
                {
                    IsSelectVertushkaSelected = true;
                    
                    for (int i = 0; i < Enemies.Count; i++)
                    {
                        if (!Enemies[i].IsActive)
                        {
                            VertushkaEnemyLabels.Add(null);
                            continue;
                        }
                        
                        VertushkaEnemyLabels.Add(Instantiate(Resources.Load<VertushkaLabel>("Vertushka Label"), _vertuchkaSelectedScreen.transform));
                    
                        VertushkaEnemyLabels[i].transform.localPosition = i switch
                        {
                            0 => new Vector2(-2.1f, 0.97f),
                            1 => new Vector2(-2.1f, 0.05f),
                            2 => new Vector2(-2.1f, -0.88f)
                        };
                    
                        VertushkaEnemyLabels[i].Label.text = Enemies[i].Name;
                        VertushkaEnemyLabels[i].SetHealth(Enemies[i].Health, Enemies[i].MaxHealth, Enemies[i].Relationship >= 0);
                    }
                }
                else if (SelectMainButtonIndex == 1)
                {
                    if (Enemies.Count == 1)
                    {
                        IsSelectBazar = true;
                
                        for (int i = 0; i < Enemies[0].Actions.Count; i++)
                        {
                            ActionLabels.Add(Instantiate(Resources.Load<ActionLabel>("Action Label"), _bazarScreen.transform));
                    
                            ActionLabels[i].transform.localPosition = i switch
                            {
                                0 => new Vector2(-2f, 0.97f),
                                1 => new Vector2(4.3f, 0.97f),
                                2 => new Vector2(-2f, 0.05f),
                                3 => new Vector2(4.3f, 0.05f),
                                4 => new Vector2(-2f, -0.88f),
                                5 => new Vector2(4.3f, -0.88f),
                            };
                    
                            ActionLabels[i].Label.text = Enemies[0].Actions[i];
                        }
                    }
                    else
                    {
                        IsSelectBazarSelected = true;

                        for (int i = 0; i < Enemies.Count; i++)
                        {
                            if (!Enemies[i].IsActive)
                            {
                                BazarSelectedLabels.Add(null);
                                continue;
                            }
                            
                            BazarSelectedLabels.Add(Instantiate(Resources.Load<ActionLabel>("Action Label"), _bazarSelectedScreen.transform));
                            
                            BazarSelectedLabels[i].transform.localPosition = i switch
                            {
                                0 => new Vector2(-2f, 0.97f),
                                1 => new Vector2(-2f, 0.05f),
                                2 => new Vector2(-2f, -0.88f),
                            };
                    
                            BazarSelectedLabels[i].Label.text = Enemies[i].Name;
                            BazarSelectedLabels[i].Label.color = Enemies[i].Relationship >= 0 ? Color.yellow : Color.white;
                            BazarSelectedLabels[i].Star.GetComponent<TMP_Text>().color = Enemies[i].Relationship >= 0 ? Color.yellow : Color.white;
                        }
                    }
                }
                else if (SelectMainButtonIndex == 2)
                {
                    IsSelectKarmani = true;
                    var index = 0;
                    var itemsCount = 0;

                    foreach (var item in Stats.Instance.Items)
                    {
                        if (string.IsNullOrEmpty(item))
                            continue;

                        itemsCount++;
                    }
                    
                    var items = new string[itemsCount];
                    var j = 0;
                    
                    foreach (var item in Stats.Instance.Items)
                    {
                        if (string.IsNullOrEmpty(item))
                            continue;
                        
                        items[j] = item;
                        j++;
                    }

                    for (int i = 0; i < items.Length; i++)
                    {
                        if (string.IsNullOrEmpty(items[i]))
                            continue;
                        
                        if (index >= 6)
                            continue;
                        
                        KarmaniLabels.Add(Instantiate(Resources.Load<ActionLabel>("Action Label"), _karmaniScreen.transform));
                    
                        KarmaniLabels[i].transform.localPosition = index switch
                        {
                            0 => new Vector2(-2f, 0.97f),
                            1 => new Vector2(4.3f, 0.97f),
                            2 => new Vector2(-2f, 0.05f),
                            3 => new Vector2(4.3f, 0.05f),
                            4 => new Vector2(-2f, -0.88f),
                            5 => new Vector2(4.3f, -0.88f),
                        };
                    
                        KarmaniLabels[i].Label.text = items[i];
                        index++;
                    }
                }
                else if (SelectMainButtonIndex == 3)
                {
                    if (IsRun)
                    {
                        _runText.SetActive(true);
                    }
                    else
                    {
                        _runText.SetActive(false);
                    }

                    var isSoryan = false;

                    foreach (var enemy in Enemies)
                    {
                        if (enemy.Relationship >= 0 && enemy.IsActive)
                        {
                            isSoryan = true;
                        }
                    }
                    
                    _soryanText.Label.color = isSoryan ? Color.yellow : Color.white;
                    _soryanText.Star.GetComponent<TMP_Text>().color = isSoryan ? Color.yellow : Color.white;
                    IsSelectSoryan = true;
                }
                
                _mainText.gameObject.SetActive(false);
                IsSelectMain = false;
                
                _selectSFX.Play();
            }
        }
        else if (IsSelectVertushkaSelected)
        {
            _vertuchkaSelectedScreen.gameObject.SetActive(true);
            
            if (Input.GetButtonDown("Submit"))
            {
                if (!Enemies[SelectVertushkaIndex].IsActive)
                    return;
                
                IsSelectVertushkaSelected = false;
                StartCoroutine(AwaitVertushka());
            }
            else if (Input.GetButtonDown("Cancel"))
            {
                _mainText.SetText(MainText, false);
                IsSelectMain = true;
                IsSelectVertushkaSelected = false;
                
                for (int i = 0; i < VertushkaEnemyLabels.Count; i++)
                {
                    if (!VertushkaEnemyLabels[i])
                        continue;
                    
                    Destroy(VertushkaEnemyLabels[i].gameObject);
                }
                
                VertushkaEnemyLabels = new List<VertushkaLabel>();
                _vertuchkaSelectedScreen.gameObject.SetActive(false);
            }
            else if (Input.GetButtonDown("Vertical"))
            {
                if (Input.GetAxisRaw("Vertical") > 0)
                {
                    if (SelectVertushkaIndex == 2)
                    {
                        SelectVertushkaIndex = 1;
                    }
                    else if (SelectVertushkaIndex == 1)
                    {
                        SelectVertushkaIndex = 0;
                    }
                }
                else if (Input.GetAxisRaw("Vertical") < 0)
                {
                    if (SelectVertushkaIndex == 0 && Enemies.Count > 1)
                    {
                        SelectVertushkaIndex = 1;
                    }
                    else if (SelectVertushkaIndex == 1 && Enemies.Count > 2)
                    {
                        SelectVertushkaIndex = 2;
                    }
                }
            }
            
            Heart.Instance.transform.localPosition = SelectVertushkaIndex switch
            {
                0 => new Vector2(-6.54f, -1.09f),
                1 => new Vector2(-6.54f, -2f),
                2 => new Vector2(-6.54f, -2.94f)
            };
        }
        else if (IsSelectBazarSelected)
        {
            _bazarSelectedScreen.gameObject.SetActive(true);
            
            if (Input.GetButtonDown("Submit"))
            {
                if (!BazarSelectedLabels[SelectBazarSelectedIndex])
                    return;
                
                for (int i = 0; i < BazarSelectedLabels.Count; i++)
                {
                    if (!BazarSelectedLabels[i])
                        continue;
                    
                    Destroy(BazarSelectedLabels[i].gameObject);
                }
                
                BazarSelectedLabels = new List<ActionLabel>();
                IsSelectBazarSelected = false;
                _selectSFX.Play();
                _bazarSelectedScreen.SetActive(false);
                
                IsSelectBazar = true;
                
                for (int i = 0; i < Enemies[SelectBazarSelectedIndex].Actions.Count; i++)
                {
                    ActionLabels.Add(Instantiate(Resources.Load<ActionLabel>("Action Label"), _bazarScreen.transform));
                    
                    ActionLabels[i].transform.localPosition = i switch
                    {
                        0 => new Vector2(-2f, 0.97f),
                        1 => new Vector2(4.3f, 0.97f),
                        2 => new Vector2(-2f, 0.05f),
                        3 => new Vector2(4.3f, 0.05f),
                        4 => new Vector2(-2f, -0.88f),
                        5 => new Vector2(4.3f, -0.88f),
                    };
                    
                    ActionLabels[i].Label.text = Enemies[SelectBazarSelectedIndex].Actions[i];
                }
            }
            else if (Input.GetButtonDown("Cancel"))
            {
                _mainText.SetText(MainText, false);
                IsSelectMain = true;
                IsSelectBazarSelected = false;
                
                for (int i = 0; i < BazarSelectedLabels.Count; i++)
                {
                    if (!BazarSelectedLabels[i])
                        continue;
                    
                    Destroy(BazarSelectedLabels[i].gameObject);
                }
                
                BazarSelectedLabels = new List<ActionLabel>();
                _bazarSelectedScreen.gameObject.SetActive(false);
            }
            else if (Input.GetButtonDown("Vertical"))
            {
                if (Input.GetAxisRaw("Vertical") > 0)
                {
                    if (SelectBazarSelectedIndex == 2)
                    {
                        SelectBazarSelectedIndex = 1;
                    }
                    else if (SelectBazarSelectedIndex == 1)
                    {
                        SelectBazarSelectedIndex = 0;
                    }
                }
                else if (Input.GetAxisRaw("Vertical") < 0)
                {
                    if (SelectBazarSelectedIndex == 0 && Enemies.Count > 1)
                    {
                        SelectBazarSelectedIndex = 1;
                    }
                    else if (SelectBazarSelectedIndex == 1 && Enemies.Count > 2)
                    {
                        SelectBazarSelectedIndex = 2;
                    }
                }
            }
            
            Heart.Instance.transform.localPosition = SelectBazarSelectedIndex switch
            {
                0 => new Vector2(-6.54f, -1.09f),
                1 => new Vector2(-6.54f, -2f),
                2 => new Vector2(-6.54f, -2.94f)
            };
        }
        else if (IsSelectBazar)
        {
            _bazarScreen.gameObject.SetActive(true);
            var actionsCount = Enemies[SelectBazarSelectedIndex].Actions.Count;
            
            if (Input.GetButtonDown("Submit"))
            {
                for (int i = 0; i < ActionLabels.Count; i++)
                {
                    Destroy(ActionLabels[i].gameObject);
                }
                
                ActionLabels = new List<ActionLabel>();
                IsSelectBazar = false;
                _selectSFX.Play();
                _bazarScreen.gameObject.SetActive(false);
                _mainText.SetText(Enemies[SelectBazarSelectedIndex].ActionAnswers[SelectBazarIndex], true);
                
                Heart.Instance.gameObject.SetActive(false);
            }
            else if (Input.GetButtonDown("Cancel"))
            {
                IsSelectBazar = false;
                
                for (int i = 0; i < ActionLabels.Count; i++)
                {
                    Destroy(ActionLabels[i].gameObject);
                }
                
                ActionLabels = new List<ActionLabel>();
                _bazarScreen.gameObject.SetActive(false);

                if (Enemies.Count > 1)
                {
                    IsSelectBazarSelected = true;

                    for (int i = 0; i < Enemies.Count; i++)
                    {
                        if (!Enemies[i].IsActive)
                        {
                            BazarSelectedLabels.Add(null);
                            continue;
                        }
                        
                        BazarSelectedLabels.Add(Instantiate(Resources.Load<ActionLabel>("Action Label"), _bazarSelectedScreen.transform));
                            
                        BazarSelectedLabels[i].transform.localPosition = i switch
                        {
                            0 => new Vector2(-2f, 0.97f),
                            1 => new Vector2(-2f, 0.05f),
                            2 => new Vector2(-2f, -0.88f),
                        };
                    
                        BazarSelectedLabels[i].Label.text = Enemies[i].Name;
                        BazarSelectedLabels[i].Label.color = Enemies[i].Relationship >= 0 ? Color.yellow : Color.white;
                        BazarSelectedLabels[i].Star.GetComponent<TMP_Text>().color = Enemies[i].Relationship >= 0 ? Color.yellow : Color.white;
                    }
                }
                else
                {
                    _mainText.SetText(MainText, false);
                    IsSelectMain = true;
                }
            }
            else if (Input.GetButtonDown("Horizontal"))
            {
                if (Input.GetAxisRaw("Horizontal") > 0)
                {
                    if (SelectBazarIndex == 0 && actionsCount > 1)
                    {
                        SelectBazarIndex = 1;
                    }
                    else if (SelectBazarIndex == 2 && actionsCount > 3)
                    {
                        SelectBazarIndex = 3;
                    }
                    else if (SelectBazarIndex == 4 && actionsCount > 5)
                    {
                        SelectBazarIndex = 5;
                    }
                }
                else if (Input.GetAxisRaw("Horizontal") < 0)
                {
                    if (SelectBazarIndex == 1)
                    {
                        SelectBazarIndex = 0;
                    }
                    else if (SelectBazarIndex == 3)
                    {
                        SelectBazarIndex = 2;
                    }
                    else if (SelectBazarIndex == 5)
                    {
                        SelectBazarIndex = 4;
                    }
                }
            } 
            else if (Input.GetButtonDown("Vertical"))
            {
                if (Input.GetAxisRaw("Vertical") > 0)
                {
                    if (SelectBazarIndex == 2)
                    {
                        SelectBazarIndex = 0;
                    }
                    else if (SelectBazarIndex == 3)
                    {
                        SelectBazarIndex = 1;
                    }
                    else if (SelectBazarIndex == 4)
                    {
                        SelectBazarIndex = 2;
                    }
                    else if (SelectBazarIndex == 5)
                    {
                        SelectBazarIndex = 3;
                    }
                }
                else if (Input.GetAxisRaw("Vertical") < 0)
                {
                    if (SelectBazarIndex == 0 && actionsCount > 2)
                    {
                        SelectBazarIndex = 2;
                    }
                    else if (SelectBazarIndex == 1 && actionsCount > 3)
                    {
                        SelectBazarIndex = 3;
                    }
                    else if (SelectBazarIndex == 2 && actionsCount > 4)
                    {
                        SelectBazarIndex = 4;
                    }
                    else if (SelectBazarIndex == 3 && actionsCount > 5)
                    {
                        SelectBazarIndex = 5;
                    }
                }
            }
            
            Heart.Instance.transform.localPosition = SelectBazarIndex switch
            {
                0 => new Vector2(-6.54f, -1.09f),
                1 => new Vector2(-0.26f, -1.09f),
                2 => new Vector2(-6.54f, -2f),
                3 => new Vector2(-0.26f, -2f),
                4 => new Vector2(-6.54f, -2.94f),
                5 => new Vector2(-0.26f, -2.94f),
                _ => new Vector2(-6.54f, -1.09f)
            };
        }
        else if (IsSelectKarmani)
        {
            _karmaniScreen.gameObject.SetActive(true);
            
            var allItems = new List<string>();
                
            for (int i = 0; i < Stats.Instance.Items.Length; i++)
            {
                if (string.IsNullOrEmpty(Stats.Instance.Items[i]))
                    continue;
                        
                allItems.Add(Stats.Instance.Items[i]);
            }
            
            if (Input.GetButtonDown("Submit"))
            {
                IsSelectKarmani = false;
                
                for (int i = 0; i < KarmaniLabels.Count; i++)
                {
                    Destroy(KarmaniLabels[i].gameObject);
                }
                
                KarmaniLabels = new List<ActionLabel>();
                _karmaniScreen.gameObject.SetActive(false);
        
                Heart.Instance.gameObject.SetActive(false);
                var item = Stats.Instance.Items[SelectKarmaniIndex];
                var message = "Разработчик это не предусмотрел...";

                if (Stats.IsWeapon(item))
                {
                    message = $"Вы экипировали \"{item}\"";
                    var currentWeapon = Stats.Instance.Weapon;
                    Stats.Instance.Weapon = item;
                    Stats.Instance.Items[SelectKarmaniIndex] = currentWeapon;
                }
                else if (Stats.IsArmor(item))
                {
                    message = $"Вы экипировали \"{item}\"";
                    var currentArmor = Stats.Instance.Armor;
                    Stats.Instance.Armor = item;
                    Stats.Instance.Items[SelectKarmaniIndex] = currentArmor;
                }
                else if (Stats.IsComida(item))
                {
                    var hp = Stats.GetItemHP(item);
                    Stats.Instance.HP += hp;
                        
                    var beberMessage = $"Вы выпили \"{item}\"";
                    var eatMessage = $"Вы съели \"{item}\"";
                        
                    message = item switch
                    {
                        Constants.ANTIPOHMELIN => beberMessage,
                        Constants.JAGUAR => beberMessage,
                        Constants.MASHA_JAM => eatMessage,
                        Constants.MASHA_PIES => eatMessage,
                        Constants.BELASH => eatMessage,
                        Constants.NASTOYKA_GASTERA => beberMessage,
                        _ => "Разработчик этого не добавил"
                    };
                        
                    if (Stats.Instance.HP >= Stats.Instance.MaxHP)
                    {
                        Stats.Instance.HP = Stats.Instance.MaxHP;
                        message += "*Вы восстановили всё ОЗ";
                    }
                    else
                    {
                        message += $"*Вы восстановили {hp} ОЗ";
                    }
                        
                    _healSFX.Play();
                    Stats.Instance.Items[SelectKarmaniIndex] = string.Empty;
                }
                
                _mainText.SetText(message, true);
            }
            else if (Input.GetButtonDown("Cancel"))
            {
                _mainText.SetText(MainText, false);
                IsSelectMain = true;
                IsSelectKarmani = false;
                
                for (int i = 0; i < KarmaniLabels.Count; i++)
                {
                    Destroy(KarmaniLabels[i].gameObject);
                }
                
                KarmaniLabels = new List<ActionLabel>();
                _karmaniScreen.gameObject.SetActive(false);
            }
            else if (Input.GetButtonDown("Horizontal"))
            {
                if (Input.GetAxisRaw("Horizontal") > 0)
                {
                    if (SelectKarmaniIndex == 0 && allItems.Count > 1)
                    {
                        SelectKarmaniIndex = 1;
                    }
                    else if (SelectKarmaniIndex == 1 && allItems.Count > 6)
                    {
                        SelectKarmaniIndex = 6;
                        IsKarmaniPage_2 = true;
                    }
                    else if (SelectKarmaniIndex == 2 && allItems.Count > 3)
                    {
                        SelectKarmaniIndex = 3;
                    }
                    else if (SelectKarmaniIndex == 3 && allItems.Count > 6)
                    {
                        SelectKarmaniIndex = 6;
                        IsKarmaniPage_2 = true;
                    }
                    else if (SelectKarmaniIndex == 4 && allItems.Count > 5)
                    {
                        SelectKarmaniIndex = 5;
                    }
                    else if (SelectKarmaniIndex == 5 && allItems.Count > 6)
                    {
                        SelectKarmaniIndex = 6;
                        IsKarmaniPage_2 = true;
                    }
                    else if (SelectKarmaniIndex == 6 && allItems.Count > 7)
                    {
                        SelectKarmaniIndex = 7;
                    }
                }
                else if (Input.GetAxisRaw("Horizontal") < 0)
                {
                    if (SelectKarmaniIndex == 1)
                    {
                        SelectKarmaniIndex = 0;
                    }
                    else if (SelectKarmaniIndex == 3)
                    {
                        SelectKarmaniIndex = 2;
                    }
                    else if (SelectKarmaniIndex == 5)
                    {
                        SelectKarmaniIndex = 4;
                    }
                    else if (SelectKarmaniIndex == 6)
                    {
                        SelectKarmaniIndex = 1;
                        IsKarmaniPage_2 = false;
                    }
                    else if (SelectKarmaniIndex == 7)
                    {
                        SelectKarmaniIndex = 6;
                    }
                }
            } 
            else if (Input.GetButtonDown("Vertical"))
            {
                if (Input.GetAxisRaw("Vertical") > 0)
                {
                    if (SelectKarmaniIndex == 2)
                    {
                        SelectKarmaniIndex = 0;
                    }
                    else if (SelectKarmaniIndex == 3)
                    {
                        SelectKarmaniIndex = 1;
                    }
                    else if (SelectKarmaniIndex == 4)
                    {
                        SelectKarmaniIndex = 2;
                    }
                    else if (SelectKarmaniIndex == 5)
                    {
                        SelectKarmaniIndex = 3;
                    }
                }
                else if (Input.GetAxisRaw("Vertical") < 0)
                {
                    if (SelectKarmaniIndex == 0 && allItems.Count > 2)
                    {
                        SelectKarmaniIndex = 2;
                    }
                    else if (SelectKarmaniIndex == 1 && allItems.Count > 3)
                    {
                        SelectKarmaniIndex = 3;
                    }
                    else if (SelectKarmaniIndex == 2 && allItems.Count > 4)
                    {
                        SelectKarmaniIndex = 4;
                    }
                    else if (SelectKarmaniIndex == 3 && allItems.Count > 5)
                    {
                        SelectKarmaniIndex = 5;
                    }
                }
            }

            if (IsKarmaniPage_2 && !IsKarmaniSpawnedPage_2)
            {
                for (int i = 0; i < KarmaniLabels.Count; i++)
                {
                    Destroy(KarmaniLabels[i].gameObject);
                }
                
                KarmaniLabels =  new List<ActionLabel>();
                
                for (int i = 6; i < allItems.Count; i++)
                {
                    KarmaniLabels.Add(Instantiate(Resources.Load<ActionLabel>("Action Label"), _karmaniScreen.transform));
                    
                    KarmaniLabels[i - 6].transform.localPosition = i switch
                    {
                        6 => new Vector2(-2f, 0.97f),
                        7 => new Vector2(4.3f, 0.97f)
                    };
                    
                    KarmaniLabels[i - 6].Label.text = Stats.Instance.Items[i];
                }
                
                IsKarmaniSpawnedPage_2 = true;
            }
            else if (!IsKarmaniPage_2 && IsKarmaniSpawnedPage_2)
            {
                for (int i = 0; i < KarmaniLabels.Count; i++)
                {
                    Destroy(KarmaniLabels[i].gameObject);
                }
                
                KarmaniLabels =  new List<ActionLabel>();
                
                for (int i = 0; i < 6; i++)
                {
                    KarmaniLabels.Add(Instantiate(Resources.Load<ActionLabel>("Action Label"), _karmaniScreen.transform));
                    
                    KarmaniLabels[i].transform.localPosition = i switch
                    {
                        0 => new Vector2(-2f, 0.97f),
                        1 => new Vector2(4.3f, 0.97f),
                        2 => new Vector2(-2f, 0.05f),
                        3 => new Vector2(4.3f, 0.05f),
                        4 => new Vector2(-2f, -0.88f),
                        5 => new Vector2(4.3f, -0.88f),
                    };
                    
                    KarmaniLabels[i].Label.text = Stats.Instance.Items[i];
                }
                
                IsKarmaniSpawnedPage_2 = false;
            }

            Heart.Instance.transform.localPosition = SelectKarmaniIndex switch
            {
                0 => new Vector2(-6.54f, -1.09f),
                1 => new Vector2(-0.26f, -1.09f),
                2 => new Vector2(-6.54f, -2f),
                3 => new Vector2(-0.26f, -2f),
                4 => new Vector2(-6.54f, -2.94f),
                5 => new Vector2(-0.26f, -2.94f),
                6 => new Vector2(-6.54f, -1.09f),
                7 => new Vector2(-0.26f, -1.09f),
                _ => new Vector2(-6.54f, -1.09f)
            };
        }
        else if (IsSelectSoryan)
        {
            _soryanScreen.gameObject.SetActive(true);
            
            if (Input.GetButtonDown("Submit"))
            {
                IsSelectSoryan = false;
                
                if (SelectSoryanIndex == 0)
                {
                    _soryanScreen.SetActive(false);
                    Heart.Instance.gameObject.SetActive(false);
                    var canSoryanAround = true;

                    foreach (var enemy in Enemies)
                    {
                        if (enemy.Health > 0)
                        {
                            if (enemy.Relationship < 0)
                            {
                                canSoryanAround = false;
                            }
                            else if (!enemy.IsMercy)
                            {
                                enemy.IsMercy = true;
                                Instantiate(_mercyVFX, enemy.transform.position, new Quaternion());
                            }
                        }
                    }
                    
                    if (!canSoryanAround)
                    {
                        StartCoroutine(AwaitEnemyTurn());
                    }
                    else
                    {
                        StartCoroutine(AwaitExitMessage());
                    }
                }
                else if (SelectSoryanIndex == 1)
                {
                    StartCoroutine(AwaitRunHeart());
                }
            }
            else if (Input.GetButtonDown("Cancel"))
            {
                _mainText.SetText(MainText, false);
                IsSelectMain = true;
                IsSelectSoryan = false;
                _soryanScreen.gameObject.SetActive(false);
            }
            else if (Input.GetButtonDown("Vertical"))
            {
                if (Input.GetAxisRaw("Vertical") > 0)
                {
                    if (SelectSoryanIndex == 1)
                    {
                        SelectSoryanIndex = 0;
                    }
                }
                else if (Input.GetAxisRaw("Vertical") < 0)
                {
                    if (SelectSoryanIndex == 0 && IsRun)
                    {
                        SelectSoryanIndex = 1;
                    }
                }
            }
            
            Heart.Instance.transform.localPosition = SelectSoryanIndex switch
            {
                0 => new Vector2(-6.54f, -1.09f),
                1 => new Vector2(-6.54f, -2.08f),
            };
        }
    }

    private IEnumerator AwaitEnemyTurn()
    {
        TargetSizeFrame = AttackFrameSize_1;
        yield return new WaitUntil(() => IsEndAnimationFrame);
        IsEndAnimationFrame = false;
        IsEnemyTurn = true;
    }

    private IEnumerator AwaitVertushka()
    {
        for (int i = 0; i < VertushkaEnemyLabels.Count; i++)
        {
            if (!VertushkaEnemyLabels[i])
                continue;
            
            Destroy(VertushkaEnemyLabels[i].gameObject);
        }
        
        VertushkaEnemyLabels = new List<VertushkaLabel>();
        
        Heart.Instance.gameObject.SetActive(false);
        _vertuchkaSelectedScreen.SetActive(false);
        _vertuchkaScreen.SetActive(true);
        _targetAnimator.gameObject.SetActive(true);
        _targetAnimator.Play("Target_Intro");
        _stickAnimator.transform.localPosition = new Vector2(-7.47f, 0);
        
        yield return new WaitUntil(() => _targetAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1);
        
        _stickAnimator.gameObject.SetActive(true);
        var isMoveStick = true;
        
        while (isMoveStick)
        {
            yield return null;
            _stickAnimator.transform.localPosition = Vector2.MoveTowards(
                _stickAnimator.transform.localPosition, new Vector2(7.47f, 0), 
                Time.deltaTime * 10);

            if (_stickAnimator.transform.localPosition.x >= 7.47f)
            {
                isMoveStick = false;
            }
            
            if (Input.GetButtonDown("Submit"))
            {
                isMoveStick = false;
            }
        }
        
        _stickAnimator.Play("Stick");
        var hitLeg = Instantiate(Resources.Load<GameObject>("Hit Leg"));
        hitLeg.transform.position = Enemies[SelectVertushkaIndex].transform.position;
        _lazSFX.Play();
        
        var coefficientAccuracy = 3f;
        var distance = Mathf.Abs(_stickAnimator.transform.localPosition.x);

        if (distance < 0.6f)
        {
            coefficientAccuracy = 3f;
        }
        else if (distance < 2.5f)
        {
            coefficientAccuracy = 2.5f;
        }
        else if (distance < 5.31f)
        {
            coefficientAccuracy = 2f;
        }
        else if (distance < 6.64f)
        {
            coefficientAccuracy = 1f;
        }
        else
        {
            coefficientAccuracy = 0f;
        }
        
        yield return new WaitUntil(() => hitLeg.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1);
        Destroy(hitLeg);
        
        var at = -2 + 2 * Stats.Instance.LV;
        var weaponPower = Stats.GetWeaponATK(Stats.Instance.Weapon);
        var defence = Enemies[SelectVertushkaIndex].Defence;
        var random = Random.Range(0, 3);
        var damage = (int)((at + weaponPower - defence + random) * coefficientAccuracy);
        
        if (Enemies[SelectVertushkaIndex].Relationship >= 0 && damage < Enemies[SelectVertushkaIndex].Health)
        {
            damage = Enemies[SelectVertushkaIndex].Health;
        }
        
        if (Enemies[SelectVertushkaIndex].Name == "Герман" && coefficientAccuracy != 0)
        {
            damage = 32;
        }

        if (Enemies[SelectVertushkaIndex].Name == "Тётя Тома")
        {
            if (Enemies[SelectVertushkaIndex].Health <= 190)
            {
                damage = Enemies[SelectVertushkaIndex].Health;
            }
            else if (Stats.Instance.IsGenocide)
            {
                damage = Enemies[SelectVertushkaIndex].Health;
            }
        }
        
        _damageSFX.Play();
        var damageLabel = Instantiate(Resources.Load<DamageIndicator>("Damage Indicator"));
        yield return damageLabel.Init(Enemies[SelectVertushkaIndex], damage);

        Enemies[SelectVertushkaIndex].Health -= damage;

        if (Enemies[SelectVertushkaIndex].Health <= 0)
        {
            IsEnemyDead = true;
        }
        
        Destroy(damageLabel.gameObject);
        
        _targetAnimator.Play("Target_Exit");
        yield return new WaitUntil(() => _targetAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1);
        
        _targetAnimator.gameObject.SetActive(false);
        _stickAnimator.gameObject.SetActive(false);
        _vertuchkaScreen.SetActive(false);

        var isEveryoneNotActive = true;

        foreach (var enemy in Enemies)
        {
            if (enemy.Health > 0)
                isEveryoneNotActive = false;
        }
        
        if (!isEveryoneNotActive)
        {
            StartCoroutine(AwaitEnemyTurn());
        }
        else
        {
            StartCoroutine(AwaitExitMessage());
        }
    }

    private IEnumerator AwaitRunHeart()
    {
        Heart.Instance.gameObject.SetActive(false);
        _runHeart.SetActive(true);
        _runSFX.Play();

        while (_runHeart.transform.localPosition != new Vector3(-6.5f, 0.06f))
        {
            _runHeart.transform.localPosition = Vector2.MoveTowards(_runHeart.transform.localPosition, 
                new Vector3(-6.5f, 0.06f), Time.deltaTime * 2);
            yield return null;
        }
        
        _runHeart.SetActive(false);
        _soryanScreen.gameObject.SetActive(false);

        StartCoroutine(AwaitExitMessage());
    }

    public IEnumerator AwaitExitMessage()
    {
        Heart.Instance.gameObject.SetActive(false);
        IsEndBattle = true;
        TargetSizeFrame = PlayerTurnFrameSize_1;
        
        while (_frame.size != TargetSizeFrame)
        {
            yield return null;
            _frame.size = Vector2.MoveTowards(_frame.size, TargetSizeFrame, 
                Time.deltaTime * SPEED_SIZE_FRAME);
        }

        if (Enemies.Count != 0 && Enemies[0].Name == "DJ Николай")
        {
            
        }
        else
        {
            MusicManager.Instance.Stop();
        }
        
        var rub = 0;
        var exp = 0;

        foreach (var enemy in Enemies)
        {
            if (!enemy.IsActive)
            {
                rub += enemy.RUB;
            }

            if (enemy.Health <= 0)
            {
                exp += enemy.EXP;

                if (enemy.EXP > 0)
                {
                    Stats.Instance.Kills++;
                }
            }
            else
            {
                if (enemy.EXP > 0)
                {
                    Stats.Instance.Spared++;
                }
            }
        }
        
        if (Stats.Instance.Kills > 5 && Stats.Instance.Spared == 0)
        {
            Stats.Instance.IsGenocide = true;
        }

        if (Stats.Instance.Spared > 0)
        {
            Stats.Instance.IsGenocide = false;
        }
        
        Stats.Instance.RUB += rub;
        Stats.Instance.EXP += exp;

        var endMessage = $"Вы победили!*Вы получили {exp} ОП и {rub} РУБ.";
        var levels = 0;
        
        while (Stats.Instance.EXP >= Stats.GetNextEXP(Stats.Instance.LV))
        {
            levels++;
            Stats.Instance.EXP -= Stats.GetNextEXP(Stats.Instance.LV);
            Stats.Instance.LV++;
            Stats.Instance.MaxHP += 4;
            Stats.Instance.HP = Stats.Instance.MaxHP;

            if (levels > 1)
            {
                if (levels == 2 && AdditionalExitText == null)
                {
                    AdditionalExitText = new [] {"Ваш УР повышен!"};
                }
                
                if (levels > 2)
                    AdditionalExitText[0] += "*Ваш УР повышен!";
            }
            else
            {
                endMessage += "*Ваш УР повышен!";
            }
            
            _levelUPSFX.Play();
            _lvLabel.text = $"УР {Stats.Instance.LV}";
        }
        
        _mainText.SetText(endMessage, false);
        yield return new WaitUntil(() => _mainText.IsSkip);
        yield return new WaitUntil(() => Input.GetButtonDown("Submit"));

        if (AdditionalExitText != null)
        {
            _mainText.SetText(AdditionalExitText, false);
            yield return new WaitUntil(() => _mainText.IsSkip);
            yield return new WaitUntil(() => Input.GetButtonDown("Submit"));
        }
        
        CoroutineRunner.Instance.StartCoroutine(AwaitExit());
    }
    
    public IEnumerator AwaitExit()
    {
        _blackout.gameObject.SetActive(true);
        _spareSFX.Play();
        Heart.Instance.GetComponent<SpriteRenderer>().sortingOrder = 1;

        while (_blackout.color.a < 1)
        {
            yield return null;
            var color = _blackout.color;
            color.a += Time.deltaTime;
            _blackout.color = color;
        }
        
        yield return SceneManager.LoadSceneAsync("Overworld", LoadSceneMode.Additive);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Overworld"));
        SceneManager.LoadScene(Stats.Instance.LevelName, LoadSceneMode.Additive);

        yield return SceneManager.UnloadSceneAsync("Battle", UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
        
        Fedya.Instance.transform.position = Stats.Instance.Position;
        Debug.Log("Перешли на другой уровень");
    }

    public bool IsEnemySelected(Enemy enemy)
    {
        if (SelectMainButtonIndex == 0 && Enemies[SelectVertushkaIndex].IsActive)
        {
            return Enemies[SelectVertushkaIndex] == enemy;
        }
        else if (SelectMainButtonIndex == 1 && Enemies[SelectBazarSelectedIndex].IsActive)
        {
            return Enemies[SelectBazarSelectedIndex] == enemy;
        }

        if (Enemies[0].IsActive)
        {
            if (enemy == Enemies[0])
                return true;

            return false;
        }


        if (Enemies.Count > 1 && Enemies[1].IsActive)
        {
            if (enemy == Enemies[1])
                return true;

            return false;
        }

        if (Enemies.Count > 2 && Enemies[2].IsActive && enemy == Enemies[2])
        {
            if (enemy == Enemies[2])
                return true;

            return false;
        }
        
        return Enemies[0] == enemy;
    }
}

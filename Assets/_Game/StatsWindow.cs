using System;
using TMPro;
using UnityEngine;

public class StatsWindow : MonoBehaviour
{
    [SerializeField]
    private Transform _heart;

    [SerializeField]
    private Transform _itemsContainer;

    [SerializeField]
    private Transform _statsContainer;

    [SerializeField]
    private TMP_Text _nameLabel;
    
    [SerializeField]
    private TMP_Text _lvLabel;
    
    [SerializeField]
    private TMP_Text _hpLabel;
    
    [SerializeField]
    private TMP_Text _rubLabel;
    
    [SerializeField]
    private  TMP_Text[] _itemLabels;
    
    [SerializeField]
    private TMP_Text _nameStatsLabel;
    
    [SerializeField]
    private TMP_Text _lvStatsLabel;
    
    [SerializeField]
    private TMP_Text _hpStatsLabel;
    
    [SerializeField]
    private TMP_Text _atkStatsLabel;
    
    [SerializeField]
    private TMP_Text _defStatsLabel;
    
    [SerializeField]
    private TMP_Text _expStatsLabel;
    
    [SerializeField]
    private TMP_Text _nextExpStatsLabel;
    
    [SerializeField]
    private TMP_Text _weaponStatsLabel;
    
    [SerializeField]
    private TMP_Text _armorStatsLabel;
    
    [SerializeField]
    private TMP_Text _rubStatsLabel;

    [SerializeField]
    private AudioSource _selectSFX;
    
    [SerializeField]
    private AudioSource _submitSFX;
    
    [SerializeField]
    private AudioSource _healSFX;
    
    private int _mainIndex;
    private bool _isMain = true;
    private bool _isItemsContainer;
    private bool _isStatsContainer;
    private int _itemsIndex;
    private bool _isSelectItemsContainer;
    private int _selectItemsIndex;

    private void OnEnable()
    {
        _heart.localPosition = new Vector2(-6.35f, 0.88f);
        UpgradeStats();
        _submitSFX.Play();
    }
    
    private void Update()
    {
        if (_isMain)
        {
            if (Input.GetButtonDown("OpenInventory") || Input.GetButtonDown("Cancel"))
            {
                gameObject.SetActive(false);
                Fedya.Instance.enabled = true;
                _submitSFX.Play();

                return;
            }
            else if (Input.GetButtonDown("Vertical"))
            {
                if (Input.GetAxisRaw("Vertical") < 0)
                {
                    if (_mainIndex == 0)
                    {
                        _mainIndex = 1;
                        _selectSFX.Play();
                    }
                }
                else if (Input.GetAxisRaw("Vertical") > 0)
                {
                    if (_mainIndex == 1)
                    {
                        _mainIndex = 0;
                        _selectSFX.Play();
                    }
                }
            }
            else if (Input.GetButtonDown("Submit"))
            {
                if (_mainIndex == 0)
                {
                    _isItemsContainer = true;
                    _isMain = false;
                    _itemsContainer.gameObject.SetActive(true);
                    _submitSFX.Play();
                    
                    for (int i = 0; i < Stats.Instance.Items.Length; i++)
                    {
                        _itemLabels[i].text = Stats.Instance.Items[i];
                    }
                }
                else if (_mainIndex == 1)
                {
                    _submitSFX.Play();
                    _isStatsContainer = true;
                    _isMain = false;
                    _statsContainer.gameObject.SetActive(true);
                }
            }

            if (_mainIndex == 0)
            {
                _heart.localPosition = new Vector2(-6.35f, 0.88f);
            }
            else if (_mainIndex == 1)
            {
                _heart.localPosition = new Vector2(-6.35f, -0.02f);
            }
        } 
        else if (_isItemsContainer)
        {
            if (Input.GetButtonDown("OpenInventory"))
            {
                _itemsContainer.gameObject.SetActive(false);
                _isItemsContainer = false;
                _isMain = true;
                
                gameObject.SetActive(false);
                Fedya.Instance.enabled = true;
                _submitSFX.Play();
            }
            else if (Input.GetButtonDown("Vertical"))
            {
                if (Input.GetAxisRaw("Vertical") < 0)
                {
                    if (_itemsIndex < 7)
                    {
                        _itemsIndex++;
                        _selectSFX.Play();
                    }
                }
                else if (Input.GetAxisRaw("Vertical") > 0)
                {
                    if (_itemsIndex > 0)
                    {
                        _itemsIndex--;
                        _selectSFX.Play();
                    }
                }
            }
            else if (Input.GetButtonDown("Cancel"))
            {
                _itemsContainer.gameObject.SetActive(false);
                _isItemsContainer = false;
                _isMain = true;
            }
            else if (Input.GetButtonDown("Submit"))
            {
                if (Stats.Instance.Items[_itemsIndex] == string.Empty)
                    return;
                
                _isItemsContainer = false; 
                _isSelectItemsContainer = true; 
                _submitSFX.Play();
            }

            _heart.localPosition = _itemsIndex switch
            {
                0 => new Vector2(-2.6f, 3.65f),
                1 => new Vector2(-2.6f, 2.799f),
                2 => new Vector2(-2.6f, 1.993f),
                3 => new Vector2(-2.6f, 1.14f),
                4 => new Vector2(-2.6f, 0.27f),
                5 => new Vector2(-2.6f, -0.64f),
                6 => new Vector2(-2.6f, -1.46f),
                7 => new Vector2(-2.6f, -2.21f),
            };
        }
        else if (_isStatsContainer)
        {
            _heart.gameObject.SetActive(false);

            if (Input.GetButtonDown("OpenInventory"))
            {
                _statsContainer.gameObject.SetActive(false);
                _heart.gameObject.SetActive(true);
                _isStatsContainer = false;
                _isMain = true;
                
                gameObject.SetActive(false);
                Fedya.Instance.enabled = true;
                _submitSFX.Play();
            }

            if (Input.GetButtonDown("Cancel"))
            {
                _statsContainer.gameObject.SetActive(false);
                _heart.gameObject.SetActive(true);
                _isStatsContainer = false;
                _isMain = true;
            }
        }
        else if (_isSelectItemsContainer)
        {
            if (Input.GetButtonDown("Horizontal"))
            {
                if (Input.GetAxisRaw("Horizontal") > 0)
                {
                    if (_selectItemsIndex < 2)
                    {
                        _selectItemsIndex++;
                        _selectSFX.Play();
                    }
                }
                else if (Input.GetAxisRaw("Horizontal") < 0)
                {
                    if (_selectItemsIndex > 0)
                    {
                        _selectSFX.Play();
                        _selectItemsIndex--;
                    }
                }
            }
            else if (Input.GetButtonDown("Cancel"))
            {
                _isItemsContainer = true; 
                _isSelectItemsContainer = false; 
            }
            else if (Input.GetButtonDown("Submit"))
            {
                _submitSFX.Play();
                
                if (_selectItemsIndex == 0)
                {
                    var item =  Stats.Instance.Items[_itemsIndex];
                    
                    if (Stats.IsWeapon(item))
                    {
                        CoroutineRunner.Instance.StartCoroutine(DialogueWindow.StartDialogue($"Вы экипировали \"{item}\""));
                        var currentWeapon = Stats.Instance.Weapon;
                        Stats.Instance.Weapon = item;
                        Stats.Instance.Items[_itemsIndex] = currentWeapon;
                    }
                    else if (Stats.IsArmor(item))
                    {
                        CoroutineRunner.Instance.StartCoroutine(DialogueWindow.StartDialogue($"Вы экипировали \"{item}\""));
                        var currentArmor = Stats.Instance.Armor;
                        Stats.Instance.Armor = item;
                        Stats.Instance.Items[_itemsIndex] = currentArmor;
                    }
                    else if (Stats.IsComida(item))
                    {
                        var hp = Stats.GetItemHP(item);
                        Stats.Instance.HP += hp;
                        
                        var beberMessage = $"Вы выпили \"{item}\".";
                        var eatMessage = $"Вы съели \"{item}\".";
                        
                        var message = item switch
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
                            message += "*Вы восстановили всё ОЗ.";
                        }
                        else
                        {
                            message += $"*Вы восстановили {hp} ОЗ.";
                        }
                        
                        _healSFX.Play();
                        CoroutineRunner.Instance.StartCoroutine(DialogueWindow.StartDialogue(message));
                        Stats.Instance.Items[_itemsIndex] = string.Empty;
                    }
                    else
                    {
                        CoroutineRunner.Instance.StartCoroutine(DialogueWindow.StartDialogue("Разработчик этого не добавил"));
                    }
                }
                else if (_selectItemsIndex == 1)
                {
                    var message = Stats.Instance.Items[_itemsIndex] switch
                    {
                        Constants.ROZOCHKA => Constants.ROZOCHKA_INFO,
                        Constants.JAGUAR => Constants.JAGUAR_INFO,
                        Constants.ANTIPOHMELIN => Constants.ANTIPOHMELIN_INFO,
                        Constants.K_PAL => Constants.K_PAL_INFO,
                        Constants.K_NIKE => Constants.K_NIKE_INFO,
                        Constants.K_ADIDAS => Constants.K_ADIDAS_INFO,
                        Constants.MASHA_JAM => Constants.MASHA_JAM_INFO,
                        Constants.MASHA_PIES => Constants.MASHA_PIES_INFO,
                        Constants.BELASH => Constants.BELASH_INFO,
                        Constants.NASTOYKA_GASTERA => Constants.NASTOYKA_GASTERA_INFO,
                        _ => "Разработчик этого не добавил"
                    };
                    
                    CoroutineRunner.Instance.StartCoroutine(DialogueWindow.StartDialogue(message));
                }
                else if (_selectItemsIndex == 2) // Выкинули предмет
                {
                    CoroutineRunner.Instance.StartCoroutine(DialogueWindow.StartDialogue($"Вы выбросили {Stats.Instance.Items[_itemsIndex]}"));
                    Stats.Instance.Items[_itemsIndex] = string.Empty;
                }

                _isMain = true;
                _itemsContainer.gameObject.SetActive(false);
                _isItemsContainer = false;
                _selectItemsIndex = 0;
                
                gameObject.SetActive(false);
                Fedya.Instance.enabled = true;
            }

            _heart.localPosition = _selectItemsIndex switch
            {
                0 => new Vector2(-2.6f, -3.42f),
                1 => new Vector2(-0.14f, -3.42f),
                2 => new Vector2(2.69f, -3.42f),
            };
        }
    }

    private void UpgradeStats()
    {
        var lv = Stats.Instance.LV;
        
        _nameLabel.text = $"{Stats.Instance.Name}";
        _lvLabel.text = $"УР  {Stats.Instance.LV}";
        _hpLabel.text = $"ОЗ  {Stats.Instance.HP}/{Stats.Instance.MaxHP}";
        _rubLabel.text = $"РУБ {Stats.Instance.RUB}";

        for (int i = 0; i < Stats.Instance.Items.Length; i++)
        {
            _itemLabels[i].text = Stats.Instance.Items[i];
        }
        
        _nameStatsLabel.text = $"\"{Stats.Instance.Name}\"";
        _lvStatsLabel.text = $"УР {Stats.Instance.LV}";
        _hpStatsLabel.text = $"ОЗ {Stats.Instance.HP}/{Stats.Instance.MaxHP}";
        _atkStatsLabel.text = $"АТК {Stats.GetBaseATK(lv) + Stats.GetWeaponATK(Stats.Instance.Weapon)}({Stats.GetBaseATK(lv)})";
        _defStatsLabel.text = $"ЗАЩ {Stats.GetBaseDEF(lv) + Stats.GetArmorDEF(Stats.Instance.Armor)}({Stats.GetBaseDEF(lv)})";
        _expStatsLabel.text = $"ОП:{Stats.Instance.EXP}";
        _nextExpStatsLabel.text = $"СЛ ОП:{Stats.GetNextEXP(lv)}";
        _weaponStatsLabel.text = $"ОРУЖИЕ: {Stats.Instance.Weapon}";
        _armorStatsLabel.text = $"БРОНЯ: {Stats.Instance.Armor}";
        _rubStatsLabel.text = $"РУБ {Stats.Instance.RUB}";
    }
}

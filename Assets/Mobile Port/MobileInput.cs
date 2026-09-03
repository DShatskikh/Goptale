using System;
using UnityEngine;
using UnityEngine.UIElements;

public sealed class MobileInput : MonoBehaviour
{
    private UIDocument _uiDocument;
    private Button _buttonZ;
    private Button _buttonX;
    private Button _buttonC;
    private Button _buttonHide;
    private UIScreenJoystick _joystick;
    private Vector2 _previous;

    private bool _isOpenInventoryDown;
    private bool _isSubmitDown;
    private bool _isCancelDown;
    private bool _isHide;
    private VisualElement _container;

    private void Awake()
    {
#if PLATFORM_STANDALONE
        Destroy(gameObject);
        return;
#endif
        
        DontDestroyOnLoad(this);
        _uiDocument = GetComponent<UIDocument>();
        _joystick = GetComponent<UIScreenJoystick>();
    }

    private void OnEnable()
    {
        _container = _uiDocument.rootVisualElement.Q<VisualElement>("container");
        
        _buttonZ = _uiDocument.rootVisualElement.Q<Button>("button-z");
        _buttonZ.RegisterCallback<ClickEvent>(_ =>
        {
            _isSubmitDown = true;
        });
        
        _buttonX = _uiDocument.rootVisualElement.Q<Button>("button-x");
        _buttonX.RegisterCallback<ClickEvent>(_ =>
        {
            _isCancelDown = true;
        });
        
        _buttonC = _uiDocument.rootVisualElement.Q<Button>("button-c");
        _buttonC.RegisterCallback<ClickEvent>(_ =>
        {
            _isOpenInventoryDown = true;
        });
        
        _buttonHide = _uiDocument.rootVisualElement.Q<Button>("button-hide");
        
        _buttonHide.RegisterCallback<ClickEvent>(_ =>
        {
            _isHide = !_isHide;
            
            _buttonHide.style.opacity = new StyleFloat()
            {
                value = _isHide ? 0.5f : 1f
            };
            
            _buttonC.style.visibility = new StyleEnum<Visibility>()
            {
                value = _isHide ? Visibility.Hidden : Visibility.Visible
            };
            
            _buttonZ.style.visibility = new StyleEnum<Visibility>()
            {
                value = _isHide ? Visibility.Hidden : Visibility.Visible
            };
            
            _buttonX.style.visibility = new StyleEnum<Visibility>()
            {
                value = _isHide ? Visibility.Hidden : Visibility.Visible
            };
            
            _uiDocument.rootVisualElement.Q<VisualElement>("joystik").style.visibility = new StyleEnum<Visibility>()
            {
                value = _isHide ? Visibility.Hidden : Visibility.Visible
            };
        });
    }

    private void Update()
    {
        float targetAspect = 640f / 480f; // 1.333333333333333
        _container.style.width = new Length(Screen.height * targetAspect, LengthUnit.Pixel);
     
        if (_isOpenInventoryDown)
        {
            _isOpenInventoryDown = false;
            InputManager.Instance.IsOpenInventoryDown = true;
        }
        
        if (_isSubmitDown)
        {
            _isSubmitDown = false;
            InputManager.Instance.IsSubmitDown = true;
        }
        
        if (_isCancelDown)
        {
            _isCancelDown = false;
            InputManager.Instance.IsCancelDown = true;
        }
        
        int direction = GetDirection(_joystick.InputVector);
        int previousDirection = GetDirection(_previous);

        InputManager.Instance.IsVerticalDown = false;
        InputManager.Instance.IsHorizontalDown = false;
        
        if (direction != previousDirection)
        {
            if (direction == 1)
            {
                InputManager.Instance.IsVerticalDown = true;
            }
            else if (direction == 2)
            {
                InputManager.Instance.IsHorizontalDown = true;
            }
            else if (direction == 3)
            {
                InputManager.Instance.IsVerticalDown = true;
            }
            else if (direction == 4)
            {
                InputManager.Instance.IsHorizontalDown = true;
            }
        }
        
        InputManager.Instance.Horizontal = _joystick.InputVector.x;
        
        InputManager.Instance.Vertical = _joystick.InputVector.y;
        var input = new Vector2(InputManager.Instance.Horizontal, InputManager.Instance.Vertical).normalized;
        
        if (Mathf.Abs(input.y) > 0.75f)
        {
            InputManager.Instance.Horizontal = 0;
            InputManager.Instance.Vertical /= Mathf.Abs(InputManager.Instance.Vertical);
        } 
        else if (Mathf.Abs(input.x) > 0.75f)
        {
            InputManager.Instance.Vertical = 0;
            InputManager.Instance.Horizontal /= Mathf.Abs(InputManager.Instance.Horizontal);
        }
        
        _previous = _joystick.InputVector;
    }

    private void LateUpdate()
    {
       InputManager.Instance.IsOpenInventoryDown = false;
       InputManager.Instance.IsSubmitDown = false;
       InputManager.Instance.IsCancelDown = false;
    }

    private int GetDirection(Vector2 input)
    {
        if (Mathf.Abs(input.x) < Mathf.Abs(input.y))
        {
            if (input.y > 0)
            {
                return 1;
            }
            else
            {
                return 3;
            }
        }
        
        if (Mathf.Abs(input.x) > Mathf.Abs(input.y))
        {
            if (input.x > 0)
            {
                return 2;
            }
            else
            {
                return 4;
            }
        }

        return 0;
    }
}

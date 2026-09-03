using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

public class UIScreenJoystick : MonoBehaviour
{
    [Header("Настройки")]
    [FormerlySerializedAs("joystickRootName"),SerializeField]
    private string _joystickRootName = "move-joystick";
    
    [FormerlySerializedAs("stickName"),SerializeField]
    private string _stickName = "move-joystick-center";
    
    [FormerlySerializedAs("maxRadius"), SerializeField]
    private float _maxRadius = 80f;

    private VisualElement _joystickRoot;
    private VisualElement _stick;
    private Vector2 _startPosition;
    private Vector2 _inputVector = Vector2.zero;
    private bool _isDragging = false;
    private bool _isUp;
    private bool _isDown;
    
    public Vector2 InputVector => new Vector2(_inputVector.x, -_inputVector.y);
    public bool IsDragging => _isDragging;
    public bool IsUp;
    public bool IsDown;
    public event Action<bool> Dragging;

    void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        _joystickRoot = root.Q<VisualElement>(_joystickRootName);
        _stick = _joystickRoot?.Q<VisualElement>(_stickName);

        if (_joystickRoot == null || _stick == null)
        {
            Debug.LogError($"Не найдены элементы джойстика: {_joystickRootName} или {_stickName}");
            enabled = false;
            return;
        }

        RegisterEvents();
        SetDefaultStyle();
    }

    private void Update()
    {
        if (_isUp)
        {
            _isUp = false;
            IsUp = true;
        }
        
        if (_isDown)
        {
            _isDown = false;
            IsDown = true;
        }
    }

    private void LateUpdate()
    {
        IsUp = false;
        IsDown = false;
    }

    private void RegisterEvents()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        
        _joystickRoot.RegisterCallback<PointerDownEvent>(OnPointerDown);
        root.RegisterCallback<PointerMoveEvent>(OnGlobalPointerMove);
        root.RegisterCallback<PointerUpEvent>(OnGlobalPointerUp);
    }

    private void OnPointerDown(PointerDownEvent evt)
    {
        _isDragging = true;
        Dragging?.Invoke(_isDragging);
        
        // Сохраняем начальную позицию
        _startPosition = evt.position;
        
        _stick.style.translate = new Translate(0, 0, 0);
        _inputVector = Vector2.zero;
        
        // Делаем стик абсолютным, чтобы он мог двигаться за пределы корня
        _stick.style.position = Position.Absolute;
        
        evt.StopPropagation();
        
        _isDown = true;
    }

    private void OnGlobalPointerMove(PointerMoveEvent evt)
    {
        if (!_isDragging) return;
        
        // Текущая позиция пальца
        Vector2 currentPos = evt.position;
        
        // Вектор от начальной точки касания
        Vector2 offset = currentPos - _startPosition;
        
        // Ограничиваем максимальным радиусом
        Vector2 clampedOffset = Vector2.ClampMagnitude(offset, _maxRadius);
        
        // Нормализуем входной вектор
        _inputVector = clampedOffset / _maxRadius;
        
        // Двигаем стик относительно центра джойстика
        // Применяем смещение напрямую, без привязки к границам родителя
        _stick.style.translate = new Translate(clampedOffset.x, clampedOffset.y, 0);
    }

    private void OnGlobalPointerUp(PointerUpEvent evt)
    {
        if (_isDragging)
            StopDrag();

        _isUp = true;
    }

    private void StopDrag()
    {
        _isDragging = false;
        Dragging?.Invoke(_isDragging);
        _inputVector = Vector2.zero;
        _stick.style.translate = new Translate(0, 0, 0);
    }

    private void SetDefaultStyle()
    {
        _joystickRoot.style.position = Position.Absolute;
        _joystickRoot.style.overflow = Overflow.Visible; // Разрешаем выход за пределы
        _stick.style.translate = new Translate(0, 0, 0);
        _stick.style.position = Position.Absolute; // Абсолютное позиционирование
    }
}
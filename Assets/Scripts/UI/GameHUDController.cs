using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System;

public class GameHUDController : MonoBehaviour
{
    [SerializeField] private GameController _gameController;
    //[SerializeField] private PlayerCharacter _playerCharacter;
    [SerializeField] private Health _health;

    private UIDocument _document;
    private VisualElement _loseMenu;
    private VisualElement _winMenu;
    private Button _winRetryButton;
    private Button _retryButton;

    private VisualElement _playerHUDVisualTree;
    private VisualElement _expBarFill;
    private VisualElement _healthBarFill;

    private Label _elapsedTimeLabel;

    private VisualElement _joystickBG;
    private VisualElement _joystickCenter;
    private VisualElement _root;

    private AudioSource _audioSource;

    private Button _attackButton;
    private Button _interactButton;
    private List<Button> _buttons = new List<Button>();

    private bool _moving;
    private Vector2 _joystickCenterPos;
    private Vector2 _joystickInput = Vector2.zero;

    public event Action OnInteract;
    public event Action OnAttack;

    public Vector2 JoystickInput
    {
        get => _joystickInput;
        set => _joystickInput = value;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        _document = GetComponent<UIDocument>();
        
        _loseMenu = _document.rootVisualElement.Q("LoseMenuVisualTree");
        _retryButton = _loseMenu.Q("RetryButton") as Button;

        _winMenu = _document.rootVisualElement.Q("WinMenuVisualTree");
        _winRetryButton = _winMenu.Q("RetryButton") as Button;
        
        _playerHUDVisualTree = _document.rootVisualElement.Q("PlayerHUDVisualTree");
        _expBarFill = _playerHUDVisualTree.Q("ProgressBarFill");
        //Debug.Log(_expBarFill);
        _healthBarFill = _playerHUDVisualTree.Q("HealthProgressBarFill");
        //Debug.Log(_healthBarFill);
        

        _elapsedTimeLabel = _document.rootVisualElement.Q("ElapsedTimeLabel") as Label;

        _audioSource = GetComponent<AudioSource>();
        _root = _document.rootVisualElement.Q("PlayerHUDVisualTree");
        _joystickBG = _root.Q("Joystick");
        _joystickCenter = _root.Q("JoystickCenter");

        _joystickCenterPos = new Vector2(230, 760);

        _attackButton = _root.Q("AttackButton") as Button;
        Debug.Log(_attackButton);
        _interactButton = _root.Q("InteractButton") as Button;

        /**
        foreach (Button button in _buttons)
        {
            button.RegisterCallback<ClickEvent>(OnAnyButtonClick);
        }
        **/
        _attackButton.RegisterCallback<ClickEvent>(OnAttackClick);
        _interactButton.RegisterCallback<ClickEvent>(OnInteractClick);

        _joystickBG.RegisterCallback<PointerDownEvent>(OnJoystickClick);
        _root.RegisterCallback<PointerUpEvent>(OnJoystickRelease);
        _root.RegisterCallback<PointerMoveEvent>(OnJoystickHold);
        _root.RegisterCallback<PointerLeaveEvent>(OnScreenLeave);
    }

    void Start()
    {
        DisableAllDisplays();
    }

    private void OnEnable()
    {
        
        _loseMenu.RegisterCallback<ClickEvent>(OnRetryButtonClick);
        _winMenu.RegisterCallback<ClickEvent>(OnRetryButtonClick);

        _gameController.OnLose += DisplayLoseMenu;
        _gameController.OnWin += DisplayWinMenu;
    }

    public void SubscribeHealthEvents(Health health)
    {
        _health = health;
        _health.OnHealthChanged += SetHealthBarPercent;
        
    }

    private void OnDisable()
    {
        
        _loseMenu.UnregisterCallback<ClickEvent>(OnRetryButtonClick);
        _winMenu.UnregisterCallback<ClickEvent>(OnRetryButtonClick);

        _health.OnHealthChanged -= SetHealthBarPercent;
        _health.OnDeath -= DisplayLoseMenu;

        _attackButton.UnregisterCallback<ClickEvent>(OnAttackClick);
        _interactButton.UnregisterCallback<ClickEvent>(OnInteractClick);

        _joystickBG.UnregisterCallback<PointerDownEvent>(OnJoystickClick);
        _root.UnregisterCallback<PointerUpEvent>(OnJoystickRelease);
        _root.UnregisterCallback<PointerMoveEvent>(OnJoystickHold);
        _root.UnregisterCallback<PointerLeaveEvent>(OnScreenLeave);

        _gameController.OnLose -= DisplayLoseMenu;
        _gameController.OnWin -= DisplayWinMenu;
    }

    private void DisableAllDisplays()
    {
        _loseMenu.style.display = DisplayStyle.None;
        _winMenu.style.display = DisplayStyle.None;
    }

    private void DisplayLoseMenu()
    {
        _loseMenu.style.display = DisplayStyle.Flex;
        _playerHUDVisualTree.style.display = DisplayStyle.None;
    }

    private void DisplayWinMenu()
    {
        _winMenu.style.display = DisplayStyle.Flex;
        _playerHUDVisualTree.style.display = DisplayStyle.None;
    }

    private void OnRetryButtonClick(ClickEvent evt)
    {
        _loseMenu.style.display = DisplayStyle.None;
        _winMenu.style.display = DisplayStyle.None;
        _gameController.ReloadLevel();
    }

    private void SetHealthBarPercent(float currentEXP, float expForLevelUp)
    {
        float percentage = ((1 / expForLevelUp) * currentEXP) * 100;
        _healthBarFill.style.height = Length.Percent(percentage);
    }

    private void Update()
    {
        float elapsedTime = _gameController.ElapsedTime;
        int minutes = Mathf.FloorToInt(elapsedTime / 60) % 60;
        int seconds = Mathf.FloorToInt(elapsedTime % 60);
        string textElapsedTime = string.Format("{0:0}:{1:00}", minutes, seconds);
        _elapsedTimeLabel.text = textElapsedTime;
    }

    private void OnJoystickClick(PointerDownEvent evt)
    {
        _moving = true;
    }

    private void OnJoystickRelease(PointerUpEvent evt)
    {
        _moving = false;
        _joystickInput = Vector2.zero;
        _joystickCenter.style.translate = new StyleTranslate(new Translate(new Length(0, LengthUnit.Pixel), new Length(0, LengthUnit.Pixel)));
    }

    private void OnJoystickHold(PointerMoveEvent evt)
    {
        if (_moving)
        {
            _joystickInput = new Vector2(Mathf.Clamp(evt.position.x - _joystickCenterPos.x, -50, 50), Mathf.Clamp(evt.position.y - _joystickCenterPos.y, -50, 50));
            _joystickInput = Vector2.ClampMagnitude(_joystickInput, 50);
            _joystickCenter.style.translate = new StyleTranslate(new Translate(new Length(_joystickInput.x, LengthUnit.Pixel), new Length(_joystickInput.y, LengthUnit.Pixel)));
        }
    }

    private void OnScreenLeave(PointerLeaveEvent evt)
    {
        /**
        _moving = false;
        _joystickCenter.style.translate = new StyleTranslate(new Translate(new Length(0, LengthUnit.Pixel), new Length(0, LengthUnit.Pixel)));
        **/
    }

    private void OnAnyButtonClick(ClickEvent evt)
    {
        _audioSource.Play();
    }

    private void OnAttackClick(ClickEvent evt)
    {
        Debug.Log("Attack");
        OnAttack?.Invoke();
    }

    private void OnInteractClick(ClickEvent evt)
    {
        Debug.Log("Interact");
        OnInteract?.Invoke();
    }

}

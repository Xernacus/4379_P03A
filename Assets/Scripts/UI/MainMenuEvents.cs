using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class MainMenuEvents : MonoBehaviour
{
    private UIDocument _document;

    private VisualElement _rootMenu;
    private VisualElement _settingsMenu;
    private VisualElement _creditsMenu;

    private Button _button;
    private Button _settingsButton;
    private Button _settingsBackButton;
    private Button _quitButton;

    private List<Button> _menuButtons = new List<Button>();

    private AudioSource _audioSource;

    [SerializeField] private string _startLevelName;
    //[SerializeField] private AudioClip _menuAudioClip;
    //[SerializeField] private AudioClip _gameAudioClip;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _document = GetComponent<UIDocument>();

        _rootMenu = _document.rootVisualElement.Q("RootMenu");
        _settingsMenu = _document.rootVisualElement.Q("SettingsMenu");
        _creditsMenu = _document.rootVisualElement.Q("CreditsMenu");

        _button = _document.rootVisualElement.Q("StartGameButton") as Button;
        _button.RegisterCallback<ClickEvent>(OnPlayGameClick);

        _quitButton = _document.rootVisualElement.Q("QuitButton") as Button;
        _quitButton.RegisterCallback<ClickEvent>(OnQuitGameClick);

        _settingsButton = _document.rootVisualElement.Q("SettingsButton") as Button;
        _settingsButton.RegisterCallback<ClickEvent>(OnSettingsButtonClick);

        _settingsBackButton = _document.rootVisualElement.Q("SettingsBackButton") as Button;
        _settingsBackButton.RegisterCallback<ClickEvent>(OnSettingsBackButtonClick);

        _menuButtons = _document.rootVisualElement.Query<Button>().ToList();
        for (int i = 0; i < _menuButtons.Count; i++)
        {
            _menuButtons[i].RegisterCallback<ClickEvent>(OnAllButtonsClick);
        }
    }

    private void OnDisable()
    {
        _button.UnregisterCallback<ClickEvent>(OnPlayGameClick);
        for (int i = 0; i < _menuButtons.Count; i++)
        {
            _menuButtons[i].UnregisterCallback<ClickEvent>(OnAllButtonsClick);
        }

        _settingsButton.UnregisterCallback<ClickEvent>(OnSettingsButtonClick);

        _settingsBackButton.UnregisterCallback<ClickEvent>(OnSettingsBackButtonClick);

        _quitButton.UnregisterCallback<ClickEvent>(OnQuitGameClick);
    }

    private void OnPlayGameClick(ClickEvent evt)
    {
        SceneManager.LoadScene(_startLevelName);
    }

    private void OnQuitGameClick(ClickEvent evt)
    {
        Application.Quit();
    }

    private void OnAllButtonsClick(ClickEvent evt)
    {
        _audioSource.Play();
    }

    private void OnSettingsButtonClick(ClickEvent evt)
    {
        _rootMenu.style.display = DisplayStyle.None;
        _settingsMenu.style.display = DisplayStyle.Flex;
    }


    private void OnSettingsBackButtonClick(ClickEvent evt)
    {
        _rootMenu.style.display = DisplayStyle.Flex;
        _settingsMenu.style.display = DisplayStyle.None;
    }

}

using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using static UnityEngine.Rendering.DebugUI.MessageBox;

public class GameMenuController : MonoBehaviour
{
    private VisualElement _gameplayMenuVisualTree;
    private VisualElement _pauseMenuVisualTree;

    private AudioSource _audioSource;

    private Button _pauseButton;
    private Button _resumeButton;
    private Button _quitButton;
    private List<Button> _buttons = new List<Button>();

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;
        _gameplayMenuVisualTree = root.Q("GameplayMenuVisualTree");
        _pauseMenuVisualTree = root.Q("PauseMenuVisualTree");

        _pauseButton = root.Q("PauseButton") as Button;
        _pauseButton.RegisterCallback<ClickEvent>(OnPauseButtonClick);

        _quitButton = root.Q("QuitButton") as Button;
        _quitButton.RegisterCallback<ClickEvent>(OnQuitButtonClick);

        _resumeButton = root.Q("ResumeButton") as Button;
        _resumeButton.RegisterCallback<ClickEvent>(OnResumeButtonClick);

        _buttons = root.Query<Button>().ToList();
        foreach (Button button in _buttons)
        {
            button.RegisterCallback<ClickEvent>(OnAnyButtonClick);
        }
        _gameplayMenuVisualTree.style.display = DisplayStyle.Flex;
        _pauseMenuVisualTree.style.display = DisplayStyle.None;
    }

    private void OnPauseButtonClick(ClickEvent evt)
    {
        _gameplayMenuVisualTree.style.display = DisplayStyle.None;
        _pauseMenuVisualTree.style.display = DisplayStyle.Flex;
    }

    private void OnResumeButtonClick(ClickEvent evt)
    {
        _gameplayMenuVisualTree.style.display = DisplayStyle.Flex;
        _pauseMenuVisualTree.style.display = DisplayStyle.None;
    }

    private void OnQuitButtonClick(ClickEvent evt)
    {
        SceneManager.LoadScene("MainMenu");
    }

    private void OnAnyButtonClick(ClickEvent evt)
    {
        _audioSource.Play();
    }

    private void OnDisable()
    {
        _pauseButton.UnregisterCallback<ClickEvent>(OnPauseButtonClick);
        _quitButton.UnregisterCallback<ClickEvent>(OnQuitButtonClick);
        foreach (Button button in _buttons)
        {
            button.UnregisterCallback<ClickEvent>(OnAnyButtonClick);
        }
    }
}

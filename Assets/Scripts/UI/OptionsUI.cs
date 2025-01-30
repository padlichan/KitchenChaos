using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsUI : MonoBehaviour
{
    public static OptionsUI Instance;

    [SerializeField] private Button closeButton;
    [SerializeField] private Button soundEffectsButton;
    [SerializeField] private Button musicButton;
    [SerializeField] private TextMeshProUGUI soundEffectsButtonText;
    [SerializeField] private TextMeshProUGUI musicButtonText;

    [SerializeField] private GameObject pressToRebindKeyGameObject;

    [SerializeField] private Button moveUpButton;
    [SerializeField] private Button moveDownButton;
    [SerializeField] private Button moveLeftButton;
    [SerializeField] private Button moveRightButton;
    [SerializeField] private Button interactButton;
    [SerializeField] private Button interactAltButton;
    [SerializeField] private Button pauseButton;
    [SerializeField] private Button gamepadInteractButton;
    [SerializeField] private Button gamepadInteractAltButton;
    [SerializeField] private Button gamepadPauseButton;

    [SerializeField] private TextMeshProUGUI moveUpButtonText;
    [SerializeField] private TextMeshProUGUI moveDownButtonText;
    [SerializeField] private TextMeshProUGUI moveLeftButtonText;
    [SerializeField] private TextMeshProUGUI moveRightButtonText;
    [SerializeField] private TextMeshProUGUI interactButtonText;
    [SerializeField] private TextMeshProUGUI interactAltButtonText;
    [SerializeField] private TextMeshProUGUI pauseButtonText;
    [SerializeField] private TextMeshProUGUI gamepadInteractButtonText;
    [SerializeField] private TextMeshProUGUI gamepadInteractAltButtonText;
    [SerializeField] private TextMeshProUGUI gamepadPauseButtonText;

    private Action onCloseButtonAction;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Debug.LogError("Multiple instances of OptionsUI in scene");

        soundEffectsButton.onClick.AddListener(() =>
        {
            SoundManager.Instance.ChangeVolume();
            UpdateVisual();
        });

        musicButton.onClick.AddListener(() =>
        {
            MusicManager.Instance.ChangeVolume();
            UpdateVisual();
        });

        closeButton.onClick.AddListener(() =>
        {
            Hide();
            onCloseButtonAction();
        });

        moveUpButton.onClick.AddListener(() => RebindBinding(Binding.MoveUp));
        moveDownButton.onClick.AddListener(() => RebindBinding(Binding.MoveDown));
        moveLeftButton.onClick.AddListener(() => RebindBinding(Binding.MoveLeft));
        moveRightButton.onClick.AddListener(() => RebindBinding(Binding.MoveRight));
        interactButton.onClick.AddListener(() => RebindBinding(Binding.Interact));
        interactAltButton.onClick.AddListener(() => RebindBinding(Binding.InteractAlt));
        pauseButton.onClick.AddListener(() => RebindBinding(Binding.Pause));
        gamepadInteractButton.onClick.AddListener(() => RebindBinding(Binding.GamepadInteract));
        gamepadInteractAltButton.onClick.AddListener(() => RebindBinding(Binding.GamepadInteractAlt));
        gamepadPauseButton.onClick.AddListener(() => RebindBinding(Binding.GamepadPause));
    }

    private void Start()
    {
        GameManager.Instance.OnGameUnpaused += GameManager_OnGameUnpaused;
        UpdateVisual();
        HidePressToRebindKey();
        Hide();
    }

    private void GameManager_OnGameUnpaused(object sender, System.EventArgs e)
    {
        Hide();
    }

    private void UpdateVisual()
    {
        int displaySoundEffectsVolume = (int)Mathf.Round(SoundManager.Instance.GetVolume() * 10);
        soundEffectsButtonText.text = $"Sound Effects: {displaySoundEffectsVolume}";
        int displayMusicVolume = (int)Mathf.Round(MusicManager.Instance.GetVolume() * 10);
        musicButtonText.text = $"Music: {displayMusicVolume}";

        moveUpButtonText.text = InputHandler.Instance.GetBindingText(Binding.MoveUp);
        moveDownButtonText.text = InputHandler.Instance.GetBindingText(Binding.MoveDown);
        moveLeftButtonText.text = InputHandler.Instance.GetBindingText(Binding.MoveLeft);
        moveRightButtonText.text = InputHandler.Instance.GetBindingText(Binding.MoveRight);
        interactButtonText.text = InputHandler.Instance.GetBindingText(Binding.Interact);
        interactAltButtonText.text = InputHandler.Instance.GetBindingText(Binding.InteractAlt);
        pauseButtonText.text = InputHandler.Instance.GetBindingText(Binding.Pause);
        interactButtonText.text = InputHandler.Instance.GetBindingText(Binding.Interact);
        interactAltButtonText.text = InputHandler.Instance.GetBindingText(Binding.InteractAlt);
        pauseButtonText.text = InputHandler.Instance.GetBindingText(Binding.Pause);
        gamepadInteractButtonText.text = InputHandler.Instance.GetBindingText(Binding.GamepadInteract);
        gamepadInteractAltButtonText.text = InputHandler.Instance.GetBindingText(Binding.GamepadInteractAlt);
        gamepadPauseButtonText.text = InputHandler.Instance.GetBindingText(Binding.GamepadPause);
    }
    public void Show(Action onCloseButtonAction)
    {
        this.onCloseButtonAction = onCloseButtonAction;
        gameObject.SetActive(true);
        soundEffectsButton.Select();
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    private void ShowPressToRebindKey()
    {
        pressToRebindKeyGameObject.SetActive(true);
    }

    private void HidePressToRebindKey()
    {
        pressToRebindKeyGameObject.SetActive(false);
    }

    private void RebindBinding(Binding binding)
    {
        ShowPressToRebindKey();
        InputHandler.Instance.RebindBinding(binding, () =>
        {
            HidePressToRebindKey();
            UpdateVisual();
        });
    }
}

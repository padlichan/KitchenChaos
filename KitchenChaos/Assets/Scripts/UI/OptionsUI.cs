using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsUI : MonoBehaviour
{
    public static OptionsUI Instance { get; private set; }
    [SerializeField] private Button closeButton;
    [SerializeField] private Button soundEffectsButton;
    [SerializeField] private Button musicButton;
    [SerializeField] private TextMeshProUGUI soundEffectsText;
    [SerializeField] private TextMeshProUGUI musicText;
    [SerializeField] private TextMeshProUGUI moveUpText;
    [SerializeField] private TextMeshProUGUI moveLeftText;
    [SerializeField] private TextMeshProUGUI moveRightText;
    [SerializeField] private TextMeshProUGUI moveDownText;
    [SerializeField] private TextMeshProUGUI interactText;
    [SerializeField] private TextMeshProUGUI interactAltText;
    [SerializeField] private TextMeshProUGUI pauseText;

    [SerializeField] private Button moveUpButton;
    [SerializeField] private Button moveLeftButton;
    [SerializeField] private Button moveRightButton;
    [SerializeField] private Button moveDownButton;
    [SerializeField] private Button interactButton;
    [SerializeField] private Button interactAltButton;
    [SerializeField] private Button pauseButton;

    [SerializeField] Transform pressToRebindKeyTransform;



    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Debug.LogError("There are multiple OptionsUI instances in the scene");

        closeButton.onClick.AddListener(() =>
        {
            Hide();
        });

        soundEffectsButton.onClick.AddListener(() =>
        {
            SoundManager.Instance.ChangeSoundEffectsVolume();
            UpdateVisual();
        });

        musicButton.onClick.AddListener(() =>
        {
            MusicManager.Instance.ChangeMusicVolume();
            UpdateVisual();
        });

        //Key rebindings
        moveUpButton.onClick.AddListener(() =>
        {
            RebindBinding(GameInput.Binding.MoveUp);
        });
        moveDownButton.onClick.AddListener(() =>
        {
            RebindBinding(GameInput.Binding.MoveDown);
        });
        moveLeftButton.onClick.AddListener(() =>
        {
            RebindBinding(GameInput.Binding.MoveLeft);
        });
        moveRightButton.onClick.AddListener(() =>
        {
            RebindBinding(GameInput.Binding.MoveRight);
        });
        interactButton.onClick.AddListener(() =>
        {
            RebindBinding(GameInput.Binding.Interact);
        });
        interactAltButton.onClick.AddListener(() =>
        {
            RebindBinding(GameInput.Binding.InteractAlt);
        });
        pauseButton.onClick.AddListener(() =>
        {
            RebindBinding(GameInput.Binding.Pause);
        });
    }

    private void Start()
    {
        GameManager.Instance.OnGameUnpaused += GameManager_OnGameUnpaused;
        HidePressToRebindKey();
        UpdateVisual();
        Hide();

    }

    private void GameManager_OnGameUnpaused(object sender, EventArgs e)
    {
        Hide();
    }

    private void UpdateVisual()
    {
        soundEffectsText.text = "Sound effects: " + Mathf.Round(SoundManager.Instance.SoundEffectsVolume * 10f);
        musicText.text = "Music: " + Mathf.Round(MusicManager.Instance.MusicVolume * 10f);

        moveUpText.text = GameInput.Instance.GetBindingText(GameInput.Binding.MoveUp);

        moveDownText.text = GameInput.Instance.GetBindingText(GameInput.Binding.MoveDown);

        moveLeftText.text = GameInput.Instance.GetBindingText(GameInput.Binding.MoveLeft);

        moveRightText.text = GameInput.Instance.GetBindingText(GameInput.Binding.MoveRight);

        interactText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Interact);

        interactAltText.text = GameInput.Instance.GetBindingText(GameInput.Binding.InteractAlt);

        pauseText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Pause);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void ShowPressToRebindKey()
    {
        pressToRebindKeyTransform.gameObject.SetActive(true);
    }

    private void HidePressToRebindKey()
    {
        pressToRebindKeyTransform.gameObject.SetActive(false);

    }

    private void RebindBinding(GameInput.Binding binding)
    {
        ShowPressToRebindKey();
        GameInput.Instance.RebindBinding(binding, () =>
        {
            HidePressToRebindKey();
            UpdateVisual();
        });
    }
}

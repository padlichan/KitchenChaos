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

    }

    private void Start()
    {
        GameManager.Instance.OnGameUnpaused += GameManager_OnGameUnpaused;
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

    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}

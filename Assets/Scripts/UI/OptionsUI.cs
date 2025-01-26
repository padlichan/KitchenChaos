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
        });

    }

    private void Start()
    {
        GameManager.Instance.OnGameUnpaused += GameManager_OnGameUnpaused;
        UpdateVisual();
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
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }
}

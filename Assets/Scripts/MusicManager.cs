using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    private float volume = 0.3f;
    private AudioSource audioSource;

    private const string PLAYER_PREFS_MUSIC_VOLUME = "MusicVolume";

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Debug.LogError("Multiple instances of MusicManager in scene");
        audioSource = GetComponent<AudioSource>();
        volume = PlayerPrefs.GetFloat(PLAYER_PREFS_MUSIC_VOLUME, 0.3f);
        audioSource.volume = volume;
    }
    public void ChangeVolume()
    {
        volume += 0.1f;
        if (volume > 1.01f) volume = 0f;
        audioSource.volume = volume;
        PlayerPrefs.SetFloat(PLAYER_PREFS_MUSIC_VOLUME, volume);
        PlayerPrefs.Save();
    }

    public float GetVolume() => volume;
}

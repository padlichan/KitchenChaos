using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private const string PLAYER_PREFS_MUSIC_VOLUME = "MusicVolume";
    public static MusicManager Instance { get; private set; }
    private AudioSource audioSource;
    public float MusicVolume { get; private set; } = .3f;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Debug.LogError("Multiple MusicManager instances in scene");
        audioSource = GetComponent<AudioSource>();
        MusicVolume = PlayerPrefs.GetFloat(PLAYER_PREFS_MUSIC_VOLUME, .3f);
        audioSource.volume = MusicVolume;
    }
    public void ChangeMusicVolume()
    {
        MusicVolume += 0.1f;
        if (Mathf.Floor(MusicVolume) >= 1) MusicVolume = 0f;
        audioSource.volume = MusicVolume;
        PlayerPrefs.SetFloat(PLAYER_PREFS_MUSIC_VOLUME, MusicVolume);
        PlayerPrefs.Save();
    }
}

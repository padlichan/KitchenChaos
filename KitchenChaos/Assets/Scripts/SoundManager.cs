using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private const string PLAYER_PREFS_SOUND_EFFECTS_VOLUME = "SoundEffectsVolume";
    [SerializeField] private AudioClipRef audioClipRef;
    public static SoundManager Instance { get; private set; }
    public float SoundEffectsVolume { get; private set; } = 1f;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Debug.LogError("Multiple SoundManager instances in scene");
        SoundEffectsVolume = PlayerPrefs.GetFloat(PLAYER_PREFS_SOUND_EFFECTS_VOLUME, 1f);

    }
    private void Start()
    {
        DeliveryManager.Instance.OnOrderDelivered += DeliveryManager_OnOrderDelivered;
        DeliveryManager.Instance.OnWrongOrderDelivered += DeliveryManager_OnWrongOrderDelivered;
        CuttingCounter.OnAnyCut += CuttingCounter_OnAnyCut;
        Player.Instance.OnObjectPickup += Player_OnPickup;
        BaseCounter.OnAnyObjectDropped += BaseCounter_OnDrop;
        TrashCounter.OnAnyObjectTrashed += TrashCounter_OnTrash;
        PlateKitchenObject.OnAnyIngredientAdded += PlateKitchenObject_OnAnyIngredientAdded;
    }

    private void DeliveryManager_OnOrderDelivered(object sender, EventArgs e)
    {
        PlaySound(audioClipRef.deliverySuccess, DeliveryManager.Instance.transform.position);
    }
    private void DeliveryManager_OnWrongOrderDelivered(object sender, EventArgs e)
    {
        PlaySound(audioClipRef.deliveryFail, DeliveryManager.Instance.transform.position);
    }

    private void CuttingCounter_OnAnyCut(object sender, EventArgs e)
    {
        CuttingCounter cuttingCounter = sender as CuttingCounter;
        PlaySound(audioClipRef.chop, cuttingCounter.transform.position);
    }
    private void Player_OnPickup(object sender, EventArgs e)
    {
        Player player = sender as Player;
        PlaySound(audioClipRef.objectPickup, player.transform.position);
    }

    private void BaseCounter_OnDrop(object sender, EventArgs e)
    {
        BaseCounter baseCounter = sender as BaseCounter;
        PlaySound(audioClipRef.objectDrop, baseCounter.transform.position);
    }
    private void TrashCounter_OnTrash(object sender, EventArgs e)
    {
        TrashCounter trashCounter = sender as TrashCounter;
        PlaySound(audioClipRef.trash, trashCounter.transform.position);
    }

    private void PlateKitchenObject_OnAnyIngredientAdded(object sender, EventArgs e)
    {
        PlateKitchenObject plateKitchenObject = sender as PlateKitchenObject;
        PlaySound(audioClipRef.objectDrop, plateKitchenObject.transform.position);
    }

    public void PlayFootstepSound(Vector3 position, float volume = 1f)
    {
        PlaySound(audioClipRef.footStep, position, volume);
    }
    private void PlaySound(AudioClip audioClip, Vector3 position, float volumeMultiplier = 1f)
    {
        AudioSource.PlayClipAtPoint(audioClip, position, volumeMultiplier * SoundEffectsVolume);
    }

    private void PlaySound(AudioClip[] audioClipArray, Vector3 position, float volume = 1f)
    {
        int i = UnityEngine.Random.Range(0, audioClipArray.Length);
        PlaySound(audioClipArray[i], position, volume);
    }

    public void ChangeSoundEffectsVolume()
    {
        SoundEffectsVolume += 0.1f;
        if (Mathf.Floor(SoundEffectsVolume) >= 1) SoundEffectsVolume = 0f;
        PlayerPrefs.SetFloat(PLAYER_PREFS_SOUND_EFFECTS_VOLUME, SoundEffectsVolume);
        PlayerPrefs.Save();
    }

}

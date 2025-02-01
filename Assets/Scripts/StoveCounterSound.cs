using UnityEngine;

public class StoveCounterSound : MonoBehaviour
{
    [SerializeField] private StoveCounter stoveCounter;
    private AudioSource audioSource;
    private float warningSoundTimer;
    private float warningSoundTimerMax = 0.2f;
    private bool playWarningSound = false;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

    }
    private void Start()
    {
        stoveCounter.OnStateChanged += StoveCounter_OnStateChanged;
        stoveCounter.OnProgressChanged += StoveCounter_OnProgressChanged;
    }

    private void Update()
    {
        if (playWarningSound)
        {
            warningSoundTimer += Time.deltaTime;
            Debug.Log(warningSoundTimer);
            if (warningSoundTimer > warningSoundTimerMax)
            {
                warningSoundTimer = 0;
                SoundManager.Instance.PlayStoveBurnWarningSound(transform.position);
            }
        }
        else warningSoundTimer = 0;
    }

    private void StoveCounter_OnProgressChanged(object sender, IHasProgress.OnProgresschangeEventArgs e)
    {
        float burnShowProgressAmount = 0.5f;
        playWarningSound = stoveCounter.IsFried() && e.progressNormalized > burnShowProgressAmount;
    }

    private void StoveCounter_OnStateChanged(object sender, StoveCounter.OnStateChangedEventArgs e)
    {
        if (e.state == StoveState.Frying || e.state == StoveState.Fried)
        {
            audioSource.Play();
        }
        else
        {
            audioSource.Pause();
        }
    }
}

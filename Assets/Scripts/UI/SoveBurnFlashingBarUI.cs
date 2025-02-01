using UnityEngine;

public class SoveBurnFlashingBarUI : MonoBehaviour
{
    [SerializeField] private StoveCounter stoveCounter;
    private Animator animator;
    private const string IS_FLASHING = "IsFlashing";

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void Start()
    {
        stoveCounter.OnProgressChanged += StoveCounter_OnProgressChange;
        animator.SetBool(IS_FLASHING, false);

    }

    private void StoveCounter_OnProgressChange(object sender, IHasProgress.OnProgresschangeEventArgs e)
    {
        float burnShowProgressAmount = 0.5f;
        bool show = stoveCounter.IsFried() && e.progressNormalized > burnShowProgressAmount;
        animator.SetBool(IS_FLASHING, show);
    }
}

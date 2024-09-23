using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerCounterAnimator : MonoBehaviour
{
    private Animator animator;
    private const string OPEN_CLOSE = "OpenClose";

    [SerializeField] private ContainerCounter containerCounter;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        containerCounter.OnItemGrabbedFromContainer += ContainerCounter_OnItemGrabbedFromContainer;
    }

    private void ContainerCounter_OnItemGrabbedFromContainer(object sender, System.EventArgs e)
    {
        PlayOpenCloseAnimation();
    }

    private void PlayOpenCloseAnimation()
    {
        animator.SetTrigger(OPEN_CLOSE);
    }
}

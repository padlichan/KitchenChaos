using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private Animator playerAnimator;
    private const string IS_WALKING = "IsWalking";
    [SerializeField] private Player player;

    private void Awake()
    {
        playerAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        playerAnimator.SetBool(IS_WALKING, player.IsWalking());
    }
}

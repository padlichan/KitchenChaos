using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    private Player player;
    private float footstepTimer;
    private float footstepTimerMax = 0.125f;

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    private void Update()
    {
        footstepTimer += Time.deltaTime;
        if (footstepTimer > footstepTimerMax)
        {
            footstepTimer = 0;
            if (player.IsWalking)
            {
                float volume = 1f;
                SoundManager.Instance.PlayFootstepSound(transform.position, volume);
            }
        }
    }


}

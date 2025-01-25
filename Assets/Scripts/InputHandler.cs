using System;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public event EventHandler OnInteractAction;
    public event EventHandler OnInteractAlternateAction;
    public event EventHandler OnPause;

    public static InputHandler Instance { get; private set; }

    private PlayerInputActions playerInputActions;
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Debug.LogError("Multiple instances of InputHandler in scene");

        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();

        playerInputActions.Player.Interact.performed += playerInputActions_Interact_performed;
        playerInputActions.Player.InteractAlternate.performed += playerInputActions_InteractAlternate_performed;
        playerInputActions.Player.Pause.performed += playerInputActions_Pause_performed;
    }

    private void OnDestroy()
    {
        playerInputActions.Player.Interact.performed -= playerInputActions_Interact_performed;
        playerInputActions.Player.InteractAlternate.performed -= playerInputActions_InteractAlternate_performed;
        playerInputActions.Player.Pause.performed -= playerInputActions_Pause_performed;

        playerInputActions.Dispose();
    }

    private void playerInputActions_Pause_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnPause?.Invoke(this, EventArgs.Empty);
    }

    private void playerInputActions_InteractAlternate_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteractAlternateAction?.Invoke(this, EventArgs.Empty);
    }

    private void playerInputActions_Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteractAction?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetMovementVectorNormalized()
    {
        Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();

        return inputVector;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour, IKitchenObjectParent
{

    public static Player Instance { get; private set; }

    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float rotationSpeed = 15f;

    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask countersLayerMask;

    private BaseCounter selectedCounter;

    private Vector3 lastInteractionDir;
    private bool isWalking;

    //Kithcen objects

    [SerializeField] KitchenObjectSO kitchenObjectSO;
    private KitchenObject kitchenObject;
    [SerializeField] Transform kitchenObjectFollowTransform;

    //Events

    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public BaseCounter selectedCounter;
    }

    public event EventHandler OnObjectPickup;

    private void Awake()
    {
        if (Instance != null) { Debug.Log("Error: There are more than one player instances"); }
        Instance = this;
    }

    private void Start()
    {
        gameInput.OnInteractAction += GameInput_OnInteractAction;
        gameInput.OnInteractAlternateAction += GameInput_OnInteractAlternateAction;
    }

    private void GameInput_OnInteractAlternateAction(object sender, EventArgs e)
    {
        if (!GameManager.Instance.IsGamePlaying()) return;
        selectedCounter?.InteractAlternate(this);
    }

    private void GameInput_OnInteractAction(object sender, System.EventArgs e)
    {
        if (!GameManager.Instance.IsGamePlaying()) return;
        selectedCounter?.Interact(this);
    }

    private void Update()
    {
        HandlePlayerMovement();
        HandlePlayerInteraction();

    }


    private void HandlePlayerMovement()
    {
        float moveDistance = moveSpeed * Time.deltaTime;
        float playerRadius = 0.85f;
        float playerHight = 2f;

        Vector2 inputVector = gameInput.GetInputVectorNormalized();

        Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y);
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + transform.up * playerHight, playerRadius, moveDir, moveDistance);

        if (!canMove)
        {
            //So the character still turns even if they can't move
            transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotationSpeed);

            //Attempt to move only in x
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
            canMove = !Physics.CapsuleCast(transform.position, transform.position + transform.up * playerHight, playerRadius, moveDirX, moveDistance);

            if (canMove) moveDir = moveDirX;

            else
            {
                //Cannot move in x
                //Attempt to move only in z

                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
                canMove = !Physics.CapsuleCast(transform.position, transform.position + transform.up * playerHight, playerRadius, moveDirZ, moveDistance);

                if (canMove) moveDir = moveDirZ;
                else
                {
                    //Cannot move at all
                }

            }

        }

        if (canMove)
        {
            transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotationSpeed);
            transform.position += moveDir * moveDistance;
        }

        isWalking = moveDir != Vector3.zero;
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotationSpeed);

    }

    private void HandlePlayerInteraction()
    {

        Vector2 inputVector = gameInput.GetInputVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y);

        if (moveDir != Vector3.zero) { lastInteractionDir = moveDir; }

        float interactionDistance = 2f;
        if (Physics.Raycast(transform.position, lastInteractionDir, out RaycastHit raycastHit, interactionDistance, countersLayerMask))
        {
            if (raycastHit.transform.TryGetComponent(out BaseCounter baseCounter))
            {
                if (selectedCounter != baseCounter)
                {
                    SetSelectedCounter(baseCounter);
                }

            }
            else SetSelectedCounter(null);
        }
        else SetSelectedCounter(null);

        //Debug.Log(selectedCounter);
    }
    public bool IsWalking()
    {
        return isWalking;
    }

    private void SetSelectedCounter(BaseCounter baseCounter)
    {
        this.selectedCounter = baseCounter;
        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs { selectedCounter = baseCounter });
    }

    //Kitchen object interace stuff

    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;
        if (this.kitchenObject != null)
        {
            OnObjectPickup?.Invoke(this, EventArgs.Empty);
        }
    }

    public void ClearKitchenObject()
    {
        kitchenObject = null;
    }

    public bool HasKitchenObject()
    {
        return kitchenObject != null;
    }

    public Transform GetKitchenObjectFollowTransform()
    {
        return kitchenObjectFollowTransform;
    }
}

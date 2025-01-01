using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float turnSpeed;
    [SerializeField] private InputHandler inputHandler;
    [SerializeField] private LayerMask countersLayerMask;

    private ClearCounter selectedCounter;
    private Vector3 lastInteractDir;

    public bool IsWalking { get; private set; }
    public static Player Instance { get; private set; }

    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;

    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public ClearCounter selectedCounter;
    }


    private void Awake()
    {
        if (Instance != null) Debug.LogError("Multiple Player instances in scene."); 
        Instance = this;
    }

    private void Start()
    {
        inputHandler.OnInteractAction += InputHandler_OnInteractAction;    
    }

    private void InputHandler_OnInteractAction(object sender, EventArgs e)
    {
        if (selectedCounter != null) selectedCounter.Interact();
    }

    private void Update()
    {
        HandleMovement();
        SelectCounter();
    }

    private void SelectCounter()
    {
        Vector2 inputVector = inputHandler.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y);

        if(moveDir !=  Vector3.zero) lastInteractDir = moveDir;

        float interactDistance = 2f;
        if (Physics.Raycast(transform.position, lastInteractDir, out RaycastHit hit, interactDistance, countersLayerMask))
        {
            if (hit.transform.TryGetComponent(out ClearCounter counter))
            {
                if (counter != selectedCounter)
                {
                    SetSelectedCounter(counter);
                }
            }
            else if (selectedCounter != null) SetSelectedCounter(null);
        }
        else if (selectedCounter != null) SetSelectedCounter(null);
    }

    private void HandleMovement()
    {

        Vector2 inputVector = inputHandler.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y);
        IsWalking = moveDir != Vector3.zero;

        transform.forward = Vector3.Slerp(transform.forward, moveDir, turnSpeed * Time.deltaTime);

        float playerRadius = .7f;
        float playerHight = 2f;
        float moveDistance = speed * Time.deltaTime;
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHight, playerRadius, moveDir, moveDistance);
        if (!canMove)
        {
            //Attempt only x movement
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
            canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHight, playerRadius, moveDirX, moveDistance);
            if (canMove) moveDir = moveDirX;
            else
            {
                //Attempt only  z movement
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
                canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHight, playerRadius, moveDirZ, moveDistance);
                if (canMove) moveDir = moveDirZ;
            }
        }

        if (canMove) transform.position += moveDistance * moveDir;
    }

    private void SetSelectedCounter(ClearCounter selectedCounter)
    {
        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs { selectedCounter = selectedCounter });
        this.selectedCounter = selectedCounter;
    }
}

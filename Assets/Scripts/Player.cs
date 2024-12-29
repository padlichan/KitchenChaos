using System;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float turnSpeed;
    [SerializeField] private InputHandler inputHandler;
    public bool IsWalking { get; private set; }
    
    private void Update()
    {
        float playerRadius = .7f;
        float playerHight = 2f;
        float moveDistance = speed * Time.deltaTime;

        Vector2 inputVector = inputHandler.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y);
        IsWalking = moveDir != Vector3.zero;

        transform.forward = Vector3.Slerp(transform.forward, moveDir, turnSpeed * Time.deltaTime);

        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHight, playerRadius, moveDir, moveDistance);
        if(!canMove)
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
                if(canMove) moveDir = moveDirZ;
            }   
        }

        if(canMove) transform.position += moveDistance * moveDir;
    }
}

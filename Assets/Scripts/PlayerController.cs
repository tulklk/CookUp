using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private GameInput gameInput; 

    private Vector3 movement;
    private bool isWalking;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        HandleMovement();
        HandleInteraction();
    }
    private void HandleInteraction()
    {
        movement = gameInput.GetMovementVector();
        Vector3 moveDir = movement.normalized;
        float interactionDistance = 2f;
        Physics.Raycast(transform.position, moveDir,out RaycastHit raycastHit ,interactionDistance);
    }
    private void HandleMovement()
    {
        movement = gameInput.GetMovementVector();
        isWalking = movement.magnitude > 0.1f;
        Vector3 moveDir = movement.normalized;
        float moveDistance = moveSpeed * Time.fixedDeltaTime;


        float playerRadius = 0.5f;
        float playerHeight = 2f;


        Vector3 capsuleBottom = transform.position;
        Vector3 capsuleTop = capsuleBottom + Vector3.up * playerHeight;

        //phát hiện vật cảng bằng Raycast ( CapsuleCast ) là sẽ bắn tia laser tới phía trc, nếu có vật cản thì sẽ không di chuyển được
        bool canMove = !Physics.CapsuleCast(
            capsuleBottom,
            capsuleTop,
            playerRadius,
            moveDir,
            moveDistance
        );
        // Nếu không thể đi theo hướng chính, thử đi từng trục riêng
        if (!canMove)
        {
            // Thử theo trục X
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
            canMove = !Physics.CapsuleCast(capsuleBottom, capsuleTop, playerRadius, moveDirX, moveDistance);

            if (canMove)
            {
                moveDir = moveDirX;
            }
            else
            {
                // Thử theo trục Z
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
                canMove = !Physics.CapsuleCast(capsuleBottom, capsuleTop, playerRadius, moveDirZ, moveDistance);

                if (canMove)
                {
                    moveDir = moveDirZ;
                }
                else
                {
                    // Không đi được hướng nào
                    return;
                }
            }
        }

        // Di chuyển nếu không va chạm
        if (canMove && isWalking)
        {
            rb.MovePosition(rb.position + moveDir * moveDistance);
        }

        // Quay hướng nếu đang di chuyển
        if (isWalking)
        {
            Quaternion toRotation = Quaternion.LookRotation(moveDir, Vector3.up);
            rb.rotation = Quaternion.Slerp(rb.rotation, toRotation, 0.2f);
        }
    }

    public bool IsWalking()
    {
        return isWalking; 
    }
    //Hàm di chuyển Player
    
}

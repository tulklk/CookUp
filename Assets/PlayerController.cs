using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;

    [Header("Movement Settings")]
    public float speed = 5f;

    [Header("Joystick Reference")]
    public Joystick joystick;

    private Vector3 movement;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // Lấy trục joystick: lên-xuống điều khiển trục X, trái-phải điều khiển trục Z
        float moveX = joystick.Vertical;    // Đi tới/lùi
        float moveZ = -joystick.Horizontal;  // Trái/phải

        // Vector hướng di chuyển
        movement = new Vector3(moveX, 0f, moveZ);

        // Nếu có di chuyển thì quay Player theo hướng đó
        if (movement.magnitude > 0.1f)
        {
            Quaternion toRotation = Quaternion.LookRotation(movement, Vector3.up);
            rb.rotation = Quaternion.Slerp(rb.rotation, toRotation, 0.2f);
        }

        // Di chuyển Player
        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
    }
}

using UnityEngine;

public class GameInput : MonoBehaviour
{
    [Header("Joystick Reference")]
    public Joystick joystick;

    // Lấy vector hướng di chuyển từ joystick
    public Vector3 GetMovementVector()
    {
        float moveX = joystick.Vertical;     // Tiến/lùi
        float moveZ = -joystick.Horizontal;  // Trái/phải

        return new Vector3(moveX, 0f, moveZ);
    }
}

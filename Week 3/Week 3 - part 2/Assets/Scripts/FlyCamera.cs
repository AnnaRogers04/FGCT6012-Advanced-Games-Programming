using UnityEngine;
using UnityEngine.InputSystem;

public class FlyCamera : MonoBehaviour
{
    public float moveSpeed = 20f;
    public float fastMultiplier = 3f;
    public float lookSensitivity = 0.15f;

    float yaw, pitch;

    void Start()
    {
        Vector3 e = transform.eulerAngles;
        yaw = e.y;
        pitch = e.x;
    }

    void Update()
    {
        Keyboard kb = Keyboard.current;
        Mouse mouse = Mouse.current;
        if (kb == null || mouse == null) return;

        // Look around only while the right mouse button is held.
        if (mouse.rightButton.isPressed)
        {
            Vector2 delta = mouse.delta.ReadValue();
            yaw += delta.x * lookSensitivity;
            pitch -= delta.y * lookSensitivity;
            pitch = Mathf.Clamp(pitch, -89f, 89f);
            transform.rotation = Quaternion.Euler(pitch, yaw, 0f);
        }

        float speed = moveSpeed * (kb.leftShiftKey.isPressed ? fastMultiplier : 1f);

        Vector3 dir = Vector3.zero;
        if (kb.wKey.isPressed) dir += transform.forward;
        if (kb.sKey.isPressed) dir -= transform.forward;
        if (kb.dKey.isPressed) dir += transform.right;
        if (kb.aKey.isPressed) dir -= transform.right;
        if (kb.eKey.isPressed) dir += Vector3.up;
        if (kb.qKey.isPressed) dir -= Vector3.up;

        transform.position += dir.normalized * speed * Time.deltaTime;
    }
}
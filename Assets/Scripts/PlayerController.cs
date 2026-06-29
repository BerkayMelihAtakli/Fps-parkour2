using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 7f;
    public float jumpForce = 22f;
    public float gravity = -38f;
    public float mouseSensitivity = 2f;
    public Transform cameraHolder;

    CharacterController cc;
    Vector3 velocity;
    float xRotation;

    void Start()
    {
        cc = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.IsGameOver) return;

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;
        xRotation = Mathf.Clamp(xRotation - mouseY, -85f, 85f);
        cameraHolder.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);

        bool isGrounded = IsGrounded();

        if (isGrounded && velocity.y < 0f) velocity.y = -4f;

        if (Input.GetButtonDown("Jump") && isGrounded) velocity.y = jumpForce;

        Vector3 move = transform.right * Input.GetAxis("Horizontal") + transform.forward * Input.GetAxis("Vertical");
        cc.Move(move * moveSpeed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;
        cc.Move(velocity * Time.deltaTime);

        if (transform.position.y < -10f) GameManager.Instance?.RespawnPlayer();
    }

    bool IsGrounded()
    {
        Vector3 origin = transform.position + Vector3.up * 0.1f;
        return Physics.Raycast(origin, Vector3.down, 0.25f);
    }

    public void TeleportTo(Vector3 pos)
    {
        cc.enabled = false;
        transform.position = pos;
        velocity = Vector3.zero;
        cc.enabled = true;
    }
}

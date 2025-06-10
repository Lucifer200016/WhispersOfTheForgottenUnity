using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 7f;
    public Transform cam;

    private Rigidbody rb;
    private Vector2 moveInput;
    private bool jumpInput;

    private PlayerInputActions inputActions;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        if (cam == null)
            cam = Camera.main.transform;

        inputActions = new PlayerInputActions();
    }

    void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Move.canceled += ctx => moveInput = Vector2.zero;
        inputActions.Player.Jump.performed += ctx => jumpInput = true;
    }

    void OnDisable()
    {
        inputActions.Player.Disable();
    }

    void FixedUpdate()
    {
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);
        move = cam.forward * move.z + cam.right * move.x;
        move.y = 0f;
        rb.MovePosition(rb.position + move * moveSpeed * Time.fixedDeltaTime);

        if (jumpInput && Mathf.Abs(rb.linearVelocity.y) < 0.1f)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        jumpInput = false;
    }
}

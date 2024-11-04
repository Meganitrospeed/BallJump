using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public float speed = 10f;
    public float jumpForce = 5f;
    private Rigidbody rb;
    private Vector2 moveInput;
    private bool isGrounded;
    private InputActions inputActions;
    private int count;
    public TextMeshProUGUI countText;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        inputActions = new InputActions();
        count = 0;
        SetCountText();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PickUp"))
        {
            other.gameObject.SetActive(false);
            count = count + 1;
            SetCountText();
        }
    }

    void SetCountText()
    {
        countText.text = "Count: " + count.ToString();
    }

    void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Move.performed += HandleMove;
        inputActions.Player.Move.canceled += HandleMove;
        inputActions.Player.Jump.performed += HandleJump;
        inputActions.Player.Escape.performed += HandleEscape;
    }

    void OnDisable()
    {
        inputActions.Player.Move.performed -= HandleMove;
        inputActions.Player.Move.canceled -= HandleMove;
        inputActions.Player.Jump.performed -= HandleJump;
        inputActions.Player.Escape.performed -= HandleEscape;
        inputActions.Player.Disable();
    }

    void Update()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.1f);
    }

    void FixedUpdate()
    {
        Vector3 movement = new Vector3(moveInput.x, 0, moveInput.y) * speed;
        rb.AddForce(movement);
    }

    private void HandleMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        Debug.Log($"HandleMove called with moveInput: {moveInput}");
    }

    private void HandleJump(InputAction.CallbackContext context)
    {
        if (context.performed && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            Debug.Log("HandleJump called and jump force applied");
        }
    }

    private void HandleEscape(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            SceneManager.LoadScene("Level Picker");
        }
    }
}
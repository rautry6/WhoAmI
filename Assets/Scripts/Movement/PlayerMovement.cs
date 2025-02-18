using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    InputSystem_Actions input;

    // movement
    Rigidbody2D rb;
    Vector2 velocity;
    float moveSpeed = 5f;

    private void Awake()
    {
        input = new InputSystem_Actions();

        input.Player.Move.performed += inp => Move(inp.ReadValue<Vector2>());
        input.Player.Move.canceled += inp => StopMoving();

        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        input.Player.Move.Enable();
    }

    private void OnDisable()
    {
        input.Player.Move.Disable();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Move(Vector2 direction)
    {
        velocity = new Vector2(direction.x * moveSpeed, direction.y * moveSpeed);

        rb.linearVelocity = velocity;
    }

    private void StopMoving()
    {
        rb.linearVelocity = Vector2.zero;
    }
}

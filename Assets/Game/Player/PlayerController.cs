using UnityEngine;
using UnityEngine.InputSystem;

// Ensure the component is present on the gameobject the script is attached to
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    // Local rigidbody variable to hold a reference to the attached Rigidbody2D component
    new Rigidbody2D rigidbody2D;

    public PlayerInput _playerInput;
    public Animator _anim;
    public float movementSpeed = 1000.0f;

    private InputAction _moveAction;

    void Awake()
    {
        // Setup Rigidbody for frictionless top down movement and dynamic collision
        rigidbody2D = GetComponent<Rigidbody2D>();

        rigidbody2D.isKinematic = false;
        rigidbody2D.angularDrag = 0.0f;
        rigidbody2D.gravityScale = 0.0f;
    }
    void Start()
    {
        _moveAction = _playerInput.actions["Move"];
    }

    void Update()
    {
        // Handle user input
        Vector2 targetVelocity = _moveAction.ReadValue<Vector2>(); ;

        Move(targetVelocity);
    }

    void Move(Vector2 targetVelocity)
    {        
        // Set rigidbody velocity
        rigidbody2D.velocity = (targetVelocity * movementSpeed) * Time.deltaTime; // Multiply the target by deltaTime to make movement speed consistent across different framerates
        _anim.SetFloat("velocity_x", targetVelocity.x);
        _anim.SetFloat("velocity_y", targetVelocity.y);
    }
}

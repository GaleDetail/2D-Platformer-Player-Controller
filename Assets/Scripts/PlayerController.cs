using UnityEngine;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    [FormerlySerializedAs("_speed")] [SerializeField]
    private float _moveSpeed;

    [SerializeField] private float _jumpForce;

    private Animator _animator;
    private bool _isLeft;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _moveSpeed = 5;
        _jumpForce = 20;
    }

    // Update is called once per frame
    private void Update()
    {
        var movementDirection = Input.GetAxisRaw("Horizontal");
        if (ShouldFlip(movementDirection)) Flip();
        if (movementDirection != 0)
            _animator.SetTrigger("walk");
        else _animator.SetTrigger("idle");
    }

    private void FixedUpdate()
    {
        Move();
        Jump();
    }

    private void Move()
    {
        var movementDirection = Input.GetAxisRaw("Horizontal");

        var hit = Physics2D.Raycast(transform.position, Vector2.down, 0.1f);
        if (hit && movementDirection != 0)
            rb.velocity = new Vector2(movementDirection * _moveSpeed, 0);
    }

    private void Jump()
    {
        if (Input.GetButtonDown("Jump")) rb.velocity = new Vector2(rb.velocity.x, _jumpForce);
    }

    private bool ShouldFlip(float horizontalMovement)
    {
        return (horizontalMovement > 0 && _isLeft) || (horizontalMovement < 0 && !_isLeft);
    }

    private void Flip()
    {
        transform.Rotate(0, 180, 0);
        _isLeft = !_isLeft;
    }
}
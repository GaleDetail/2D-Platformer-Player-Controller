using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;

    public LayerMask whatIsGround;
    public Transform groundCheck;
    [SerializeField] private float groundCheckRadius;

    private Animator _animator;
    private Rigidbody2D _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        moveSpeed = 5;
        jumpForce = 20;
        groundCheckRadius = 0.2f;
    }

    private void Update()
    {
        var movementDirection = Input.GetAxisRaw("Horizontal");
        if (ShouldFlip(movementDirection)) Flip();
        if (movementDirection != 0 && IsGrounded())
            _animator.SetTrigger("walk");
        else _animator.SetTrigger("idle");

        Jump(); 
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }

    private void Move()
    {
        var movementDirection = Input.GetAxisRaw("Horizontal");

        if (movementDirection != 0)
            _rb.velocity = new Vector2(movementDirection * moveSpeed, _rb.velocity.y);
    }

    private void Jump()
    {
        if (Input.GetButtonDown("Jump") && IsGrounded())
            _rb.velocity = new Vector2(_rb.velocity.x, jumpForce);
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
    }

    private bool ShouldFlip(float horizontalMovement)
    {
        return (horizontalMovement > 0 && IsFacingLeft()) || (horizontalMovement < 0 && !IsFacingLeft());
    }
    private bool IsFacingLeft()
    {
        return transform.localScale.x < 0; 
    }

    private void Flip()
    {
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }
}
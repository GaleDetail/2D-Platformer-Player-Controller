using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private const float GroundFriction = 0.2f;
    private const float WallFriction = 0;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;


    public LayerMask whatIsGround;
    public Transform groundCheck;
    public Transform wallCheckUp;
    public Transform wallCheckDown;

    [SerializeField] private float groundCheckRadius;

    private Animator _animator;
    private Rigidbody2D _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        moveSpeed = 3;
        jumpForce = 20;
        groundCheckRadius = 0.2f;
    }

    private void Update()
    {
        ChangeAnimation();
        ChangeFriction();
        Jump();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        Gizmos.DrawLine(wallCheckUp.position,
            new Vector3(wallCheckUp.position.x + 0.5f, wallCheckUp.position.y, wallCheckUp.position.z));
    }

    private void ChangeAnimation()
    {
        var movementDirection = Input.GetAxisRaw("Horizontal");

        if (ShouldFlip(movementDirection)) Flip();

        if (IsFacingTheWall() && _rb.velocity.y < 0)
            _animator.SetTrigger("wallSlide");
        else if (movementDirection != 0 && IsGrounded())
            _animator.SetTrigger("walk");
        else if (IsGrounded())
            _animator.SetTrigger("idle");
        else
            _animator.SetTrigger("jump");
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

    private void ChangeFriction()
    {
        var material = new PhysicsMaterial2D();
        material.friction = GroundFriction;
        if (IsFacingTheWall()) material.friction = WallFriction;
        _rb.sharedMaterial = material;
    }

    private bool IsFacingTheWall()
    {
        Vector2 startPos = wallCheckUp.position;
        // FlipRayCast(startPos);
        var distance = 0.4f;
        var endPos = IsFacingLeft() ? startPos + Vector2.left : startPos + Vector2.right;
        var hit = Physics2D.Raycast(startPos, endPos, distance, whatIsGround);
    
        Vector2 startPos2 = wallCheckDown.position;
    
        var endPos2 = IsFacingLeft() ? startPos2 + Vector2.left : startPos2 + Vector2.right;
        var hit2 = Physics2D.Raycast(startPos2, endPos2, distance, whatIsGround);
    
        return hit.collider != null || hit2.collider != null;
    }

    // private bool IsFacingTheWall()
    // {
    //     return HittedTheWall(wallCheckUp.position) || HittedTheWall(wallCheckDown.position);
    // }

    private bool HittedTheWall(Vector2 startPosition)
    {
        var distanceToWall = 0.1f;
        var endPos = IsFacingLeft() ? startPosition + Vector2.left : startPosition + Vector2.right;
        var hit = Physics2D.Raycast(startPosition, endPos, distanceToWall, whatIsGround);
        return hit.collider != null;
    }
}
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class PlayerMovement2D : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 12f;
    [SerializeField] private float acceleration = 65f;
    [SerializeField] private float deceleration = 110f;
    [SerializeField] private float airControlMultiplier = 0.7f;
    [SerializeField] private float groundBrakeMultiplier = 2.3f;
    [SerializeField] private float stopThreshold = 0.18f;

    [Header("Jump (height-based)")]
    [Tooltip("1 = single jump, 2 = double jump")]
    [SerializeField] private int totalJumps = 2;
    [Tooltip("Desired apex height in world units")]
    [SerializeField] private float jumpHeight = 5.5f;
    [SerializeField] private float coyoteTime = 0.12f;
    [SerializeField] private float jumpBufferTime = 0.12f;

    [Header("Better Jump Gravity")]
    [SerializeField] private float baseGravityScale = 7f;
    [SerializeField] private float fallMultiplier = 2.3f;
    [SerializeField] private float lowJumpMultiplier = 2.4f;
    [SerializeField] private float maxFallSpeed = -55f;

    [Header("Ground Check (auto if left null)")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Vector2 groundCheckSize = new Vector2(0.9f, 0.1f);
    [SerializeField] private LayerMask groundLayer;

    [Header("Wall (slide / jump)")]
    [Tooltip("Layers considered 'walls'. You can set this to Ground, or a dedicated Wall layer.")]
    [SerializeField] private LayerMask wallLayer;
    [Tooltip("Horizontal cast distance to detect a wall next to you.")]
    [SerializeField] private float wallCheckDistance = 0.12f;
    [Tooltip("Portion of the collider height used to check walls (0-1).")]
    [SerializeField, Range(0.2f, 1f)] private float wallCheckHeightScale = 0.8f;
    [Tooltip("Require a mostly horizontal surface: |normal.x| must be >= this.")]
    [SerializeField, Range(0f, 1f)] private float minWallNormalX = 0.3f;
    [SerializeField] private float sideProbeWidth = 0.12f; // thickness of the side check boxes
    [SerializeField] private float sideSkin = 0.02f;       // small gap past the capsule

    [Tooltip("Max downward speed while sliding on a wall.")]
    [SerializeField] private float wallSlideSpeed = 3.0f;
    [Tooltip("Vertical height of the wall jump.")]
    [SerializeField] private float wallJumpHeight = 5.5f;
    [Tooltip("Horizontal speed applied away from the wall on wall jump.")]
    [SerializeField] private float wallJumpHorizontalSpeed = 10f;
    [Tooltip("After leaving a wall, small grace window to still wall-jump.")]
    [SerializeField] private float wallCoyoteTime = 0.12f;
    [Tooltip("After wall jumping, ignore re-grabbing wall briefly.")]
    [SerializeField] private float wallRegrabDelay = 0.12f;

    private Rigidbody2D _rb;
    private Collider2D _col;

    private float _targetX;
    private float _lastGroundedTime;
    private float _lastJumpPressedTime;

    private bool _isGrounded;
    private bool _wasGrounded;
    private int  _jumpsRemaining;
    private bool _isJumpHeld;
    
    // --- Base copies for state-based abilities --- (used for character states)
    private float _baseMoveSpeed;
    private float _baseGravityScale;
    
    // Wall state
    private bool _isOnWall;
    private bool _wasOnWall;
    private int  _wallDir;                   // -1 = wall on left, +1 = wall on right, 0 = none
    private float _lastOnWallTime;
    private float _lastWallJumpTime;
    
    private Animator _anim;
    private SpriteRenderer _sr;


    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _col = GetComponent<Collider2D>();
        
        // Save original stats so states can modify & reset them
        _baseMoveSpeed    = moveSpeed;
        _baseGravityScale = baseGravityScale;
        
        _rb.gravityScale = baseGravityScale;
        _rb.freezeRotation = true;
        _rb.linearDamping = 0f;
        _rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        _rb.sleepMode = RigidbodySleepMode2D.NeverSleep;

        _jumpsRemaining = Mathf.Max(1, totalJumps);
        
        _anim = GetComponent<Animator>();
        _sr = GetComponent<SpriteRenderer>();


        // Auto groundCheck placement/size if not set
        if (_col != null)
        {
            if (groundCheck == null)
            {
                var gc = new GameObject("GroundCheck").transform;
                gc.SetParent(transform);
                groundCheck = gc;
            }
            Vector3 min = _col.bounds.min;
            Vector3 size = _col.bounds.size;
            groundCheck.position = new Vector3(_col.bounds.center.x, min.y - 0.02f, transform.position.z);
            groundCheckSize = new Vector2(size.x * 0.95f, Mathf.Max(0.06f * size.y, 0.08f));
        }
    }

    private void Update()
    {
        // --- Ground check ---
        Vector2 boxCenter = groundCheck ? (Vector2)groundCheck.position
                                        : (Vector2)transform.position + new Vector2(0f, -0.51f);
        _isGrounded = Physics2D.OverlapBox(boxCenter, groundCheckSize, 0f, groundLayer);

        if (_isGrounded && !_wasGrounded)
            _jumpsRemaining = Mathf.Max(1, totalJumps); // reset on landing
        if (_isGrounded)
            _lastGroundedTime = Time.time;

        // --- Wall check ---
        CheckWall();

        // Reset jumps when we TOUCH a wall (edge), but only if not grounded
        if (_isOnWall && !_wasOnWall && !_isGrounded)
            _jumpsRemaining = Mathf.Max(1, totalJumps);

        if (_isOnWall)
            _lastOnWallTime = Time.time;
        
        // ---- Sprite Flip ----
        if (_targetX > 0.01f) _sr.flipX = true;
        else if (_targetX < -0.01f) _sr.flipX = false;


        // --- Buffered / coyote logic + wall logic ---
        bool buffered   = (Time.time - _lastJumpPressedTime) <= jumpBufferTime;
        bool canCoyote  = (Time.time - _lastGroundedTime)    <= coyoteTime;
        bool canWallCoy = (Time.time - _lastOnWallTime)      <= wallCoyoteTime;

        if (buffered)
        {
            // Ground or coyote jump
            if (_isGrounded || canCoyote)
            {
                RegularJump(jumpHeight);
            }
            // Wall jump (if on wall or within wall coyote)
            else if (_isOnWall || canWallCoy)
            {
                WallJump();
            }
            // Air jump(s)
            else if (_jumpsRemaining > 0)
            {
                RegularJump(jumpHeight);
            }
        }

        _wasGrounded = _isGrounded;
        _wasOnWall   = _isOnWall;
    }

    private void FixedUpdate()
    {
        // Horizontal accel/decel with extra ground braking
        float desired = _targetX * moveSpeed;
        float current = _rb.linearVelocity.x;

        float useDecel = deceleration;
        if (_isGrounded && Mathf.Abs(_targetX) < 0.01f)
            useDecel *= groundBrakeMultiplier;

        float accel = _isGrounded
            ? (Mathf.Abs(desired) > 0.01f ? acceleration : useDecel)
            : acceleration * airControlMultiplier;

        float newX = Mathf.MoveTowards(current, desired, accel * Time.fixedDeltaTime);

        if (_isGrounded && Mathf.Abs(_targetX) < 0.01f && Mathf.Abs(newX) < stopThreshold)
            newX = 0f;

        float newY = _rb.linearVelocity.y;

        // Wall slide: clamp downward speed when touching wall and not grounded
        if (_isOnWall && !_isGrounded && newY < -wallSlideSpeed)
            newY = -wallSlideSpeed;

        // Better jump gravity
        if (newY < 0f)
        {
            newY += Physics2D.gravity.y * (fallMultiplier - 1f) * _rb.gravityScale * Time.fixedDeltaTime;
        }
        else if (newY > 0f && !_isJumpHeld)
        {
            newY += Physics2D.gravity.y * (lowJumpMultiplier - 1f) * _rb.gravityScale * Time.fixedDeltaTime;
        }

        if (newY < maxFallSpeed) newY = maxFallSpeed;

        _rb.linearVelocity = new Vector2(newX, newY);
        
        // ---- Animator Parameters ----
        float speed = Mathf.Abs(_rb.linearVelocity.x);
        float yVel  = _rb.linearVelocity.y;
        bool isWallSliding = _isOnWall && !_isGrounded && yVel < 0f;

        _anim.SetFloat("Speed", speed);
        _anim.SetBool("IsGrounded", _isGrounded);
        _anim.SetFloat("YVelocity", yVel);
        _anim.SetBool("IsWallSlide", isWallSliding);
        
    }

    // ----- Jump helpers -----

    private void RegularJump(float height)
    {
        if (_jumpsRemaining <= 0) return;

        float g = Mathf.Abs(Physics2D.gravity.y) * _rb.gravityScale;
        float v = Mathf.Sqrt(2f * g * Mathf.Max(0.01f, height));
        _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, v);

        _jumpsRemaining--;
        _lastJumpPressedTime = float.NegativeInfinity; // consume buffer
    }

    private void WallJump()
    {
        // If we just wall-jumped, don't allow immediate re-grab
        _lastWallJumpTime = Time.time;

        // Determine direction to push: away from wall
        int dir = (_wallDir != 0) ? -_wallDir : (Input.GetAxisRaw("Horizontal") >= 0 ? 1 : -1);

        // Vertical component from wallJumpHeight
        float g = Mathf.Abs(Physics2D.gravity.y) * _rb.gravityScale;
        float vy = Mathf.Sqrt(2f * g * Mathf.Max(0.01f, wallJumpHeight));

        // Apply
        _rb.linearVelocity = new Vector2(dir * wallJumpHorizontalSpeed, vy);

        // Consumes one jump (we reset on wall touch already)
        if (_jumpsRemaining > 0) _jumpsRemaining--;
        _lastJumpPressedTime = float.NegativeInfinity;
        
        
    }
    
    
    // ----- Command pattern entry points -----

    public void SetMoveInput(float dir)
    {
        // clamp just to be safe
        _targetX = Mathf.Clamp(dir, -1f, 1f);
    }

    public void JumpPressed()
    {
        _lastJumpPressedTime = Time.time;
    }

    public void SetJumpHeld(bool isHeld)
    {
        _isJumpHeld = isHeld;
    }
    
    // ======= Ability / State helpers =======

    public void ResetStatsToBase()
    {
        moveSpeed      = _baseMoveSpeed;
        baseGravityScale = _baseGravityScale;

        // Make sure Rigidbody uses updated gravity
        if (_rb != null)
            _rb.gravityScale = baseGravityScale;
    }

    public void SetMoveSpeedMultiplier(float multiplier)
    {
        moveSpeed = _baseMoveSpeed * multiplier;
    }

    public void SetTotalJumps(int jumps)
    {
        totalJumps = Mathf.Max(1, jumps);
        // keep current jumpsRemaining valid
        _jumpsRemaining = Mathf.Min(_jumpsRemaining, totalJumps);
    }

    public void SetGravityScaleMultiplier(float multiplier)
    {
        baseGravityScale = _baseGravityScale * multiplier;
        if (_rb != null)
            _rb.gravityScale = baseGravityScale;
    }
    
    // ----- Detection -----

    private void CheckWall()
    {
        _isOnWall = false;
        _wallDir  = 0;

        if (_col == null) return;

        // Don’t regrab instantly after a wall jump
        if ((Time.time - _lastWallJumpTime) < wallRegrabDelay)
            return;

        Bounds b = _col.bounds;
        float h = b.size.y * Mathf.Clamp01(wallCheckHeightScale);
        Vector2 size   = new Vector2(sideProbeWidth, h);
        Vector2 leftC  = new Vector2(b.min.x - sideSkin - size.x * 0.5f, b.center.y);
        Vector2 rightC = new Vector2(b.max.x + sideSkin + size.x * 0.5f, b.center.y);

        Collider2D hitR = Physics2D.OverlapBox(rightC, size, 0f, wallLayer);
        Collider2D hitL = Physics2D.OverlapBox(leftC,  size, 0f, wallLayer);

        if (hitR != null && !hitR.isTrigger) { _isOnWall = true; _wallDir = +1; }
        if (hitL != null && !hitL.isTrigger) { _isOnWall = true; _wallDir = -1; }

        if (_isOnWall) _lastOnWallTime = Time.time;
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Vector2 boxCenter = groundCheck ? (Vector2)groundCheck.position
            : (Vector2)transform.position + new Vector2(0f, -0.51f);
        Gizmos.DrawWireCube(boxCenter, groundCheckSize);

        // side probes (wall)
        if (_col == null) _col = GetComponent<Collider2D>();
        if (_col != null)
        {
            Bounds b = _col.bounds;
            float h = b.size.y * Mathf.Clamp01(wallCheckHeightScale);
            Vector2 size   = new Vector2(sideProbeWidth, h);
            Vector2 leftC  = new Vector2(b.min.x - sideSkin - size.x * 0.5f, b.center.y);
            Vector2 rightC = new Vector2(b.max.x + sideSkin + size.x * 0.5f, b.center.y);

            Gizmos.color = Color.cyan;
            Gizmos.DrawWireCube(leftC,  size);
            Gizmos.DrawWireCube(rightC, size);
        }
    }
}

using System.Collections;
using Unity.VisualScripting;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerControl : MonoBehaviour
{
    [Header("Config Life")]
    public int life = 3;
    public float damageTimeColor = 0.06f;
    public float enableVisible = 0.4f;
    public Color hitColor;
    public Color noHitColor;

    [Header("Config Ground")]
    public float groundCheckDistance = 0.5f;
    public float groundDistanceYCollider = 1.8f;

    [Header("Config Velocity")]
    public float speed;
    public int gravityScale = 3;

    [Header("Config Jump")]
    public int numberJumps = 1;
    public int maxJump = 2;
    public float jumpForce;

    [Header("Config Ladder")]
    public int discountSpeed = 2;

    [Header("Config Slopes")]
    public float slopeCheckDistance = 1.0f;

    [Header("Config Dash")]
    public float dashingVelocity = 12f;
    public float dashingTime = 0.3f;

    [Header("Config Wall")]
    public float wallCheckDistance = 0.4f;

    [Header("Config Wall Jump")]
    public float wallSlideSpeed = 3.0f;
    public float wallJumpTime = 0.2f;
    public float wallJumpDuration = 0.4f;
    public Vector2 wallJumpPower = new Vector2(6f, 10f);

    [Header("Config Audio")]
    public AudioSource fxGame;
    public AudioClip fxJump;
    public AudioClip fxHit;
    public AudioClip fxCarrot;
    public AudioClip fxDie;
    public AudioClip fxExplosion;

    [Header("Config Die")]
    public GameObject playerDie;
    public float loadSceneTime = 3f;

    [Header("Config Dust")]
    public ParticleSystem dust;
 
    public bool invincible;

    private bool isWallJumping = false;
    private bool isWallSliding = false;
    private float wallJumpCounter;
    private float wallJumpDirection;

    private Vector2 dashingDir;
    private bool isDashing;
    private bool canDash = false;

    private bool facingRight = true;
    private bool isGround = false;
    private bool isTouchingWall = false;
    private bool jump = false;
    private bool isLadder = false;
    private bool isClimbing = false;
    private bool isOnSlope = false;
    private bool dashInput = false;

    private float moveInput = 0.0f;
    private float touchClimbing = 0.0f;
    private float slopeAngle = 0.0f;

    private Vector2 colliderSize;
    private Vector2 position;

    private Animator playerAnimator;
    private Rigidbody2D playerRb;
    private CapsuleCollider2D playerCollider;
    private ControllerGame controlGame;
    private TrailRenderer trailRenderer;
    private SpriteRenderer spriteRenderer;

    [Header("Config WallCheck")]
    [SerializeField] private Transform wallCheck;

    [Header("Config Friction")]
    [SerializeField] private PhysicsMaterial2D noFrictionMaterial;
    [SerializeField] private PhysicsMaterial2D frictionMaterial;


    void Start()
    {
        LoadComponents();
    }

    void Update()
    {
        GlobalPosition();
        DetectWall();
        DetectGround();
        DetectSlopes();
        DetectWallSliding();
        HandleInput();
        HandleAnimator();
    }

    public void SetBarLife(int newlife)
    {
        life = newlife;
        controlGame.BarLifes(life);
    }

    public void SetInvincible(float timeInvincible, Color color)
    {
        StartCoroutine(EffectColor(color, timeInvincible));
    }

    private void FixedUpdate()
    {
        MovePlayer();
        JumpPlayer();
        WallJump();
        DashPlayer();
        ClimbingPlayer();
    }

    private void LoadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    private void LoadComponents()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerAnimator = GetComponent<Animator>();
        playerRb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<CapsuleCollider2D>();
        trailRenderer = GetComponent<TrailRenderer>();
        controlGame = FindObjectOfType(typeof(ControllerGame)) as ControllerGame;
        playerRb.gravityScale = gravityScale;
        colliderSize = playerCollider.size;
    }

    private void GlobalPosition()
    {
        position = transform.position - new Vector3(0f, colliderSize.y / groundDistanceYCollider, 0f);
    }

    private void DetectGround()
    {
        isGround = Physics2D.Raycast(position, Vector3.down, groundCheckDistance, 1 << LayerMask.NameToLayer("Ground"));
    }

    private void DetectWall()
    {
        if (facingRight)
        {
            isTouchingWall = Physics2D.Raycast(wallCheck.position, Vector3.right, wallCheckDistance, 1 << LayerMask.NameToLayer("Wall"));
        }
        else
        {
            isTouchingWall = Physics2D.Raycast(wallCheck.position, Vector3.left, wallCheckDistance, 1 << LayerMask.NameToLayer("Wall"));
        }
    }

    private void DetectSlopes()
    {

        RaycastHit2D hitSlope = Physics2D.Raycast(position, Vector2.down, slopeCheckDistance, 1 << LayerMask.NameToLayer("Ground"));

        if (hitSlope)
        {
            slopeAngle = Vector2.Angle(hitSlope.normal, Vector2.up);
            isOnSlope = slopeAngle != 0;
        }

        if (isOnSlope && moveInput == 0.0f || isWallSliding)
        {
            playerRb.sharedMaterial = frictionMaterial;
        }
        else
        {
            playerRb.sharedMaterial = noFrictionMaterial;
        }
    }

    private void DetectWallSliding()
    {
        if (isTouchingWall && !isGround)
        {
            isWallSliding = true;
            playerRb.velocity = new Vector2(playerRb.velocity.x,
                Mathf.Clamp(playerRb.velocity.y, -wallSlideSpeed, float.MaxValue));
        }
        else
        {
            isWallSliding = false;
        }
    }

    private void MovePlayer()
    {
        if (!isWallJumping)
        {
            playerRb.velocity = new Vector2(moveInput * speed, playerRb.velocity.y);

            if (moveInput < 0 && facingRight || (moveInput > 0 && !facingRight))
            {
                Flip();
            }
        }
    }

    private void DashPlayer()
    {
        if (canDash)
        {
            isDashing = true;
            trailRenderer.emitting = true;
            dashingDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            dashingDir = new Vector2(transform.localScale.x, 0);

            StartCoroutine(StopDashing());
        }
        if (isDashing)
        {
            playerRb.velocity = dashingDir.normalized * dashingVelocity;
            canDash = false;
            return;
        }
    }

    private void Flip()
    {
        facingRight = !facingRight;
        transform.localScale = new
            Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }

    private void ClimbingPlayer()
    {
        if (isLadder)
        {
            touchClimbing = Input.GetAxisRaw("Vertical");
            if (touchClimbing > 0)
            {
                isClimbing = true;
            }
        }
        else
        {
            isClimbing = false;
        }

        if (isLadder && isClimbing)
        {
            playerRb.velocity = new Vector2(playerRb.velocity.x / 4, touchClimbing * (speed - discountSpeed));
            playerRb.gravityScale = 0;
        }
        else
        {
            playerRb.gravityScale = gravityScale;
        }
    }

    private void JumpPlayer()
    {
        if (jump && !isWallSliding)
        {
            if (isGround)
            {
                numberJumps = 0;
                CreateDust();
            }

            if (isGround || numberJumps < maxJump)
            {
                var force = numberJumps > 0 ? jumpForce / 2 : jumpForce;
                playerRb.AddForce(new Vector2(0f, force));
                isGround = false;
                numberJumps++;

                fxGame.PlayOneShot(fxJump);
            }

            jump = false;
        }
    }

    private void WallJump()
    {

        if (isWallSliding)
        {
            isWallJumping = false;
            wallJumpDirection = -transform.localScale.x;
            wallJumpCounter = wallJumpTime;

            CancelInvoke(nameof(StopWallJump));
        }
        else
        {
            wallJumpCounter -= Time.deltaTime;
        }

        if (jump && wallJumpCounter > 0f)
        {
            isWallJumping = true;
            numberJumps = 0;
            playerRb.velocity = new Vector2(wallJumpDirection * wallJumpPower.x, wallJumpPower.y);
            wallJumpCounter = 0f;

            if (transform.localScale.x != wallJumpDirection)
            {
                facingRight = !facingRight;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
            }

            fxGame.PlayOneShot(fxJump);

            Invoke(nameof(StopWallJump), wallJumpDuration);
            jump = false;
        }

    }

    private void HandleInput()
    {
        dashInput = Input.GetButtonDown("Dash");
        if (dashInput)
        {
            canDash = true;
        }
        moveInput = Input.GetAxisRaw("Horizontal");
        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
        }
    }

    private void Hurt()
    {
        if (!invincible)
        {
            life--;
            invincible = true;
            fxGame.PlayOneShot(fxHit);
            StartCoroutine(EffectColor(hitColor, damageTimeColor));
            controlGame.BarLifes(life);

            if (life < 1)
            {
                GameObject playerDieTemp = Instantiate(playerDie, transform.position, Quaternion.identity);
                Rigidbody2D rbDie = playerDieTemp.GetComponent<Rigidbody2D>();
                rbDie.AddForce(new Vector2(150f, 500f));
                fxGame.PlayOneShot(fxDie);
                Invoke("LoadScene", loadSceneTime);
                gameObject.SetActive(false);
            }
        }
    }

    private void HandleAnimator()
    {
        playerAnimator.SetBool("IsGrounded", isGround);
        playerAnimator.SetBool("Walk", moveInput != 0.0f && isGround);
        playerAnimator.SetBool("Climp", isClimbing && isLadder);
        playerAnimator.SetBool("ClimpStop", touchClimbing == 0 && isLadder && !isGround);
        playerAnimator.SetBool("Jump", !isGround && !isClimbing);
        playerAnimator.SetFloat("EixoY", playerRb.velocity.y);
        playerAnimator.SetBool("Dash", isDashing);
    }

    private void CreateDust()
    {
        dust.Play();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Platform":
                transform.parent = collision.transform;
                break;
            case "Enemy":
                Hurt();
                break;
            case "Damage":
                Hurt();
                break;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Platform":
                transform.parent = null;
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Coletables":
                controlGame.Points(1);
                fxGame.PlayOneShot(fxCarrot);
                Destroy(collision.gameObject);
                break;
            case "Ladder":
                isLadder = true;
                break;
            case "Enemy":
                var positionEnemy = collision.gameObject.transform.position;
                GameObject tempExplosion = Instantiate(controlGame.hitPrefab, positionEnemy, transform.localRotation);
                Destroy(tempExplosion, 0.5f);

                Rigidbody2D rb = GetComponentInParent<Rigidbody2D>();
                rb.velocity = new Vector2(rb.velocity.x, 0);
                rb.AddForce(new Vector2(0, jumpForce));

                fxGame.PlayOneShot(fxExplosion);
                Destroy(collision.gameObject);
                break;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Ladder":
                isLadder = false;
                break;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        if (facingRight)
        {
            Gizmos.DrawLine(wallCheck.position,
                new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y, wallCheck.position.z));
        }
        else
        {
            Gizmos.DrawLine(wallCheck.position,
                new Vector3(wallCheck.position.x - wallCheckDistance, wallCheck.position.y, wallCheck.position.z));
        }
    }

    private void StopWallJump()
    {
        isWallJumping = false;
    }

    private IEnumerator StopDashing()
    {
        yield return new WaitForSeconds(dashingTime);
        trailRenderer.emitting = false;
        isDashing = false;
    }

    private IEnumerator EffectColor(Color color, float time)
    {
        spriteRenderer.color = color;
        yield return new WaitForSeconds(time);

        for (float i = 0; i <= time; i += time)
        {
            spriteRenderer.enabled = false;
            yield return new WaitForSeconds(enableVisible);
            spriteRenderer.enabled = true;
            yield return new WaitForSeconds(enableVisible);
        }

        spriteRenderer.color = noHitColor;
        invincible = false;
    }

}

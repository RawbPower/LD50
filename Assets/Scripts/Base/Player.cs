using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Controller2D))]
public class Player : Entity
{
    public enum PlayerState { Grounded, Air, Dashing, WallSliding, Hurt, Eating };

    public float attackSpeed;
    public float maxJumpHeight;
    public float minJumpHeight;

    private bool isTouchingFront;
    private bool isWallJumping;

    private Vector2 moveInput;

    private bool isJumping;
    private bool isHurt;

    private Dash dash;
    private WallSlide wallSlide;

    public ParticleSystem skidParticles;

    public GameObject endScreen;

    private bool jumpDown;
    private bool jumpHeld;
    private bool jumpUp;
    private bool dashDown;
    private bool mouseDash;

    private float maxJumpSpeed;
    private float minJumpSpeed;

    public float hangTime;
    private float hangCounter;

    public float jumpBufferLength;
    private float jumpBufferCount;

    private PlayerState playerState;
    private Vector2 lastGroundLocation;
    PlayerAttackManager attack;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        playerState = PlayerState.Grounded;
        maxJumpSpeed = Mathf.Sqrt(-2 * gravity * maxJumpHeight);
        minJumpSpeed = Mathf.Sqrt(-2 * gravity * minJumpHeight);
        dash = GetComponent<Dash>();
        wallSlide = GetComponent<WallSlide>();
        attack = GetComponentInChildren<PlayerAttackManager>();
        lastGroundLocation = Vector2.zero;
        audioManager = GetComponent<AudioManager>();
        FindObjectOfType<GlobalAudioManager>().Play("DayTheme");
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = controller.collisions.below && !isFlying;
        isTouchingFront = controller.collisions.left || controller.collisions.right;
        //wallDirX = (controller.collisions.left) ? -1 : 1;

        ProcessInputs();

        // Jump Buffer
        if (jumpDown && moveInput.y > -0.9f)
        {
            jumpBufferCount = jumpBufferLength;
        }
        else if (jumpBufferCount >= 0)
        {
            jumpBufferCount -= Time.deltaTime;
        }

        // Hang Time
        if (playerState != PlayerState.Grounded)
        {
            hangCounter -= Time.deltaTime;
        }

        // Dash Cooldown
        if (playerState != PlayerState.Dashing)
        {
            if (dashDown)
            {
                playerState = PlayerState.Dashing;
                if (mouseDash)
                {
                    Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    Vector2 dashDirection = mousePos - transform.position;
                    dashDirection.Normalize();
                    dash.StartDash(dashDirection, facingRight);
                    audioManager.Play("Bat");
                }
                else
                {
                    dash.StartDash(moveInput, facingRight);
                    audioManager.Play("Bat");
                }
            }
        }

        if (isAlive)
        {
            if (playerState != PlayerState.WallSliding && playerState != PlayerState.Dashing)
            {
                if (!facingRight && moveInput.x > 0.1f)
                {
                    Flip();
                }
                else if (facingRight && moveInput.x < -0.1f)
                {
                    Flip();
                }
            }
        }

        // Set last ground position
        if (isGrounded)
        {
            lastGroundLocation = transform.position;
        }

        if (isAlive)
        {
            animationManager?.ManagePlayerAnimationState(playerState, velocity, moveInput);
        }
        UpdateAudio();
    }

    void ProcessInputs()
    {
        moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        jumpDown = Input.GetButtonDown("Jump");
        jumpHeld = Input.GetButton("Jump");
        jumpUp = Input.GetButtonUp("Jump");

        dashDown = Input.GetButtonDown("Dash") || Input.GetMouseButtonDown(1);
        mouseDash = Input.GetMouseButtonDown(1);

        // Finite State Machine for inputs
        switch (playerState)
        {
            case PlayerState.Grounded:

                // Transition to air state if not on ground
                if (!isGrounded)
                {
                    playerState = PlayerState.Air;
                }

                if (attack != null && attack.IsEating())
                {
                    playerState = PlayerState.Eating;
                }

                if (isHurt)
                {
                    playerState = PlayerState.Hurt;
                }

                hangCounter = hangTime;

                break;

            case PlayerState.Air:

                // Transition to ground state
                if (isGrounded)
                {
                    playerState = PlayerState.Grounded;
                }   // Transition to wall slide state is moving into wall
                else if (isTouchingFront && !isWallJumping && controller.collisions.canWallSlideOnCollision)
                {
                    playerState = PlayerState.WallSliding;
                }

                if (attack != null && attack.IsEating())
                {
                    playerState = PlayerState.Eating;
                }

                if (isHurt)
                {
                    playerState = PlayerState.Hurt;
                }


                break;

            case PlayerState.Dashing:

                // Transition to wall slide state is moving into wall
                bool dashingIntoWall = (controller.collisions.left && dash.GetDashDirection().x < 0) || (controller.collisions.right && dash.GetDashDirection().x > 0);
                if (dashingIntoWall && !isGrounded && controller.collisions.canWallSlideOnCollision)
                {
                    dash.EndDash();
                    playerState = PlayerState.WallSliding;
                }

                // Transition to Ground or Air state after Dash
                if (!dash.GetIsDashing())
                {
                    if (isGrounded)
                    {
                        playerState = PlayerState.Grounded;
                    }
                    else
                    {
                        playerState = PlayerState.Air;
                    }
                }

                if (attack != null && attack.IsEating())
                {
                    playerState = PlayerState.Eating;
                }

                if (isHurt)
                {
                    playerState = PlayerState.Hurt;
                }

                break;

            case PlayerState.WallSliding:

                if (!wallSlide.GetIsWallSliding())
                {
                    wallSlide.StartWallSlide(facingRight);
                }

                // Transition to Ground or Air state after Wall jump
                if (isGrounded)
                {
                    playerState = PlayerState.Grounded;
                }
                else if (!isTouchingFront)
                {
                    playerState = PlayerState.Air;
                }

                if (attack != null && attack.IsEating())
                {
                    playerState = PlayerState.Eating;
                }

                if (isHurt)
                {
                    playerState = PlayerState.Hurt;
                }

                break;

            case PlayerState.Hurt:
                
                if (!isHurt)
                {
                    playerState = PlayerState.Air;
                }

                break;

            case PlayerState.Eating:

                if (attack != null && !attack.IsEating())
                {
                    playerState = PlayerState.Grounded;
                }

                break;
        }
    }

    private void UpdateAudio()
    {
        if (audioManager == null)
        {
            return;
        }

        if (isGrounded && velocity.x != 0.0f)
        {
            if (inWater)
            {
                if (audioManager.IsPlaying("Footsteps"))
                {
                    audioManager.Stop("Footsteps");
                }

                if (!audioManager.IsPlaying("FootstepsWater"))
                {
                    audioManager.Play("FootstepsWater");
                }
            }
            else
            {
                if (audioManager.IsPlaying("FootstepsWater"))
                {
                    audioManager.Stop("FootstepsWater");
                }

                if (!audioManager.IsPlaying("Footsteps"))
                {
                    audioManager.Play("Footsteps");
                }
            }
        }
        else
        {
            if (audioManager.IsPlaying("Footsteps"))
            {
                audioManager.Stop("Footsteps");
            }

            if (audioManager.IsPlaying("FootstepsWater"))
            {
                audioManager.Stop("FootstepsWater");
            }
        }
    }

    protected override void FixedUpdate()
    {
        CalculateVelocity();

        isChangingDirection = velocity.x * moveInput.x < 0.0;

        // Add Friction
        ApplyFriction(isChangingDirection || moveInput.x == 0);

        // Add Air Drag
        ApplyAirDrag();

        // Add Gravity
        ApplyGravity();

        // Limit Velocity
        LimitVelocity();

        if (stallMovement)
        {
            velocity *= stallMovementScale;
        }

        bool fallThrough = (jumpDown || jumpHeld);
        Vector2 movementInput = Vector2.zero;
        if (fallThrough)
        {
            movementInput = moveInput;
        }
        controller.Move(velocity * Time.deltaTime, movementInput);

        if (controller.collisions.above && velocity.y > 0.0f)
        {
            velocity.y = 0;
        }

        if (controller.collisions.below && velocity.y < 0.0f)
        {
            velocity.y = 0;
        }

        ResetValues();
    }

    protected override void CalculateVelocity()
    {
        if (isAlive)
        {
            switch (playerState)
            {
                case PlayerState.Grounded:

                    if (hangCounter > 0 && jumpBufferCount >= 0)
                    {
                        isJumping = true;
                        playerState = PlayerState.Air;
                        velocity = new Vector2(velocity.x, maxJumpSpeed);
                        jumpBufferCount = 0;
                    }

                    if (isChangingDirection)
                    {
                        /*var shape = skidParticles.shape;
                        float rotation = moveInput.x > 0 ? -90.0f : 90.0f;
                        shape.rotation = new Vector3(0.0f, rotation, 0.0f);
                        skidParticles.Play();*/
                        if (instantTurn)
                        {
                            velocity = new Vector2(0, velocity.y);
                        }
                        else
                        {
                            velocity = new Vector2(velocity.x + groundDeceleration * Time.deltaTime * moveInput.x, velocity.y);
                        }
                    }
                    else
                    {
                        velocity = new Vector2(velocity.x + groundAcceleration * Time.deltaTime * moveInput.x, velocity.y);
                    }

                    break;

                case PlayerState.Air:

                    // Stop jump
                    if (jumpUp || (!jumpDown && !jumpHeld))
                    {
                        if (isJumping && velocity.y > minJumpSpeed)
                        {
                            velocity.y = minJumpSpeed;
                        }

                        isJumping = false;
                        isWallJumping = false;
                    }

                    velocity = new Vector2(velocity.x + airAcceleration * Time.deltaTime * moveInput.x, velocity.y);

                    // Air Drag
                    //velocity.x -= airDrag * velocity.x * Mathf.Abs(velocity.x);


                    break;

                case PlayerState.Dashing:

                    dash.UpdateDash();

                    break;

                case PlayerState.WallSliding:

                    if (!wallSlide.GetIsWallSliding())
                    {
                        wallSlide.StartWallSlide(facingRight);
                    }

                    wallSlide.UpdateWallSlide(moveInput, jumpBufferCount >= 0);

                    if (!wallSlide.GetIsWallSliding())
                    {
                        isWallJumping = wallSlide.GetWallJumpExit();
                        playerState = PlayerState.Air;
                    }

                    break;

                case PlayerState.Hurt:

                    velocity = Vector2.zero;
                    gravity = 0.0f;

                    break;

                case PlayerState.Eating:

                    velocity = Vector2.zero;
                    gravity = 0.0f;

                    break;
            }
        }
    }

    public override void Reset(bool resetHealth = true)
    {
        base.Reset(resetHealth);
        isHurt = false;
    }

    public PlayerState GetPlayerState()
    {
        return playerState;
    }

    public bool GetIsHurt()
    {
        return isHurt;
    }

    public void SetIsHurt(bool hurt)
    {
        isHurt = hurt;
    }

    public Vector2 GetLastGroundLocation() { return lastGroundLocation; }
}

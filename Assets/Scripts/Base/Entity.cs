using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Controller2D))]
public class Entity : MonoBehaviour
{

    protected Vector2 velocity;
    public float gravity = -15;
    public float groundAcceleration;
    public float groundDeceleration;
    public bool instantTurn;
    public bool instantStop;
    public float airAcceleration;
    public float maxXSpeedGround;
    public float maxXSpeedAir;
    public float maxYSpeed;
    public float movingFriction;
    public float stoppingFriction;
    public float stallMovementScale;

    public float airDrag;
    public bool isFlying = false;

    protected bool isChangingDirection;
    protected bool isGrounded;
    protected bool inWater;
    protected bool stallMovement;
    protected bool isAlive = true;
    protected bool isStopping;
    protected bool velocityOverride = false;

    protected float _defaultMaxXSpeedGround;
    protected float _defaultMaxXSpeedAir;
    protected float _defaultMaxYSpeed;
    protected float _defaultMovingFriction;
    protected float _defaultStoppingFriction;
    protected float _defaultGravity;

    protected bool facingRight;
    protected Controller2D controller;
    protected AnimationManager animationManager;
    protected SpriteRenderer spriteRenderer;

    [HideInInspector]
    public AudioManager audioManager;

    protected Sun sun;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        facingRight = transform.localScale.x > 0 ? true : false;
        controller = GetComponent<Controller2D>();
        animationManager = GetComponent<AnimationManager>();
        audioManager = GetComponent<AudioManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        velocity = Vector2.zero;
        stallMovement = false;
        isStopping = true;
        inWater = false;

        _defaultMaxXSpeedGround = maxXSpeedGround;
        _defaultMaxXSpeedAir = maxXSpeedAir;
        _defaultMaxYSpeed = maxYSpeed;
        _defaultMovingFriction = movingFriction;
        _defaultStoppingFriction = stoppingFriction;
        _defaultGravity = gravity;
        sun = FindObjectOfType<Sun>();
    }

    protected virtual void FixedUpdate()
    {
        if (controller.collisions.above && velocity.y > 0.0f)
        {
            velocity.y = 0;
        }

        if (controller.collisions.below && velocity.y < 0.0f)
        {
            velocity.y = 0;
        }
        CalculateVelocity();

        // Add Friction
        ApplyFriction(isStopping);

        // Add Gravity
        ApplyGravity();

        // Limit Velocity
        LimitVelocity();

        if (stallMovement)
        {
            Debug.Log(this.name + " is stalling");
            velocity *= stallMovementScale;
        }

        if (isFlying)
        {
            controller.AirMove(velocity * Time.deltaTime);
        }
        else
        {
            controller.Move(velocity * Time.deltaTime);
        }

        ResetValues();
    }

    protected virtual void LateUpdate()
    {
        if (spriteRenderer)
        {
            inWater = false;
            velocityOverride = false;
            Color tintColor = sun.GetSunColor();
            spriteRenderer.color = tintColor;
        }
    }

    protected virtual void ResetValues()
    {
        isStopping = true;
        maxXSpeedGround = _defaultMaxXSpeedGround;
        maxXSpeedAir = _defaultMaxXSpeedAir;
        maxYSpeed = Mathf.Max(_defaultMaxYSpeed, Mathf.Abs(velocity.y));
        movingFriction = _defaultMovingFriction;
        stoppingFriction = _defaultStoppingFriction;
        gravity = _defaultGravity;
}

    protected virtual void CalculateVelocity()
    {
        //velocity.y += gravity * Time.deltaTime;
    }

    protected void ApplyFriction(bool isStopping)
    {
        if (isGrounded)
        {
            if (isStopping)
            {
                float friction = stoppingFriction;

                if (instantStop)
                {
                    velocity.x = 0;
                }
                else
                {
                    velocity.x -= Mathf.Min(Mathf.Abs(velocity.x), friction) * Mathf.Sign(velocity.x);
                }
            }
            else
            {
                float friction = movingFriction;

                velocity.x -= Mathf.Min(Mathf.Abs(velocity.x), friction) * Mathf.Sign(velocity.x);
            }
        }
    }

    protected void ApplyAirDrag()
    {
        if (!isGrounded)
        {
            Vector2 drag = airDrag * Vector2.Dot(velocity, velocity) * velocity.normalized;

            // Only doing horizontal air drag for game feel
            velocity.x -= drag.x;
        }
    }

    protected void ApplyGravity()
    {
        velocity.y += gravity * Time.deltaTime;
    }

    protected void LimitVelocity()
    {
        if (isGrounded)
        {
            if (Mathf.Abs(velocity.x) > maxXSpeedGround)
            {
                velocity.x = Mathf.Sign(velocity.x) * maxXSpeedGround;
            }

            if (Mathf.Abs(velocity.y) > maxYSpeed)
            {
                velocity.y = Mathf.Sign(velocity.y) * maxYSpeed;
            }
        }
        else
        {
            if (isFlying)
            {
                float targetSpeed = maxXSpeedAir;

                if (velocity.magnitude > targetSpeed)
                {
                    velocity = velocity.normalized * targetSpeed;
                }
            }
            else
            {
                if (Mathf.Abs(velocity.x) > maxXSpeedAir)
                {
                    velocity.x = Mathf.Sign(velocity.x) * maxXSpeedAir;
                }

                if (Mathf.Abs(velocity.y) > maxYSpeed)
                {
                    velocity.y = Mathf.Sign(velocity.y) * maxYSpeed;
                }
            }
        }
    }

    public virtual void Reset(bool resetHealth = true)
    {
        gameObject.SetActive(true);
        isAlive = true;
        stallMovement = false;
    }

    public virtual void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    public Vector2 GetVelocity()
    {
        return velocity;
    }
    public void AddVelocity(Vector2 addVelocity)
    {
        velocity += addVelocity;
        LimitVelocity();
    }

    public void SetVelocity(Vector2 setVelocity)
    {
        if (velocityOverride)
        {
            return;
        }

        velocity = setVelocity;
        LimitVelocity();

        if (!IsFacingRight() && setVelocity.x > 0.0f)
        {
            Flip();
        }
        else if (IsFacingRight() && setVelocity.x < 0.0f)
        {
            Flip();
        }

        if (setVelocity != Vector2.zero)
        {
            SetIsStopping(false);
        }
    }

    public void OverrideVelocity(Vector2 setVelocity)
    {
        velocity = setVelocity;
        LimitVelocity();

        if (!IsFacingRight() && setVelocity.x > 0.0f)
        {
            Flip();
        }
        else if (IsFacingRight() && setVelocity.x < 0.0f)
        {
            Flip();
        }

        if (setVelocity != Vector2.zero)
        {
            SetIsStopping(false);
        }

        velocityOverride = true;
    }

    public bool IsFacingRight()
    {
        return facingRight;
    }

    public bool IsGrounded()
    {
        return isGrounded;
    }

    public bool GetInWater() { return inWater; }
    public void SetInWater(bool water) { inWater = water; }

    public void SetStallMovement(bool stall)
    {
        stallMovement = stall;
    }

    public bool GetStallMovement()
    {
        return stallMovement;
    }

    public Controller2D GetController()
    {
        return controller;
    }

    public bool IsAlive() { return isAlive; }

    public void SetAlive(bool alive) { isAlive = alive; }

    public void SetIsStopping(bool stopping) { isStopping = stopping; }
    public bool GetIsStopping() { return isStopping; }

    public float GetDefaultMaxXSpeedGround() { return _defaultMaxXSpeedGround; }
    public float GetDefaultMaxXSpeedAir() { return _defaultMaxXSpeedAir; }
    public float GetDefaultMaxYSpeed() { return _defaultMaxYSpeed; }
    public float GetDefaultMovingFriction() { return _defaultMovingFriction; }
    public float GetDefaultStoppingFriction() { return _defaultStoppingFriction; }
    public float GetDefaultGravity() { return _defaultGravity; }

}

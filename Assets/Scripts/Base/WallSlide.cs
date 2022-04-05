using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSlide : MonoBehaviour
{
    public Vector2 wallJumpClimb;
    public Vector2 wallJumpOff;
    public Vector2 wallLeap;
    public float wallSlideSpeed;
    public float wallStickTime;

    private int _wallDirX;
    private float _wallStickCounter;

    private bool _isWallSliding = false;
    private bool _wallJumpExit = false;

    private Entity _wallSlideEntity;
    private Controller2D _wallSlideController;
    private Vector2 _moveInput;

    private void Awake()
    {
        _wallSlideEntity = GetComponent<Entity>();
    }

    private void Update()
    {
        if (_wallSlideEntity is Player)
        {
            Player player = (Player)_wallSlideEntity;
            if (player.GetPlayerState() != Player.PlayerState.WallSliding)
            {
                EndWallSlide(true);
            }
        }

        _wallSlideController = _wallSlideEntity.GetController();

        _wallDirX = (_wallSlideController.collisions.left) ? -1 : 1;

        if (_isWallSliding)
        {
            if (_wallStickCounter > 0)
            {
                if (_moveInput.x != _wallDirX && _moveInput.x != 0)
                {
                    _wallStickCounter -= Time.deltaTime;
                }
                else
                {
                    _wallStickCounter = wallStickTime;
                }
            }
            else
            {
                _wallStickCounter = wallStickTime;
            }
        }
    }

    public void StartWallSlide(bool facingRight)
    {
        if (_wallSlideEntity)
        {
            if (_wallDirX == 1 && !facingRight)
            {
                _wallSlideEntity.Flip();
            }

            if (_wallDirX == -1 && facingRight)
            {
                _wallSlideEntity.Flip();
            }

            _isWallSliding = true;
        }
    }

    public void UpdateWallSlide(Vector2 movementInput, bool canJump)
    {
        _moveInput = movementInput;

        Vector2 velocity = _wallSlideEntity.GetVelocity();

        if (_wallStickCounter > 0 && canJump)
        {
            _wallStickCounter = 0;

            if (_wallDirX * _moveInput.x > 0.0f)
            {
                velocity = new Vector2(-_wallDirX * wallJumpClimb.x, wallJumpClimb.y);
            }
            else if (_moveInput.x == 0.0f)
            {
                velocity = new Vector2(-_wallDirX * wallJumpOff.x, wallJumpOff.y);
            }
            else
            {
                velocity = new Vector2(-_wallDirX * wallLeap.x, wallLeap.y);
            }
        }

        if (velocity.y < -wallSlideSpeed)
        {
            velocity = new Vector2(velocity.x, -wallSlideSpeed);
        }

        if (_wallStickCounter > 0)
        {
            velocity.x = 0.0f;
        }
        else if (_wallDirX != _moveInput.x)
        {
            velocity = new Vector2(velocity.x + _wallSlideEntity.airAcceleration * Time.deltaTime * _moveInput.x, velocity.y);
        }

        _wallSlideEntity.SetVelocity(velocity);
    }

    public void EndWallSlide(bool jumpExit)
    {
        _wallJumpExit = jumpExit;
        _isWallSliding = false;
    }

    public bool GetIsWallSliding()
    {
        return _isWallSliding;
    }

    public bool GetWallJumpExit()
    {
        return _wallJumpExit;
    }
}
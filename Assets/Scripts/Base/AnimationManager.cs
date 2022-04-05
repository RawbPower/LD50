using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimationManager : MonoBehaviour
{
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
    }

    public void ManagePlayerAnimationState(Player.PlayerState playerState, Vector2 velocity, Vector2 input)
    {
        // Init Anim variables
        SetBoolParameter("WallSlide", false);
        SetBoolParameter("Dash", false);
        SetBoolParameter("Jump", false);
        SetBoolParameter("Run", false);
        SetBoolParameter("Hurt", false);

        SetFloatParameter("Vertical", velocity.y);

        // Finite State Machine for animations
        switch (playerState)
        {
            case Player.PlayerState.Grounded:

                if (Mathf.Abs(input.x) > 0.0f)
                {
                    SetBoolParameter("Run", true);
                }

                break;

            case Player.PlayerState.Air:

                SetBoolParameter("Jump", true);

                break;

            case Player.PlayerState.Dashing:

                SetBoolParameter("Dash", true);

                break;

            case Player.PlayerState.WallSliding:

                SetBoolParameter("WallSlide", true);

                break;

            case Player.PlayerState.Hurt:

                SetBoolParameter("Hurt", true);

                break;
        }
    }

    public void ManageEnemyAnimationState(Vector2 velocity, bool isAlive)
    {
        SetBoolParameter("Prone", false);
        SetBoolParameter("Jump", false);
        SetBoolParameter("Run", false);

        SetFloatParameter("Vertical", velocity.y);

        Entity animatingEntity = GetComponent<Entity>();

        if (animatingEntity)
        {
            if (animatingEntity.IsAlive())
            {
                if (animatingEntity.IsGrounded())
                {
                    // Move this to animation manager and base off of FSM
                    if (Mathf.Abs(velocity.x) > 0)
                    {
                        SetBoolParameter("Run", true);
                    }
                    else
                    {
                        SetBoolParameter("Run", false);
                    }
                }
                else
                {
                    SetBoolParameter("Jump", true);
                }
            }
            else
            {
                SetBoolParameter("Prone", true);
            }
        }
    }

    public bool HasParameter(string paramName)
    {
        foreach (AnimatorControllerParameter param in _animator.parameters)
        {
            if (param.name == paramName)
                return true;
        }
        return false;
    }
    public void SetBoolParameter(string parameter, bool arg)
    {
        if (HasParameter(parameter))
        {
            _animator.SetBool(parameter, arg);
        }
    }

    public void SetFloatParameter(string parameter, float arg)
    {
        if (HasParameter(parameter))
        {
            _animator.SetFloat(parameter, arg);
        }
    }

    public void SetTrigger(string parameter)
    {
        if (HasParameter(parameter))
        {
            _animator.SetTrigger(parameter);
        }
    }

    public bool IsAnimationPlaying(string name)
    {
        return _animator.GetCurrentAnimatorStateInfo(0).IsName(name);
    }
}

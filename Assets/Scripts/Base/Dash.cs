using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : MonoBehaviour
{
    public float dashSpeed;
    public float dashDistance;
    public float dashCooldownTime;
    public ParticleSystem dashTrail;
    public float dashTrailRate;
    public BoxCollider2D dashBox;
    public LayerMask targetMask;

    private Vector2 _dashDirection;
    private float _dashCounter;
    private float _dashCooldownCounter;
    private float _dashTrailCounter;
    private bool _isDashing = false;
    private bool isTrackingEnemy;

    private Entity _dashEntity;

    private void Awake()
    {
        _dashEntity = GetComponent<Entity>();
        isTrackingEnemy = false;
    }

    private void Update()
    {
        if (dashBox != null)
        {
            float dashBoxAngle = Mathf.Atan2(Input.GetAxis("Vertical") * transform.localScale.x, Input.GetAxis("Horizontal") * transform.localScale.x);
            dashBox.transform.eulerAngles = new Vector3(0.0f, 0.0f, dashBoxAngle * Mathf.Rad2Deg);
        }

        if (_isDashing)
        {
            // Dash time
            if (_dashCounter > 0.0)
            {
                _dashCounter -= Time.deltaTime;
            }
            else
            {
                EndDash();
            }

            if (dashTrail != null)
            {
                if (_dashTrailCounter > 0.0f)
                {
                    _dashTrailCounter -= Time.deltaTime;
                }
                else
                {
                    PlayDashEffect(transform);
                    _dashTrailCounter = dashTrailRate;
                }
            }
        }
        else
        {
            if (_dashCooldownCounter > 0.0)
            {
                _dashCooldownCounter -= Time.deltaTime;
                _dashEntity.maxXSpeedAir = _dashEntity.GetDefaultMaxXSpeedAir() + (_dashCooldownCounter / dashCooldownTime) * (dashSpeed - _dashEntity.GetDefaultMaxXSpeedAir());
                _dashEntity.maxXSpeedGround = _dashEntity.GetDefaultMaxXSpeedGround() + (_dashCooldownCounter / dashCooldownTime) * (dashSpeed - _dashEntity.GetDefaultMaxXSpeedGround());
            }
            else
            {
                _dashCooldownCounter = 0.0f;
            }
        }
    }

    public void StartDash(Vector2 direction, bool facingRight)
    {
        if (_dashEntity && _dashCooldownCounter <= 0.0)
        {
            _dashDirection = direction;
            _dashCounter = dashDistance / dashSpeed;

            if (dashBox != null)
            {
                int numColliders = 10;
                Collider2D[] colliders = new Collider2D[numColliders];
                ContactFilter2D contactFilter = new ContactFilter2D();
                contactFilter.SetLayerMask(targetMask);
                contactFilter.useTriggers = true;
                int colliderCount = dashBox.OverlapCollider(contactFilter, colliders);

                if (colliderCount > 0)
                {
                    GarlicKnight garlicKnight = colliders[0].GetComponent<GarlicKnight>();

                    if (garlicKnight == null)
                    {
                        _dashDirection = colliders[0].transform.position - transform.position;
                        _dashCounter = _dashDirection.magnitude / dashSpeed;
                        _dashDirection.Normalize();
                        if (colliders[0].CompareTag("Enemy"))
                        {
                            isTrackingEnemy = true;
                        }
                    }
                }
            }

            if (!facingRight && _dashDirection.x > 0)
            {
                _dashEntity.Flip();
            }
            else if (facingRight && _dashDirection.x < 0)
            {
                _dashEntity.Flip();
            }


            _isDashing = true;

            if (dashTrail != null)
            {
                PlayDashEffect(transform);
                _dashTrailCounter = dashTrailRate;
            }

            AudioManager am = GetComponent<AudioManager>();
            if (am != null)
            {
                am.Play("Dash");
            }
        }
    }

    public void UpdateDash()
    {
        Vector2 velocity = dashSpeed * _dashDirection;

        _dashEntity.maxXSpeedAir = dashSpeed;
        _dashEntity.maxXSpeedGround = dashSpeed;

        _dashEntity.SetVelocity(velocity);
    }

    public void PlayDashEffect(Transform parent)
    {
        var main = dashTrail.main;
        main.startRotationY = parent.localScale.x == 1 ? 0.0f : Mathf.PI;
        dashTrail.Play();
    }

    public void EndDash()
    {
        _dashCounter = 0.0f;
        _dashCooldownCounter = dashCooldownTime;
        _dashDirection = new Vector2(0.0f, 0.0f);
        _isDashing = false;

        if (isTrackingEnemy)
        {
            _dashEntity.SetVelocity(new Vector2(0.0f, _dashEntity.GetVelocity().y));
        }

        isTrackingEnemy = false;
    }

    public bool GetIsDashing()
    {
        return _isDashing;
    }

    public Vector2 GetDashDirection()
    {
        return _dashDirection;
    }
}

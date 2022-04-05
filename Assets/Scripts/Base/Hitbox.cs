using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : Box
{
    public enum ColliderState{Closed, Open, Colliding}

    public LayerMask mask;
    public Color inactiveColor;
    public Color collisionOpenColor;
    public Color collidingColor;

    private IHitboxResponder _responder = null;
    private ColliderState _state;

    protected override void Awake()
    {
        base.Awake();
        gameObject.layer = 10;
    }

    private void Update()
    {

        _state = isOpen ? ColliderState.Open : ColliderState.Closed;

        if (!isOpen)
        {
            _state = ColliderState.Closed;
            _responder.resetHit();
        }

        if (_state == ColliderState.Closed) { return; }

        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, boxSize, transform.eulerAngles.z, mask);

        for (int i = 0; i < colliders.Length; i++)
        {
            Collider2D aCollider = colliders[i];

            Hurtbox hurtbox = colliders[i].GetComponent<Hurtbox>();
            if (hurtbox != null && hurtbox.isOpen)
            {
                if (aCollider.tag != gameObject.tag)
                {
                    _responder?.collisionedWith(aCollider);
                }
            }
        }

        _state = colliders.Length > 0 ? ColliderState.Colliding : ColliderState.Open;
    }

    public void useResponder(IHitboxResponder responder)
    {
        _responder = responder;
    }

    public void startCheckingCollisions()
    {
        _state = ColliderState.Open;
    }

    public void stopCheckingCollisions()
    {
        _state = ColliderState.Closed;
    }

    private void OnDrawGizmos()
    {
        if (!isOpen)
        {
            _state = ColliderState.Closed;
        }
        else
        {
            _state = ColliderState.Open;
        }

        CheckGizmoColor();

        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.localScale);

        Gizmos.DrawWireCube(Vector3.zero, new Vector3(boxSize.x, boxSize.y, 0));
    }

    private void CheckGizmoColor()
    {
        switch (_state)
        {
            case ColliderState.Closed:
                Gizmos.color = inactiveColor;
                break;
            case ColliderState.Open:
                Gizmos.color = collisionOpenColor;
                break;
            case ColliderState.Colliding:
                Gizmos.color = collidingColor;
                break;
        }
    }
}

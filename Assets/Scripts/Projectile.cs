using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Controller2D))]
public class Projectile : MonoBehaviour
{
    public float speed;
    public float gravity = -15.0f;
    public float maxYSpeed = 12.0f;
    public bool destroyOnImpact;
    public LayerMask collisionMask;
    public AnimationClip impactAnimation;
    public GameObject spawnOnImpact;

    protected Vector2 velocity;
    protected Entity _owner;
    protected Vector2 _direction = new Vector2(-1.0f, 0.0f);
    protected bool _impacted;

    protected Controller2D controller;

    public void SetOwner(Entity owner) { _owner = owner; }
    public Entity GetOwner() { return _owner; }
    public bool HasImpacted() { return _impacted; }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        _impacted = false;
        velocity = _direction*speed;
        controller = GetComponent<Controller2D>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        Collider2D myCollider = gameObject.GetComponent<Collider2D>();
        int numColliders = 10;
        Collider2D[] colliders = new Collider2D[numColliders];
        ContactFilter2D contactFilter = new ContactFilter2D();
        contactFilter.SetLayerMask(collisionMask);
        contactFilter.useTriggers = true;
        int colliderCount = myCollider.OverlapCollider(contactFilter, colliders);

        for (int i = 0; i < colliderCount; i++)
        {
            Collider2D aCollider = colliders[i];

            StartCoroutine(ProjectileImpact());
        }
    }

    protected virtual void FixedUpdate()
    {
        velocity += new Vector2(0.0f, 1.0f) * gravity * Time.deltaTime;
        if (Mathf.Abs(velocity.y) > maxYSpeed)
        {
            velocity.y = Mathf.Sign(velocity.y) * maxYSpeed;
        }
        controller.Move(velocity * Time.deltaTime);
    }

    public IEnumerator ProjectileImpact()
    {
        if (!_impacted)
        {
            _impacted = true;
            Animator animator = GetComponent<Animator>();
            Instantiate(spawnOnImpact, transform.position, transform.rotation);

            yield return new WaitForSeconds(0.1f);

            AudioSource audioSource = GetComponent<AudioSource>();
            if (audioSource != null && !audioSource.isPlaying)
            {
                audioSource.Play();
            }

            if (animator != null && impactAnimation)
            {
                animator.Play(impactAnimation.name);
                yield return new WaitForSeconds(impactAnimation.length);
            }

            if (destroyOnImpact)
            {
                Destroy(gameObject);
            }
        }
    }

    public void SetDirection(Vector2 direction)
    {
        _direction = direction.normalized;
    }
}

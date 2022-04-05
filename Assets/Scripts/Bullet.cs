using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Controller2D))]
public class Bullet : MonoBehaviour, IHitboxResponder
{
    public float bulletSpeed;
    public float bulletLifetime;
    public float damage;

    private float bulletTime;
    protected Vector2 velocity;
    private Vector2 bulletDirection;
    private bool impacted;
    protected Controller2D controller;

    public void SetDirection(Vector2 direction)
    {
        bulletDirection = direction.normalized;
    }

    // Start is called before the first frame update
    void Start()
    {
        impacted = false;
        controller = GetComponent<Controller2D>();
        bulletTime = bulletLifetime;

        Hitbox[] hitboxes = this.GetComponentsInChildren<Hitbox>();
        foreach (Hitbox hitbox in hitboxes)
        {
            hitbox.useResponder(this);
        }
    }

    private void Update()
    {
        if (bulletTime > 0.0f)
        {
            bulletTime -= Time.deltaTime;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        velocity = bulletDirection * bulletSpeed;
        controller.Move(velocity * Time.deltaTime);
    }

    public void collisionedWith(Collider2D collider)
    {
        if (!impacted)
        {
            Health health = collider.GetComponentInParent<Health>();
            if (health.GetTotalHealth() > 0.0f)
            {
                health.RemoveHealth(damage);
                health.bloodHit.transform.localScale = new Vector3(-Mathf.Sign(velocity.x), health.bloodHit.transform.localScale.y, health.bloodHit.transform.localScale.y);
                health.bloodHit.Play();
                FindObjectOfType<GlobalAudioManager>().Play("Hit");
                Destroy(this.gameObject);
            }
        }
    }

    public void resetHit()
    {
        impacted = false;
    }
}

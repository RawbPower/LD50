using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISword : MonoBehaviour, IHitboxResponder
{
    public float attackRange;
    public float cooldown;
    public float damage;

    public bool hasAura;
    public ParticleSystem aura;

    [HideInInspector]
    public bool freezeMovement = false;

    private Player target;
    private bool hasHit;
    private Entity entity;
    private float cooldownCounter;

    public void collisionedWith(Collider2D collider)
    {
        if (!hasHit)
        {
            Health health = collider.GetComponentInParent<Health>();
            if (health.GetTotalHealth() > 0.0f)
            {
                health.RemoveHealth(damage);
                health.bloodHit.transform.localScale = -transform.parent.transform.localScale;
                health.bloodHit.Play();
                FindObjectOfType<GlobalAudioManager>().Play("Hit");
                hasHit = true;
            }
        }
    }

    public void resetHit()
    {
        hasHit = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        entity = GetComponentInParent<Entity>();
        target = FindObjectOfType<Player>();
        cooldownCounter = 0.0f;
        hasHit = false;

        Hitbox[] hitboxes = this.GetComponentsInChildren<Hitbox>();
        foreach (Hitbox hitbox in hitboxes)
        {
            hitbox.useResponder(this);
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (cooldownCounter > 0.0f)
        {
            cooldownCounter -= Time.deltaTime;
        }
        else
        {
            freezeMovement = false;
        }

        if (entity.IsAlive())
        {
            Vector3 distanceToTarget = target.transform.position - entity.transform.position;
            if (distanceToTarget.x < 0.0f && entity.IsFacingRight())
            {
                entity.Flip();
            }
            else if (distanceToTarget.x > 0.0f && !entity.IsFacingRight())
            {
                entity.Flip();
            }

            if (cooldownCounter <= 0.0f && distanceToTarget.sqrMagnitude < attackRange * attackRange)
            {
                AnimationManager animationManager = entity.GetComponent<AnimationManager>();
                if (animationManager != null)
                {
                    if (hasAura)
                    {
                        float randomNumer = Random.Range(0.0f, 1.0f);
                        if (randomNumer > 0.5f)
                        {
                            animationManager.SetTrigger("Attack");
                            FindObjectOfType<GlobalAudioManager>().Play("Swing");
                        }
                        else
                        {
                            animationManager.SetTrigger("Aura");
                            FindObjectOfType<GlobalAudioManager>().Play("Garlic");
                            freezeMovement = true;
                            aura.Play();
                        }
                    }
                    else
                    {
                        animationManager.SetTrigger("Attack");
                        FindObjectOfType<GlobalAudioManager>().Play("Swing");
                    }
                    cooldownCounter = cooldown;
                }
            }
        }
    }
}

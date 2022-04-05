using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chariot : Enemy, IHitboxResponder
{
    public float maxDistance;
    public GameObject winScreen;
    private Player target;
    public void collisionedWith(Collider2D collider)
    {
         Health health = collider.GetComponentInParent<Health>();
         if (health.GetTotalHealth() > 0.0f)
         {
            health.RemoveHealth(100.0f);
         }
    }

    public void resetHit()
    {
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        target = FindObjectOfType<Player>();
        Hitbox[] hitboxes = this.GetComponentsInChildren<Hitbox>();
        foreach (Hitbox hitbox in hitboxes)
        {
            hitbox.useResponder(this);
        }
    }

    protected override void Update()
    {
        base.Update();

        Health health = GetComponent<Health>();
        if (health != null)
        {
            health.UIGroup.SetActive(true);
        }
    }

    protected override void CalculateVelocity()
    {
        if (isAlive)
        {
            if (target.transform.position.x - transform.position.x > maxDistance && target.GetVelocity().x > maxXSpeedGround)
            {
                velocity = new Vector2(target.GetVelocity().x, 0.0f);
            }
            else
            {
                velocity = new Vector2(maxXSpeedGround, 0.0f);
            }
        }
        else
        {
            velocity = Vector2.zero;
        }
    }
}

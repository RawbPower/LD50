using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Impact : MonoBehaviour, IHitboxResponder
{
    public float damage;

    private bool hit;

    // Start is called before the first frame update
    void Start()
    {
        hit = false;

        Hitbox[] hitboxes = this.GetComponentsInChildren<Hitbox>();
        foreach (Hitbox hitbox in hitboxes)
        {
            hitbox.useResponder(this);
        }

        FindObjectOfType<GlobalAudioManager>().Play("Explosion");
    }

    public void collisionedWith(Collider2D collider)
    {
        if (!hit)
        {
            Health health = collider.GetComponentInParent<Health>();
            if (health.GetTotalHealth() > 0.0f)
            {
                health.RemoveHealth(damage);
                hit = true;
            }
        }
    }

    public void resetHit()
    {
        hit = false;
    }
}

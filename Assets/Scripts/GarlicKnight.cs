using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarlicKnight : Enemy
{
    public Projectile garlicBomb;
    public float throwTime;
    public float throwRange;
    public Transform bombSpawn;
    public float throwAngleRange;
    public float agroRange;

    private float throwCounter;
    private Entity entity;
    private Player target;
    private AISword crusher;
    private bool playingTheme = false;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        throwCounter = 0.0f;
        entity = GetComponent<Entity>();
        target = FindObjectOfType<Player>();
        crusher = GetComponentInChildren<AISword>();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if (throwCounter > 0.0f)
        {
            throwCounter -= Time.deltaTime;
        }

        if (entity.IsAlive())
        {
            Vector3 distanceToTarget = target.transform.position - entity.transform.position;

            if (Mathf.Abs(distanceToTarget.x) < agroRange)
            {
                Health health = GetComponent<Health>();
                if (health != null)
                {
                    health.UIGroup.SetActive(true);
                    if (!playingTheme)
                    {
                        FindObjectOfType<GlobalAudioManager>().Stop("DayTheme");
                        FindObjectOfType<GlobalAudioManager>().Play("GarlicKnightTheme");
                        playingTheme = true;
                    }
                }

                if (distanceToTarget.x < 0.0f && entity.IsFacingRight())
                {
                    entity.Flip();
                }
                else if (distanceToTarget.x > 0.0f && !entity.IsFacingRight())
                {
                    entity.Flip();
                }

                if (throwCounter <= 0.0f && distanceToTarget.sqrMagnitude < throwRange * throwRange)
                {
                    Vector2 projectPos = bombSpawn.position;
                    GameObject projectileObject = Instantiate(garlicBomb.gameObject, projectPos, Quaternion.identity);
                    Projectile projectile = projectileObject.GetComponent<Projectile>();
                    Vector2 throwDirection = new Vector2(0.0f, 1.0f);
                    throwDirection = Quaternion.AngleAxis(Random.Range(-throwAngleRange, throwAngleRange), Vector3.forward) * Vector2.up;
                    projectile.SetDirection(throwDirection);
                    projectile.SetOwner(entity);
                    throwCounter = throwTime;
                }
            }
        }
    }

    protected override void CalculateVelocity()
    {
        if (isAlive && !crusher.freezeMovement)
        {
            if (Mathf.Abs(target.transform.position.x - transform.position.x) < agroRange)
            {
                if (Mathf.Abs(target.transform.position.x - transform.position.x) > crusher.attackRange)
                {
                    if (target.transform.position.x > transform.position.x)
                    {
                        velocity = new Vector2(maxXSpeedGround, velocity.y);
                    }
                    else if (target.transform.position.x < transform.position.x)
                    {
                        velocity = new Vector2(-maxXSpeedGround, velocity.y);
                    }
                }
            }
        }
    }
}

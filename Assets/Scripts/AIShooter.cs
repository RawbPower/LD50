using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIShooter : MonoBehaviour
{
    public Transform triggerArm;
    public Bullet bullet;
    public Transform bulletSpawnPosition;
    private Player target;
    public float firingRange;

    public float shotsPerSecond;

    private Entity entity;
    private float shotCounter;
    // Start is called before the first frame update
    void Start()
    {
        entity = GetComponent<Entity>();
        target = FindObjectOfType<Player>();
        shotCounter = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (shotCounter > 0.0f)
        {
            shotCounter -= Time.deltaTime;
        }

        Vector3 distanceToTarget = target.transform.position - transform.position;

        if (triggerArm.gameObject.activeSelf)
        {
            if (distanceToTarget.sqrMagnitude < firingRange * firingRange)
            {
                if (distanceToTarget.x < 0.0f && entity.IsFacingRight())
                {
                    entity.Flip();
                }
                else if (distanceToTarget.x > 0.0f && !entity.IsFacingRight())
                {
                    entity.Flip();
                }

                float armAngle = Mathf.Atan2(distanceToTarget.y * transform.localScale.x, distanceToTarget.x * transform.localScale.x);
                triggerArm.eulerAngles = new Vector3(0.0f, 0.0f, armAngle * Mathf.Rad2Deg);

                if (shotCounter <= 0.0f)
                {
                    GameObject spawnedBullet = Instantiate(bullet.gameObject, bulletSpawnPosition.position, Quaternion.identity);
                    spawnedBullet.GetComponent<Bullet>().SetDirection(distanceToTarget);
                    FindObjectOfType<GlobalAudioManager>().Play("Gun");
                    shotCounter = 1.0f/shotsPerSecond;
                }
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    public Projectile cannonball;
    public float throwTime;
    public Vector2 direction;
    public Transform bombSpawn;
    public float throwAngleRange;

    private Entity entity;
    private float throwCounter;

    // Start is called before the first frame update
    public void Start()
    {
        throwCounter = 0.0f;
        entity = GetComponentInParent<Entity>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (entity.IsAlive())
        {
            if (throwCounter > 0.0f)
            {
                throwCounter -= Time.deltaTime;
            }

            if (throwCounter <= 0.0f)
            {
                Vector2 projectPos = bombSpawn.position;
                GameObject projectileObject = Instantiate(cannonball.gameObject, projectPos, Quaternion.identity);
                Projectile projectile = projectileObject.GetComponent<Projectile>();
                Vector2 throwDirection = direction;
                throwDirection = Quaternion.AngleAxis(Random.Range(-throwAngleRange, throwAngleRange), Vector3.forward) * direction;
                projectile.SetDirection(throwDirection);
                projectile.SetOwner(entity);
                throwCounter = throwTime;
                FindObjectOfType<GlobalAudioManager>().Play("Cannon");
            }
        }
    }
}

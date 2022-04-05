using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAttackManager : MonoBehaviour, IHitboxResponder
{
    public float attackTime;
    public float eatTime;
    public float comboTime;
    public float cooldownTime;
    public float gunCooldownTime;
    public Transform bulletSpawnPosition;
    public Bullet bullet;
    public int ammo;
    public Animator cylinder;
    public Image[] bulletUI;

    private bool rolling;
    private bool isAttacking;
    private bool isEating;
    private bool hasHit;
    private float attackCounter;
    private float eatCounter;
    private float cooldownCounter;
    private float gunCooldownCounter;
    private AnimationManager animationManager;
    private Player player;

    // Start is called before the first frame update
    void Start()
    {
        isAttacking = true;
        isEating = true;
        hasHit = false;
        attackCounter = 0.0f;
        eatCounter = 0.0f;
        cooldownCounter = 0.0f;
        gunCooldownCounter = 0.0f;
        animationManager = GetComponentInParent<AnimationManager>();
        player = GetComponentInParent<Player>();

        Hitbox[] hitboxes = this.GetComponentsInChildren<Hitbox>();
        foreach (Hitbox hitbox in hitboxes)
        {
            hitbox.useResponder(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        rolling = false;
        if (player.IsAlive())
        {
            if (cooldownCounter > 0.0f)
            {
                cooldownCounter -= Time.deltaTime;
            }

            if (gunCooldownCounter > 0.0f)
            {
                gunCooldownCounter -= Time.deltaTime;
            }

            if (eatCounter > 0.0f)
            {
                eatCounter -= Time.deltaTime;
            }

            if (attackCounter > 0.0f)
            {
                attackCounter -= Time.deltaTime;

                if (animationManager.IsAnimationPlaying("Claw1"))
                {
                    if (attackCounter < comboTime && Input.GetButtonDown("Fire1"))
                    {
                        player.audioManager.Play("Swipe2");
                        animationManager.SetBoolParameter("Claw1", false);
                        animationManager.SetBoolParameter("Claw2", true);
                        hasHit = false;
                        attackCounter = attackTime;
                        isAttacking = true;
                    }
                }
                else if (animationManager.IsAnimationPlaying("Claw2"))
                {
                    animationManager.SetBoolParameter("Claw1", false);
                }
            }
            else
            {
                if (isAttacking)
                {
                    animationManager.SetBoolParameter("Claw1", false);
                    animationManager.SetBoolParameter("Claw2", false);
                    animationManager.SetBoolParameter("Eat", false);
                    isAttacking = false;
                    cooldownCounter = cooldownTime;
                }

                if (cooldownCounter <= 0.0f)
                {
                    if (Input.GetButtonDown("Fire1"))
                    {
                        player.audioManager.Play("Swipe1");
                        animationManager.SetBoolParameter("Claw1", true);
                        attackCounter = attackTime;
                        isAttacking = true;
                    }
                }

                if (gunCooldownCounter <= 0.0f && ammo > 0)
                {
                    if (Input.GetAxis("Shoot") > 0.0f)
                    {
                        animationManager.SetTrigger("Shoot");
                        GameObject spawnedBullet = Instantiate(bullet.gameObject, bulletSpawnPosition.position, Quaternion.identity);
                        player.audioManager.Play("Gun");
                        ammo--;
                        cylinder.SetTrigger("Roll");
                        float bulletDirection = Input.GetAxis("Horizontal") != 0.0f ? Mathf.Sign(Input.GetAxis("Horizontal")) : transform.parent.transform.localScale.x;
                        spawnedBullet.GetComponent<Bullet>().SetDirection(new Vector2(bulletDirection, 0.0f));
                        gunCooldownCounter = gunCooldownTime;
                        rolling = true;
                    }
                }

                if (eatCounter <= 0.0f)
                {
                    if (isEating)
                    {
                        animationManager.SetBoolParameter("Eat", false);
                        isEating = false;
                    }
                    if (Input.GetAxis("Eat") > 0.0f && player.IsGrounded())
                    {
                        animationManager.SetBoolParameter("Eat", true);
                        eatCounter = eatTime;
                        isEating = true;
                    }
                }
            }
        }

        UpdateUI();
    }

    private void UpdateUI()
    {
        int ammoPlusRolling = ammo;
        int loopShift = 0;
        if (cylinder.GetCurrentAnimatorStateInfo(0).IsName("Ammo") || rolling)
        {
            bulletUI[0].gameObject.SetActive(false);
            ammoPlusRolling++;
            loopShift++;
        }

        for (int i = 0; i < bulletUI.Length - loopShift; i++)
        {
            bulletUI[i + loopShift].gameObject.SetActive(i + loopShift < ammoPlusRolling);
        }
    }

    public bool IsEating() { return isEating; }

    public void collisionedWith(Collider2D collider)
    {
        if (!hasHit)
        {
            if (isAttacking)
            {
                Health health = collider.GetComponentInParent<Health>();
                if (health)
                {
                    health.RemoveHealth(50.0f);
                    health.bloodHit.transform.localScale = -transform.parent.transform.localScale;
                    health.bloodHit.Play();
                    FindObjectOfType<GlobalAudioManager>().Play("Hit");
                }
            }
            else if (isEating)
            {
                Health health = GetComponentInParent<Health>();
                if (health != null)
                {
                    Enemy enemy = collider.GetComponentInParent<Enemy>();
                    if (enemy != null && !enemy.IsAlive() && enemy.GetHasBlood())
                    {
                        health.FullHeal();
                        player.audioManager.Play("Bite");
                        enemy.SetHasBlood(false);
                    }
                }
            }
            hasHit = true;
        }
    }

    public void resetHit()
    {
        hasHit = false;
    }
}

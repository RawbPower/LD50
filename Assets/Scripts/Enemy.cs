using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Controller2D))]
public class Enemy : Entity
{
    public GameObject triggerArm;
    public ParticleSystem hasBloodParticles;

    private bool hasBlood;

    protected override void Start()
    {
        base.Start();
        hasBlood = true;
    }

    protected virtual void Update()
    {
        isGrounded = controller.collisions.below && !isFlying;
        animationManager?.ManageEnemyAnimationState(velocity, isAlive);

        if (triggerArm != null && !IsAlive() && triggerArm.activeSelf)
        {
            triggerArm.SetActive(false);
        }

        if (!isAlive && hasBloodParticles != null)
        {
            if (hasBlood && !hasBloodParticles.isPlaying)
            {
                hasBloodParticles.Play();
            }
            else if (!hasBlood && hasBloodParticles.isPlaying)
            {
                hasBloodParticles.Stop();
            }
        }
    }

    public bool GetHasBlood() { return hasBlood; }
    public void SetHasBlood(bool blood) { hasBlood = blood; }
}

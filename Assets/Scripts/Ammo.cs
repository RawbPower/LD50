using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour
{
    public LayerMask playerMask;

    private BoxCollider2D boxCollider;
    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        int numColliders = 10;
        Collider2D[] colliders = new Collider2D[numColliders];
        ContactFilter2D contactFilter = new ContactFilter2D();
        contactFilter.SetLayerMask(playerMask);
        contactFilter.useTriggers = true;
        int colliderCount = boxCollider.OverlapCollider(contactFilter, colliders);

        if (colliderCount > 0)
        {
            PlayerAttackManager player = colliders[0].GetComponentInChildren<PlayerAttackManager>();
            if (player != null)
            {
                player.ammo = 6;
                GlobalAudioManager audioManager = FindObjectOfType<GlobalAudioManager>();
                audioManager.Play("Ammo");
                Destroy(this.gameObject);
            }
        }
    }
}

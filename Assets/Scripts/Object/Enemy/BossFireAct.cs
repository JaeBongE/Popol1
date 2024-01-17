using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFireAct : MonoBehaviour
{
    [SerializeField] Collider2D bossFireActHitBox;

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    private void Awake()
    {
        bossFireActHitBox.enabled = false;
    }

    private void Update()
    {
        Invoke("destroySelf", 1f);
    }

    public void EnableAttack()
    {
        bossFireActHitBox.enabled = true;
    }

    public void DisableAttack()
    {
        bossFireActHitBox.enabled = false;
    }

    private void destroySelf()
    {
        Destroy(gameObject);
    }
}

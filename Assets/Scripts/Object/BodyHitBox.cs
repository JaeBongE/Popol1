using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyHitBox : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == GameTag.Enemy.ToString())
        {
            Player playerSc = GetComponentInParent<Player>();
            playerSc.onDamage(collision.transform.position);
            playerSc.Hit(1.0f);
        }
    }
}

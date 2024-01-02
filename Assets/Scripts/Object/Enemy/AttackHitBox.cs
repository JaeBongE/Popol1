using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHitBox : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == GameTag.Player.ToString())
        {
            Player playerSc = GetComponentInParent<Player>();
            playerSc.onDamaged(collision.gameObject.transform.position);
            playerSc.Hit(1.0f);
        }
    }
}

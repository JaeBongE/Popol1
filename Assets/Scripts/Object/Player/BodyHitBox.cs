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
           // Enemy enemy = collision.GetComponentInParent<Enemy>();
            playerSc.onDamaged(collision.transform.root.position);
            playerSc.Hit(1.0f);
        }
        if (collision.gameObject.tag == GameTag.Potal.ToString())
        {
            Player playerSc = GetComponentInParent<Player>();
            playerSc.NextStage();
        }
    }

}

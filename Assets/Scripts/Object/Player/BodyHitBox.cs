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
        
        if (collision.gameObject.tag == GameTag.EnemyOj.ToString())
        {
            Player playerSc = GetComponentInParent<Player>();
           // Enemy enemy = collision.GetComponentInParent<Enemy>();
            playerSc.onDamaged(collision.transform.root.position);
            playerSc.Hit(1.0f);
        }

        //if (Input.GetKeyDown(KeyCode.UpArrow) && collision.gameObject.tag == GameTag.Potal.ToString())
        //{
        //    Debug.Log("Æ÷Å»¿¡ ´ê¾Ò½À´Ï´Ù.");    
        //    Player playerSc = GetComponentInParent<Player>();
        //    playerSc.NextStage();
        //}
        //if (collision.gameObject.tag == GameTag.Potal.ToString())
        //{
        //    Debug.Log("Æ÷Å»¿¡ ´ê¾Ò½À´Ï´Ù.");
        //    Player playerSc = GetComponentInParent<Player>();
        //    playerSc.NextStage();
        //}
    }

   
}

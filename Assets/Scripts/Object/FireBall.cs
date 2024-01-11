using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    [SerializeField] private float speed = 5;
    private float damage = 1f;
    private bool isPlayerFire;
    Transform player;
    bool isRight = false;

    private void Awake()
    {
        if(transform.rotation.eulerAngles.z == 90f)
        {
            isRight = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isPlayerFire == true && collision.gameObject.tag == GameTag.Enemy.ToString())
        {
            Enemy enemySc = collision.GetComponentInParent<Enemy>();
            enemySc.Hit(damage);
            Destroy(gameObject);
        }
        //if (isPlayerFire == false && collision.gameObject.tag == GameTag.BodyHitBox.ToString())
        //{
        //    Player playerSc = GetComponentInParent<Player>();
        //    playerSc.onDamaged(gameObject.transform.position);
        //    playerSc.Hit(damage);
        //    Destroy(gameObject);
        //}
        if (isPlayerFire == false && collision.gameObject.tag == GameTag.BodyHitBox.ToString() || collision.gameObject.tag == GameTag.Ground.ToString())
        {
            Destroy(gameObject);
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    private void checkPlayer()
    {
    if (player == null)
        {
            GameObject objPlayer = GameObject.Find("Player");
            player = objPlayer.transform;
        }
    }

    void Update()
    {
        checkPlayer();
        movingP();
        movingE();
    }

    private void movingP()
    {
        if (isPlayerFire == false) return;
        if (isRight == true)
        {
            gameObject.transform.position += Vector3.right * Time.deltaTime * speed;
        }
        else if (isRight == false)
        {
            gameObject.transform.position -= Vector3.right * Time.deltaTime * speed;
        }
    }
    private void movingE()
    {
        if (isPlayerFire == true) return;
        if (isRight == true)
        {
            gameObject.transform.position += Vector3.right * Time.deltaTime * speed;
        }
        else if (isRight == false)
        {
            gameObject.transform.position -= Vector3.right * Time.deltaTime * speed;
        }
    }

    public void SetFire(bool _isPlayer)
    {
        isPlayerFire = _isPlayer;
    }
}

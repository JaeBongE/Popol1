using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class Elevator2 : MonoBehaviour
{
    [SerializeField] private float speed = 3.0f;
    Vector3 dir = Vector3.left;
    Vector3 dir2 = Vector3.up;
    GameObject player;

    [SerializeField] private bool checkX;
    [SerializeField] private bool checkY;

    [SerializeField] private float checkMinPos1;
    [SerializeField] private float checkMaxPos2;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == GameTag.BodyHitBox.ToString())
        {
            player.transform.SetParent(transform);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == GameTag.BodyHitBox.ToString())
        {
            player.transform.SetParent(transform);
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == GameTag.BodyHitBox.ToString())
        {
            player.transform.SetParent(null);
        }
    }

    private void Update()
    {
        checkPlayer();
        checkMoveDir();
    }

    private void checkPlayer()
    {
        if (player == null)
        {
            GameObject objPlayer = GameObject.Find("Player");
            player = objPlayer;
        }
    }

    private void FixedUpdate()
    {
        moving();
    }

    private void checkMoveDir()
    {
        //if (checkLeft.IsTouchingLayers(ground) == true)
        //{
        //    dir *= -1f;
        //}
        //else if (checkRight.IsTouchingLayers(ground) == true)
        //{
        //    dir *= -1f;
        //}

        if (checkX)//x를 체크
        {
            if(transform.position.x <= checkMinPos1)
            {
                dir.x = 1;
            }
            else if(transform.position.x >= checkMaxPos2)
            {
                dir.x = -1;
            }
        }

        if (checkY)//y를 체크
        {
            if (transform.position.y <= checkMinPos1)
            {
                dir2.y = 1;
            }
            else if (transform.position.y >= checkMaxPos2)
            {
                dir2.y = -1;
            }
        }

    }

    private void moving()
    {
        if (checkX == false && checkY == false) return;
        if (checkX == true)
        {
            gameObject.transform.position += dir * Time.deltaTime * speed;
        }
        else if (checkY == true)
        {
            gameObject.transform.position += dir2 * Time.deltaTime * speed;
        }
        
    }

}

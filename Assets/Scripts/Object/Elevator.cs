using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    [SerializeField] private float speed = 3.0f;
    [SerializeField] Collider2D dirCheck;
    [SerializeField] private LayerMask ground;
    private bool isMoveUp = false;
    private Vector3 dir = Vector3.up;


    void FixedUpdate()
    {
        moving();
    }
    private void moving()
    {
        checkDir();
        gameObject.transform.position += dir * Time.deltaTime * speed;

    }

    private void checkDir()
    {
        if (dirCheck.IsTouchingLayers(ground) == false) return;
        if (dirCheck.IsTouchingLayers(ground) == true)
        {
            dir *= -1;
        }
        
    }

}

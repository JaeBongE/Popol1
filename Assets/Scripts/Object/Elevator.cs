using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    [SerializeField] private float speed = 3.0f;
    [SerializeField] Collider2D underCheck;
    [SerializeField] Collider2D upCheck;
    [SerializeField] private LayerMask ground;

    private void Awake()
    {
    }

    void Update()
    {
        moving();
    }

    private void moving()
    {
        gameObject.transform.position += Vector3.up * Time.deltaTime * speed;
    }

}

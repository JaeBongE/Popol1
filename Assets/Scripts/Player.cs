using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("플레이어 데이터")]
    Rigidbody2D rigid;
    Vector3 moveDir;//default 0,0,0
    [SerializeField] float moveSpeed = 5.0f;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        moving();
        turning();
    }
    /// <summary>
    /// horizontal을 입력했을 때 플레이어 이동함수
    /// </summary>
    private void moving()
    {
        moveDir.x = Input.GetAxisRaw("Horizontal") * moveSpeed;//-1 0 1
        moveDir.y = rigid.velocity.y;
        rigid.velocity = moveDir;
    }
    /// <summary>
    /// 플레이어의 이동 방향에 따라 로컬스케일 x 값을 반대로 주어 회전하게 하는 함수
    /// </summary>
    private void turning()
    {
        if (moveDir.x > 0 && transform.localScale.x < 1 || moveDir.x < 0 && transform.localScale.x > -1)//오른쪽 or 왼쪽 이동
        {
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }
}

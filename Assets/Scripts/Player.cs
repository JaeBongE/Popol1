using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Camera mainCam;

    [Header("플레이어 데이터")]
    Rigidbody2D rigid;
    PolygonCollider2D polygonColider2D;
    Animator anim;
    Vector3 moveDir;//default 0,0,0
    [SerializeField] float moveSpeed = 5.0f;
    [SerializeField] float jumpForce = 5.0f;

    [Header("점프")]
    [SerializeField] float gravity = 9.81f;
    [SerializeField] bool isGround = false;
    private bool isJump = false;
    private bool isDoubleJump = false;
    private bool doubleJump = false;
    private float verticalVelocity;//수직으로 받는 힘

    private void OnDrawGizmos()
    {
        if (polygonColider2D != null)
        {
            Gizmos.color = Color.red;
            Vector3 pos = polygonColider2D.bounds.center - new Vector3(0, 0.1f, 0);
            Gizmos.DrawWireCube(pos, polygonColider2D.bounds.size);
        }
    }

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        polygonColider2D = GetComponent<PolygonCollider2D>();
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        mainCam = Camera.main;
    }

    void Update()
    {
        checkGround();

        moving();
        turning();

        jumping();
        checkGravity();

        doAnimation();
    }

    /// <summary>
    /// 플레이어가 땅에 닿아 있는지 체크하는 함수
    /// </summary>
    private void checkGround()
    {
        isGround = false;
        if (verticalVelocity > 0)
        {
            return;
        }

        RaycastHit2D hit = Physics2D.BoxCast(polygonColider2D.bounds.center, polygonColider2D.bounds.size, 0, Vector2.down, 0.1f, LayerMask.GetMask("Ground"));
        if (hit.transform != null && hit.transform.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            //Debug.Log(hit.transform.gameObject.name);
            isGround = true;
        }
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

    /// <summary>
    /// 점프 조건을 확인하는 함수
    /// </summary>
    private void jumping()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGround == true)//스페이스바를 누르고 땅에 붙어 있다면 점프조건 활성화
        {
            isJump = true;
        }
        else if (Input.GetKeyDown(KeyCode.Space) && isGround == false && isJump == false && isDoubleJump == true)
        {
            doubleJump = true;
        }
    }

    /// <summary>
    /// 중력 데이터로 수직으로 받는 힘을 조절하는 함수
    /// </summary>
    private void checkGravity()
    {
        if (isGround == false)//공중에 떠 있을 때
        {
            verticalVelocity -= gravity * Time.deltaTime;//수직으로 받는 힘이 gravity * Time.deltaTime만큼 줄어들고
            if (verticalVelocity < -10.0f)//수직으로 받는 힘이 -10이 넘는다면
            {
                verticalVelocity = -10.0f;//-10으로 받는다
            }
        }
        else//땅에 붙어 있을 때
        {
            verticalVelocity = 0.0f;//수직으로 받는 힘은 0이 된다.
        }
        
        if (isJump == true)//점프가 활성화 되었을 때
        {
            isJump = false;
            isDoubleJump = true;
            verticalVelocity = jumpForce;//점프 포스만큼 수직으로 받는 힘을 준다
        }

        if (doubleJump == true)
        {
            doubleJump = false;
            isDoubleJump = false;
            verticalVelocity = jumpForce;
        }

        rigid.velocity = new Vector2 (rigid.velocity.x, verticalVelocity);
    }
    /// <summary>
    /// 애니메이션 변수 전달 함수
    /// </summary>
    private void doAnimation()
    {
        anim.SetInteger("Horizontal", (int)moveDir.x);
        anim.SetBool("isGround", isGround);
    }
}

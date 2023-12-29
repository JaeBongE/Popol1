using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using static GameManager;

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
    float maxHp = 10;
    [SerializeField] float curHp = 0;
    Slider playerHp;

    [Header("점프")]
    [SerializeField] float gravity = 9.81f;
    [SerializeField] bool isGround = false;
    private bool isJump = false;
    private bool isDoubleJump = false;
    private bool doubleJump = false;
    private float verticalVelocity;//수직으로 받는 힘

    [Header("어택")]
    [SerializeField] Collider2D swordHitBox;
    Enemy enemy;
    private float VskillCoolTime = 5.0f;
    private float VskillCoolTimer = 5.0f;
    private bool isVcoolTime = false;
    Image vCoolTime;
    TMP_Text vCoolTimeText;

    float timerHit = 0.0f;
    float timerHitLimit = 0.5f;

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.gameObject.tag == GameTag.Potal.ToString())
    //    {
    //        Debug.Log("다음 스테이지 이동");
    //    }
    //}

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        polygonColider2D = GetComponent<PolygonCollider2D>();
        anim = GetComponent<Animator>();
        curHp = maxHp;
        
    }

    private void Start()
    {
        mainCam = Camera.main;
        enemy = GetComponent<Enemy>();
        //zCoolTime.enabled = false;
    }

    void Update()
    {
        checkPlayerUI();
        checkGround();
        checkTimer();

        moving();
        turning();

        jumping();
        checkGravity();
        attack();
        heal();
        healTimer();

        doAnimation();
        NextStage();
    }

    private void checkPlayerUI()
    {
        if (playerHp == null)//UI에서 데이터를 가져 오는 조건
        {
            GameObject objPlayerUI = GameObject.Find("PlayerUI");

            if (objPlayerUI == null)
            { 
                return;
            }

            PlayerUI scUI = objPlayerUI.GetComponent<PlayerUI>();
            //(Image _VcoolTime, Slider _PlayerHp, TMP_Text _VCoolTimeText) data = scUI.GetProperty();

            //var data2 = scUI.GetProperty();
            playerHp = scUI.GetPlayerHp();
            (Image _vCoolTime, TMP_Text _vCoolTimeText) VskillData = scUI.GetVskill();
            vCoolTime = VskillData._vCoolTime;
            vCoolTimeText = VskillData._vCoolTimeText;
            //playerHp = data._PlayerHp;
            //vCoolTime = data._VcoolTime;
            //vCoolTimeText = data._VCoolTimeText;
            playerHp.maxValue = maxHp;
            vCoolTime.fillAmount = 0f;

            //vCoolTime = scUI.GetImage();
        }



        //if (playerHp == null)
        //{
        //    GameObject objCanvas = GameObject.Find("Canvas");

        //    if (objCanvas == null)
        //    {
        //        return;
        //    }

        //    GameObject objPlayerHp = objCanvas.transform.Find("PlayerIcon/PlayerHp").gameObject;
        //    playerHp = objPlayerHp.GetComponent<Slider>();

        //}
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

    private void checkTimer()
    {
        if (timerHit > 0.0f)
        {
            timerHit -= Time.deltaTime;
            if(timerHit < 0.0f)
            {
                timerHit = 0.0f;
            }
        }
    }

    /// <summary>
    /// horizontal을 입력했을 때 플레이어 이동함수
    /// </summary>
    private void moving()
    {
        if (timerHit > 0.0f)
            return;

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
        else if (Input.GetKeyDown(KeyCode.Space) && isGround == false && isJump == false && isDoubleJump == true)//스페이스바를 누르고 공중에 있고 점프 비활성화 더블 점프조건이 활성화
        {
            doubleJump = true;//더블점프 활성화
        }
    }

    /// <summary>
    /// 중력 데이터로 수직으로 받는 힘을 조절하는 함수(점프,더블점프)
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
            isDoubleJump = true;//더블점프 조건 활성화
            verticalVelocity = jumpForce;//점프 포스만큼 수직으로 받는 힘을 준다
        }

        if (doubleJump == true)//더블 점프가 활성화 되었을 때
        {
            doubleJump = false;
            isDoubleJump = false;
            verticalVelocity = jumpForce;
        }

        rigid.velocity = new Vector2(rigid.velocity.x, verticalVelocity);
    }

    /// <summary>
    /// z키를 눌렀을 때 공격 활성
    /// </summary>
    private void attack()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            anim.SetTrigger("isAttacking");
        }
    }

    private void heal()
    {
        if (curHp == maxHp) return;
        
        if (Input.GetKeyDown(KeyCode.V) && isVcoolTime == false)
        {
            isVcoolTime = true;
            //zCoolTime.enabled = true;
            curHp += 3;
            if (curHp >= maxHp)
            {
                curHp = maxHp;
            }
            playerHp.value = curHp;
        }

        if (VskillCoolTime == 0f)
        {
            //zCoolTime.enabled = false;
            VskillCoolTime = VskillCoolTimer;
        }

    }

    private void healTimer()
    {
        if (isVcoolTime == true && VskillCoolTime > 0f)
        {
            VskillCoolTime -= Time.deltaTime;
            vCoolTime.fillAmount = VskillCoolTime / VskillCoolTimer;
            //zCoolTimeText.text = ($"{Mathf.Floor(ZskillCoolTime * 10f) / 10f}");
            vCoolTimeText.text = ($"{VskillCoolTime.ToString("F1")}");
            if (VskillCoolTime <= 0f)
            {
                VskillCoolTime = 0f;
                vCoolTimeText.text = "";
                isVcoolTime = false;
            }
        }
    }

    /// <summary>
    /// 애니메이션 변수 전달 함수
    /// </summary>
    private void doAnimation()
    {
        anim.SetInteger("Horizontal", (int)moveDir.x);//좌우 이동 애니메이션
        anim.SetBool("isGround", isGround);//땅에 닿았는지 체크해서 닿지 않으면 점프 애니메이션
    }

    /// <summary>
    /// _pos의 적이 닿았다면 player가 뒤로 날아가며 애니메이션이 작동함
    /// </summary>
    /// <param name="_pos"> 적</param>
    public void onDamaged(Vector2 _pos)
    {
        //무적상태라면 return;
        timerHit = timerHitLimit;
        rigid.velocity = Vector2.zero;
        verticalVelocity = 0.0f;
        Vector3 position = transform.position;
        if (position.x > _pos.x)//닿은 상대가 player보다 왼쪽에 있다면
        {
            rigid.AddForce(new Vector2(2, 1) * 2, ForceMode2D.Impulse);//오른쪽으로 날아감
        }
        else if (position.x <= _pos.x)//오른쪽에 있다면
        {
            rigid.AddForce(new Vector2(-2, 1) * 2, ForceMode2D.Impulse);//왼쪽으로 날아감
        }
        verticalVelocity = rigid.velocity.y;

        anim.SetTrigger("isPlayerHit");//hit애니메이션 작동
    }

    /// <summary>
    /// player가 데미지를 입는 함수
    /// </summary>
    /// <param name="_damage"></param>
    public void Hit(float _damage)
    {
        curHp -= _damage;//hp가 _damage만큼 감소
        playerHp.value = curHp;
        if (curHp <= 0)//플레이어가 죽는 경우
        {
            Debug.Log("플레이어가 죽었습니다");
            anim.SetTrigger("isPlayerDeath");//플레이어 사망 애니메이션 
            GameManager.Instance.GameOver();//게임오버 메뉴가 나온다
        }
    }

    /// <summary>
    /// 플레이어가 어택 했을 때 소드 히트박스가 생기게 해주는 함수
    /// </summary>
    public void EnableAttack()
    {
        swordHitBox.enabled = true;
    }
    /// <summary>
    /// 공격이 끝난 후 소드 히트박스를 없애주는 함수
    /// </summary>
    public void DisableAttack()
    {
        swordHitBox.enabled = false;
    }

    public void NextStage()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            SceneManager.LoadSceneAsync((int)enumScene.Stage2);
        }
    }
}

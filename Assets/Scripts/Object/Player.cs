using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private Camera mainCam;
    private GameManager gameManager;

    [Header("�÷��̾� ������")]
    Rigidbody2D rigid;
    PolygonCollider2D polygonColider2D;
    Animator anim;
    Vector3 moveDir;//default 0,0,0
    [SerializeField] float moveSpeed = 5.0f;
    [SerializeField] float jumpForce = 5.0f;
    [SerializeField] float maxHp = 10;
    [SerializeField] float curHp = 0;
    [SerializeField] Slider playerHp;

    [Header("����")]
    [SerializeField] float gravity = 9.81f;
    [SerializeField] bool isGround = false;
    private bool isJump = false;
    private bool isDoubleJump = false;
    private bool doubleJump = false;
    private float verticalVelocity;//�������� �޴� ��

    [Header("����")]
    [SerializeField] Collider2D swordHitBox;
    Enemy enemy;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        polygonColider2D = GetComponent<PolygonCollider2D>();
        anim = GetComponent<Animator>();
        curHp = maxHp;
        playerHp.maxValue = maxHp;
    }

    private void Start()
    {
        mainCam = Camera.main;
        enemy = GetComponent<Enemy>();
        gameManager = GetComponent<GameManager>();
    }

    void Update()
    {
        checkGround();

        moving();
        turning();

        jumping();
        checkGravity();

        attack();

        doAnimation();
    }

    /// <summary>
    /// �÷��̾ ���� ��� �ִ��� üũ�ϴ� �Լ�
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
    /// horizontal�� �Է����� �� �÷��̾� �̵��Լ�
    /// </summary>
    private void moving()
    {
        moveDir.x = Input.GetAxisRaw("Horizontal") * moveSpeed;//-1 0 1
        moveDir.y = rigid.velocity.y;
        rigid.velocity = moveDir;
    }
    /// <summary>
    /// �÷��̾��� �̵� ���⿡ ���� ���ý����� x ���� �ݴ�� �־� ȸ���ϰ� �ϴ� �Լ�
    /// </summary>
    private void turning()
    {
        if (moveDir.x > 0 && transform.localScale.x < 1 || moveDir.x < 0 && transform.localScale.x > -1)//������ or ���� �̵�
        {
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }

    /// <summary>
    /// ���� ������ Ȯ���ϴ� �Լ�
    /// </summary>
    private void jumping()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGround == true)//�����̽��ٸ� ������ ���� �پ� �ִٸ� �������� Ȱ��ȭ
        {
            isJump = true;
        }
        else if (Input.GetKeyDown(KeyCode.Space) && isGround == false && isJump == false && isDoubleJump == true)//�����̽��ٸ� ������ ���߿� �ְ� ���� ��Ȱ��ȭ ���� ���������� Ȱ��ȭ
        {
            doubleJump = true;//�������� Ȱ��ȭ
        }
    }

    /// <summary>
    /// �߷� �����ͷ� �������� �޴� ���� �����ϴ� �Լ�(����,��������)
    /// </summary>
    private void checkGravity()
    {
        if (isGround == false)//���߿� �� ���� ��
        {
            verticalVelocity -= gravity * Time.deltaTime;//�������� �޴� ���� gravity * Time.deltaTime��ŭ �پ���
            if (verticalVelocity < -10.0f)//�������� �޴� ���� -10�� �Ѵ´ٸ�
            {
                verticalVelocity = -10.0f;//-10���� �޴´�
            }
        }
        else//���� �پ� ���� ��
        {
            verticalVelocity = 0.0f;//�������� �޴� ���� 0�� �ȴ�.
        }

        if (isJump == true)//������ Ȱ��ȭ �Ǿ��� ��
        {
            isJump = false;
            isDoubleJump = true;//�������� ���� Ȱ��ȭ
            verticalVelocity = jumpForce;//���� ������ŭ �������� �޴� ���� �ش�
        }

        if (doubleJump == true)//���� ������ Ȱ��ȭ �Ǿ��� ��
        {
            doubleJump = false;
            isDoubleJump = false;
            verticalVelocity = jumpForce;
        }

        rigid.velocity = new Vector2(rigid.velocity.x, verticalVelocity);
    }

    /// <summary>
    /// zŰ�� ������ �� ���� Ȱ��
    /// </summary>
    private void attack()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            anim.SetTrigger("isAttacking");
            //isAttack = true;
        }
    }
    public void onDamage(Vector2 _pos)
    {
        Vector3 position = transform.position;
        if (position.x >= _pos.x)
        {
            rigid.AddForce(new Vector2(-1, 1) * 7, ForceMode2D.Impulse);
        }
        else if (position.x <= _pos.x)
        {
            rigid.AddForce(new Vector2(1, 1) * 7, ForceMode2D.Impulse);
        }

        anim.SetTrigger("isPlayerHit");
    }
    public void Hit(float _damage)
    {
        curHp -= _damage;
        playerHp.value = curHp;
        if (curHp <= 0)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// �ִϸ��̼� ���� ���� �Լ�
    /// </summary>
    private void doAnimation()
    {
        anim.SetInteger("Horizontal", (int)moveDir.x);
        anim.SetBool("isGround", isGround);
    }

    public void EnableAttack()
    {
        swordHitBox.enabled = true;
    }
    public void DisableAttack()
    {
        swordHitBox.enabled = false;
    }


}
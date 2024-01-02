using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("적 스탯")]
    [SerializeField] private float maxHp = 3.0f;
    private float curHp = 0f;
    [SerializeField] private float moveSpeed = 1.0f;
    private float defaultSpeed;
    private Rigidbody2D rigid;
    Animator anim;
    private bool isPlayerLookAtRight = false;

    [Header("턴 기능")]
    [SerializeField] private LayerMask ground;
    [SerializeField] private Collider2D wallCheckBox;
    [SerializeField] private Collider2D groundCheckBox;

    Transform player;

    [Header("적 공격")]
    [SerializeField] private Collider2D attackHitBox;


    public enum enumEnemyType
    {
        FlyingEye,
        Skeleton,
    }
    public enumEnemyType enemyType;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        curHp = maxHp;
        anim = GetComponent<Animator>();
        defaultSpeed = moveSpeed;
    }

    void Update()
    {
        checkPlayer();
        moving();
        checkDirection();
    }

    private void checkPlayer()
    {
        if (player == null)
        {
            GameObject objPlayer = GameObject.Find("Player");
            player = objPlayer.transform;
        }
    }

    private void FixedUpdate()
    {
        if (wallCheckBox.IsTouchingLayers(ground) == true)//wallcheckbox가 벽에 닿았다면 턴
        {
            turning();
        }

        if (groundCheckBox.IsTouchingLayers(ground) == false)//groundcheckbox가 땅에서 떨어진다면 턴
        {
            turning();
        }

    }

    /// <summary>
    /// 적이 좌우로 움직이는 함수
    /// </summary>
    private void moving()
    {
        rigid.velocity = new Vector2(moveSpeed, rigid.velocity.y);
    }

    private void checkDirection()
    {
        if (player.transform.localScale.x >= 1)
        {
            isPlayerLookAtRight = true;
        }
        else if (player.transform.localScale.x <= -1)
        {
            isPlayerLookAtRight = false;
        }
    }

    /// <summary>
    /// 앞에 벽이 있거나 땅이 없다면 턴하는 함수
    /// </summary>
    private void turning()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;

        moveSpeed *= -1;
    }

    public void Hit(float _damage)
    {
        curHp -= _damage;
        hitPosition();
        if (curHp <= 0 && enemyType == enumEnemyType.FlyingEye)
        {
            anim.SetTrigger("Death");
            Destroy(gameObject, 0.5f);
        }

        if (curHp <= 0 && enemyType == enumEnemyType.Skeleton)
        {
            anim.SetTrigger("Death2");
            Destroy(gameObject, 0.5f);
        }
    }

    private void hitPosition()
    {
        if (enemyType == enumEnemyType.FlyingEye)
        {
            anim.SetTrigger("Hit");
        }

        if (enemyType == enumEnemyType.Skeleton)
        {
            anim.SetTrigger("Hit2");
        }

        Vector3 position = transform.position;
        if (isPlayerLookAtRight == true)
        {
            position.x += 1;
        }
        else
        {
            position.x -= 1;
        }
        transform.position = position;
    }

    public void Attack()
    {
        if (enemyType == enumEnemyType.Skeleton)
        {
            moveSpeed = 0f;
            anim.SetTrigger("Attack");
            Invoke("reMove", 0.8f);
        }
    }

    private void reMove()
    {
        Vector3 scale = gameObject.transform.localScale;
        if (scale.x >= 1)
        {
            moveSpeed = defaultSpeed;
        }
        else if (scale.x <= 1)
        {
            moveSpeed = -defaultSpeed;
        }
    }

    public void EnableAttack()
    {
        attackHitBox.enabled = true;
    }

    public void DisableAttack()
    {
        attackHitBox.enabled = false;
    }
}

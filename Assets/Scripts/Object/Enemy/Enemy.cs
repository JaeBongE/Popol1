using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UI;

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
    private float timerHit = 0.0f;
    private float timerHitLimit = 0.5f;
    [SerializeField] GameObject enemyBody;

    [Header("턴 기능")]
    [SerializeField] private LayerMask ground;
    [SerializeField] private Collider2D wallCheckBox;
    [SerializeField] private Collider2D groundCheckBox;

    Transform player;

    [Header("적 공격")]
    [SerializeField] private Collider2D attackHitBox;
    [SerializeField] Transform fireBallMPos;
    [SerializeField] GameObject fireBallM;
    private float fireTimer = 5.0f;
    private float fireLimitTimer = 5.0f;
    private bool shootFireM = false;

    private Slider bossHp;
    private int pattern = 1;//현재 패턴
    private int patternShootCount = 0;//현재 패턴을 몇 번 사용했는지
    private float patternTimer = 0.0f;//패턴 리로드 용
    private bool patternChange = false;//패턴을 교체해야 하는지
    private float timerDash = 0.0f;
    private float timerDashLimit = 0.5f;


    public enum enumEnemyType
    {
        FlyingEye,
        Skeleton,
        Obstacle,
        Mushroom,
        Boss,
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
        setBossHp();
        checkPlayer();
        checkTimer();
        checkDashTime();
        moving();
        checkDirection();
        msTurning();
        msFire();
        msFireCoolTime();
        bossPattern();
    }

    private void setBossHp()
    {
        if (bossHp == null && enemyType == enumEnemyType.Boss)
        {
            GameObject bossUI = GameObject.Find("BossUI");

            if (bossUI == null)
            {
                return;
            }
            BossUI scBossUI = bossUI.GetComponent<BossUI>();
            bossHp = scBossUI.getBossHp();

            bossHp.maxValue = maxHp;
        }
        if (enemyType == enumEnemyType.Boss)
        {
            bossHp.value = curHp;
        }
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
        checkTurn();
    }

    private void checkTurn()
    {
        if (enemyType == enumEnemyType.Obstacle) return;
        if (enemyType == enumEnemyType.Mushroom) return;

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
        if (timerHit > 0.0f) return;
        if (enemyType == enumEnemyType.Obstacle) return;
        if (enemyType == enumEnemyType.Mushroom) return;
        if (timerDash > 0.0f && enemyType == enumEnemyType.Boss) return;
        rigid.velocity = new Vector2(moveSpeed, rigid.velocity.y);
        if (enemyType == enumEnemyType.Boss)
        {
            rigid.velocity = new Vector2(-moveSpeed, rigid.velocity.y);
        }
    }

    private void checkDirection()
    {
        if (enemyType == enumEnemyType.Obstacle) return;
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
        if (enemyType == enumEnemyType.Obstacle) return;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;

        moveSpeed *= -1;
    }

    private void msTurning()
    {
        if (enemyType == enumEnemyType.Mushroom)
        {
            Vector3 scale = transform.localScale;
            Vector2 playerPosition = player.transform.position;
            Vector2 position = gameObject.transform.position;
            if (playerPosition.x >= position.x)
            {
                scale.x = 1;
            }
            else if (playerPosition.x < position.x)
            {
                scale.x = -1;
            }
            transform.localScale = scale;
        }
    }

    private void msFire()
    {
        if (enemyType != enumEnemyType.Mushroom) return;

        Vector2 playerSc = gameObject.transform.localScale;
        if (shootFireM == true)
        {
            anim.SetTrigger("Fire");
            if (playerSc.x >= 0)
            {
                GameObject obj = Instantiate(fireBallM, fireBallMPos.position, Quaternion.Euler(0, 0, 90f));
                FireBall scFireBall = obj.GetComponent<FireBall>();
                scFireBall.SetFire(false);
            }
            else if (playerSc.x < 0)
            {
                GameObject obj = Instantiate(fireBallM, fireBallMPos.position, Quaternion.Euler(0, 0, -90f));
                FireBall scFireBall = obj.GetComponent<FireBall>();
                scFireBall.SetFire(false);
            }
        }

        if (fireTimer == 0)
        {
            fireTimer = fireLimitTimer;
            shootFireM = false;
        }
    }
    private void msFireCoolTime()
    {
        if (fireTimer > 0f)
        {
            fireTimer -= Time.deltaTime;
            if (fireTimer <= 0f)
            {
                shootFireM = true;
                fireTimer = 0f;
            }
        }
    }

    private void bossPattern()
    {
        if (enemyType != enumEnemyType.Boss) return;

        patternTimer += Time.deltaTime;

        if (patternChange == true)
        {
            if (patternTimer >= 3.0f)
            {
                patternTimer = 0.0f;
                patternChange = false;
            }
            return;
        }

        switch (pattern)
        {

        }
    }

    private void bossDash()
    {
        if (enemyType != enumEnemyType.Boss) return;
        if (Input.GetKeyDown(KeyCode.K))
        {
            timerDash = timerDashLimit;
            Vector2 scale = gameObject.transform.localScale;
            if (scale.x == 1)
            {
                rigid.AddForce(new Vector2(-5.5f, 0), ForceMode2D.Impulse);
            }
            else if (scale.x == -1)
            {
                rigid.AddForce(new Vector2(5.5f, 0), ForceMode2D.Impulse);
            }
        }
        //timerDash = timerDashLimit;
        //Vector2 scale = gameObject.transform.localScale;
        //if (scale.x == 1)
        //{
        //    rigid.AddForce(new Vector2(-1.5f, 0), ForceMode2D.Impulse);
        //}
        //else if (scale.x == -1)
        //{
        //    rigid.AddForce(new Vector2(-1.5f, 0), ForceMode2D.Impulse);
        //}
    }

    private void bossSmash()
    {
        if (enemyType != enumEnemyType.Boss) return;
        if (Input.GetKeyDown(KeyCode.K))
        {
            moveSpeed = 0f;
            anim.SetTrigger("Smash");
            Invoke("reMove", 0.8f);
        }
            
    }

    private void bossFire()
    {
        if (enemyType != enumEnemyType.Boss) return;
        if (Input.GetKeyDown(KeyCode.K))
        {
            moveSpeed = 0f;
            anim.SetTrigger("BossFire");
            Invoke("reMove", 0.8f);
        }
    }

    private void checkDashTime()
    {
        if (timerDash > 0.0f)
        {
            timerDash -= Time.deltaTime;
            if (timerDash < 0.0f)
            {
                timerDash = 0.0f;
            }
        }
    }

    public void Hit(float _damage)
    {
        if (enemyType == enumEnemyType.Obstacle) return;
        curHp -= _damage;
        hitPosition();
        if (curHp <= 0 && enemyType == enumEnemyType.FlyingEye)
        {
            anim.SetTrigger("Death");
            enemyBody.layer = LayerMask.NameToLayer("EnemyDeath");
            Destroy(gameObject, 0.8f);
        }

        if (curHp <= 0 && enemyType == enumEnemyType.Skeleton)
        {
            anim.SetTrigger("Death2");
            enemyBody.layer = LayerMask.NameToLayer("EnemyDeath");
            Destroy(gameObject, 0.8f);
        }

        if (curHp <= 0 && enemyType == enumEnemyType.Mushroom)
        {
            anim.SetTrigger("Death3");
            enemyBody.layer = LayerMask.NameToLayer("EnemyDeath");
            Destroy(gameObject, 0.8f);
        }

        if (curHp <= 0 && enemyType == enumEnemyType.Boss)
        {
            anim.SetTrigger("Death4");
            enemyBody.layer = LayerMask.NameToLayer("EnemyDeath");
            Destroy(gameObject, 0.8f);
        }

    }

    private void hitPosition()
    {
        if (enemyType == enumEnemyType.Obstacle) return;
        if (enemyType == enumEnemyType.FlyingEye)
        {
            anim.SetTrigger("Hit");
        }

        if (enemyType == enumEnemyType.Skeleton)
        {
            anim.SetTrigger("Hit2");
        }

        if (enemyType == enumEnemyType.Mushroom)
        {
            anim.SetTrigger("Hit3");
        }

        if (enemyType == enumEnemyType.Boss)
        {
            anim.SetTrigger("Hit4");
        }
        setHitPosition();

    }

    private void setHitPosition()
    {
        if (enemyType == enumEnemyType.Mushroom) return;
        if (enemyType == enumEnemyType.Boss) return;
        timerHit = timerHitLimit;
        if (isPlayerLookAtRight == true)
        {
            rigid.AddForce(new Vector2(1.5f, 2), ForceMode2D.Impulse);
        }
        else
        {
            rigid.AddForce(new Vector2(-1.5f, 2), ForceMode2D.Impulse);
        }
    }

    private void checkTimer()
    {
        if (timerHit > 0.0f)
        {
            timerHit -= Time.deltaTime;
            if (timerHit < 0.0f)
            {
                timerHit = 0.0f;
            }
        }
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

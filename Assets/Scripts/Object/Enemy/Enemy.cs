using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [Header("�� ����")]
    [SerializeField] private float maxHp = 3.0f;
    private float curHp = 0f;
    [SerializeField] private float moveSpeed = 1.0f;
    private float defaultSpeed;
    private Rigidbody2D rigid;
    Animator anim;
    private float timerHit = 0.0f;
    private float timerHitLimit = 0.5f;
    [SerializeField] GameObject enemyBody;
    [SerializeField] GameObject enemyUI;

    [Header("�� ���")]
    [SerializeField] private LayerMask ground;
    [SerializeField] private Collider2D wallCheckBox;
    [SerializeField] private Collider2D groundCheckBox;

    Transform player;

    [Header("�� ����")]
    [SerializeField] private Collider2D attackHitBox;
    [SerializeField] Transform fireBallMPos;
    [SerializeField] GameObject fireBallM;
    [SerializeField] Transform bossFirePos1;
    [SerializeField] Transform bossFirePos2;
    [SerializeField] Transform bossFirePos3;
    [SerializeField] Transform bossFirePos4;
    [SerializeField] Transform bossFirePos5;
    [SerializeField] Transform bossFirePos6;
    private float fireTimer = 4.0f;
    private float fireLimitTimer = 4.0f;
    private bool shootFireM = false;

    private Slider bossHp;
    private float patternTimer = 5.0f;
    private float pattenrLimitTime = 5.0f;
    private float timerDash = 0.0f;
    private float timerDashLimit = 0.5f;

    private int curPatten = -1;



    public enum bossPattern
    {
        Dash,
        Smash,
        Fire,
        Stop,
    }

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

        msFire();
        msFireCoolTime();

        reTurnHp();
        checkPattern();
    }


    /// <summary>
    /// ���� HP�� UI�� ��Ÿ���� �Լ�
    /// </summary>
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

    /// <summary>
    /// ���� �÷��̾� ���縦 üũ�ϴ� �Լ�
    /// </summary>
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
        msTurning();
    }

    /// <summary>
    /// ���� ���� �ϴ� ����
    /// </summary>
    private void checkTurn()
    {
        if (enemyType == enumEnemyType.Obstacle) return;
        if (enemyType == enumEnemyType.Mushroom) return;

        if (wallCheckBox.IsTouchingLayers(ground) == true)//wallcheckbox�� ���� ��Ҵٸ� ��
        {
            turning();
        }

        if (groundCheckBox.IsTouchingLayers(ground) == false)//groundcheckbox�� ������ �������ٸ� ��
        {
            turning();
        }

    }

    /// <summary>
    /// ���� �¿�� �����̴� �Լ�
    /// </summary>
    private void moving()
    {
        if (timerHit > 0.0f) return;
        if (enemyType == enumEnemyType.Obstacle) return;
        if (enemyType == enumEnemyType.Mushroom) return;
        if (timerDash > 0.0f && enemyType == enumEnemyType.Boss) return;//������ �뽬 ���� �� ����
        rigid.velocity = new Vector2(moveSpeed, rigid.velocity.y);
        if (enemyType == enumEnemyType.Boss)
        {
            if (curHp > maxHp/2)
            {
                rigid.velocity = new Vector2(-moveSpeed, rigid.velocity.y);
            }
            else if(curHp <= maxHp/2)
            {
                rigid.velocity = new Vector2(-moveSpeed * 2f, rigid.velocity.y);
                //Vector2 scale = gameObject.transform.localScale;
                //if (scale.x == 1)
                //{
                //    rigid.velocity = new Vector2(moveSpeed * 2f, rigid.velocity.y);
                //}
                //else if (scale.x == -1)
                //{
                //    rigid.velocity = new Vector2(-moveSpeed * 2f, rigid.velocity.y);
                //}
            }
        }
        if (curHp <= 0f)
        {
            rigid.velocity = Vector2.zero;
        }
    }

    /// <summary>
    /// �տ� ���� �ְų� ���� ���ٸ� ���ϴ� �Լ�
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
        if (enemyType == enumEnemyType.Mushroom && player != null)
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

    private void checkPattern()
    {
        if (enemyType != enumEnemyType.Boss) return;
        
        //� ������ ����Ұ��� ����
        int beforePatten = curPatten;
        while (beforePatten == curPatten)
        {
            curPatten = Random.Range(0, System.Enum.GetValues(typeof(bossPattern)).Length);
        }

        patternTimer -= Time.deltaTime;
        if (patternTimer <= 0f)
        {
            patternTimer = 0f;
        }

        switch ((bossPattern)curPatten)
        {
            case bossPattern.Dash:
                if (patternTimer == 0f)
                {
                    bossDash();
                    patternTimer = pattenrLimitTime;
                }
                break;

            case bossPattern.Smash:
                if (patternTimer == 0f)
                {
                    bossSmash();
                    patternTimer = pattenrLimitTime;
                }
                break;

            case bossPattern.Fire:
                if (patternTimer == 0f)
                {
                    bossFire();
                    patternTimer = pattenrLimitTime;
                }
                break;
            case bossPattern.Stop:
                if (patternTimer == 0f)
                {
                    bossStop();
                    patternTimer = pattenrLimitTime;
                }
                break;
        }
    }

    private void bossDash()
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
        moveSpeed = 0f;
        anim.SetTrigger("Smash");
        Invoke("reMove", 0.8f);

    }

    private void bossFire()
    {
        moveSpeed = 0f;
        anim.SetTrigger("BossFire");
        Invoke("shootBossFire", 0.5f);
        Invoke("reMove", 0.8f);

    }


    private void shootBossFire()
    {
        Instantiate(fireBallM, bossFirePos1.position, Quaternion.identity);
        Instantiate(fireBallM, bossFirePos2.position, Quaternion.identity);
        Instantiate(fireBallM, bossFirePos3.position, Quaternion.identity);
        Instantiate(fireBallM, bossFirePos4.position, Quaternion.identity);
        Instantiate(fireBallM, bossFirePos5.position, Quaternion.identity);
        Instantiate(fireBallM, bossFirePos6.position, Quaternion.identity);
    }

    private void bossStop()
    {
        moveSpeed = 0f;
        anim.SetTrigger("Stop");
        Invoke("reMove", 2f);
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
        Vector2 trsPlayer = player.transform.position;
        Vector2 enemyPos = gameObject.transform.position;
        if (trsPlayer.x <= enemyPos.x)
        {
            rigid.AddForce(new Vector2(1.5f, 2), ForceMode2D.Impulse);
        }
        else if (trsPlayer.x > enemyPos.x)
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
        if (timerHit > 0.0f) return;
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

    private void reTurnHp()
    {
        if (enemyType == enumEnemyType.Boss) return;
        if (enemyType == enumEnemyType.Obstacle) return;
        EnemyUI scEnemyUI = GetComponentInChildren<EnemyUI>();
        scEnemyUI.SetEnemyHp(curHp,maxHp);
    }

    //�θ��� ����� ���� ���� �� �θ��� ������ protect, �Լ��� vertual�� ���� �� �ڽ��� override�� base.���� ���� ����
}

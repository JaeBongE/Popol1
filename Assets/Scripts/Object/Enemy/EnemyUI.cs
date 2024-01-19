using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Enemy;

public class EnemyUI : MonoBehaviour
{
    [SerializeField] GameObject enemyPos;
    [SerializeField] Image enemyHp;


    void Update()
    {
        followEnemy();
    }

    private void followEnemy()
    {
        if (enemyPos.name == "Flying Eye")
        {
            gameObject.transform.position = enemyPos.transform.position + new Vector3(-0.1f, 0.4f, 0);
        }
        else if (enemyPos.name == "Mushroom")
        {
            gameObject.transform.position = enemyPos.transform.position + new Vector3(-0.1f, 0.7f, 0);
        }
        else if (enemyPos.name == "Skeleton")
        {
            gameObject.transform.position = enemyPos.transform.position + new Vector3(-0.1f, 0.8f, 0);
        }
    }

    public void SetEnemyHp(float _curHp, float _maxHp)
    {
        enemyHp.fillAmount = _curHp / _maxHp;
    }

}

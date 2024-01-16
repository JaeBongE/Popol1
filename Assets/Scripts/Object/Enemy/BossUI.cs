using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossUI : MonoBehaviour
{
    [SerializeField] Slider bossHp;

    public Slider getBossHp()
    {
        return bossHp;
    }
}

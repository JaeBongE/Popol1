using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] Slider playerHp;
    [Header("Èú")]
    [SerializeField] Image vCoolTime;
    [SerializeField] TMP_Text vCoolTimeText;

    //public (Image _VcoolTime, Slider _PlayerHp, TMP_Text _VCoolTimeText) GetProperty()
    //{
    //    return (vCoolTime, playerHp, vCoolTimeText);
    //}

    public Slider GetPlayerHp()
    {
        return playerHp;
    }

    public (Image _vCoolTime, TMP_Text _vCoolTimeText) GetVskill()
    {
        return (vCoolTime, vCoolTimeText);
    }
}

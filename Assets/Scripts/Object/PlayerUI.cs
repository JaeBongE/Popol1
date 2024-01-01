using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] Slider playerHp;
    [Header("힐")]
    [SerializeField] Image vCoolTime;
    [SerializeField] TMP_Text vCoolTimeText;
    [Header("게임오버메뉴")]
    [SerializeField] GameObject gameOverMenu;
    [SerializeField] Button retryButton;
    [SerializeField] Button exitButton;

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

    public GameObject ShowGameOverMenu()
    {
        return gameOverMenu;
    }

    public (Button _retryButton, Button _exitButton) ShowGameOverButton()
    {
        return (retryButton, exitButton);
    }
}

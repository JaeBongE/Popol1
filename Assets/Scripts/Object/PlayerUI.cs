using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] Slider playerHp;
    [SerializeField] TMP_Text textHp;
    [Header("대쉬")]
    [SerializeField] Image xCoolTime;
    [SerializeField] TMP_Text xCoolTimeText;
    [Header("파이어볼")]
    [SerializeField] Image cCoolTime;
    [SerializeField] TMP_Text cCoolTimeText;
    [Header("힐")]
    [SerializeField] Image vCoolTime;
    [SerializeField] TMP_Text vCoolTimeText;
    [Header("게임오버메뉴")]
    [SerializeField] GameObject gameOverMenu;
    [SerializeField] Button retryButton;
    [SerializeField] Button exitButton;
    [Header("HowToPlay")]
    [SerializeField] GameObject howToPlay;

    //public (Image _VcoolTime, Slider _PlayerHp, TMP_Text _VCoolTimeText) GetProperty()
    //{
    //    return (vCoolTime, playerHp, vCoolTimeText);
    //}

    public (Slider playerHp, TMP_Text textHp) GetPlayerHp()
    {
        return (playerHp, textHp);
    }

    public (Image _xCoolTime, TMP_Text _xCoolTimeText) GetXskill()
    {
        return (xCoolTime, xCoolTimeText);
    }

    public (Image _cCoolTime, TMP_Text _cCoolTimeText) GetCskill()
    {
        return (cCoolTime, cCoolTimeText);
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

    public GameObject getHowToPlay()
    {
        return howToPlay;
    }
}

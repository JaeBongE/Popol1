using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static NextSceneClass;
using UnityEngine.UI;

public class NextSceneClass : MonoBehaviour
{
    public static NextSceneClass Instance;
    private float playerCurHp;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);

        }

        DontDestroyOnLoad(gameObject);
    }


    public void setCurHp(float _curHp)
    {
        playerCurHp = _curHp;
    }

    public float getCurHp()
    {
        return playerCurHp;
    }
}

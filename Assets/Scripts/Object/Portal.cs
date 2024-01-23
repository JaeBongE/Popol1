using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Portal : MonoBehaviour
{
    [SerializeField] GameManager.enumScene toScene;
    GameManager gameManager;
    NextSceneClass nextSceneClass;
    BoxCollider2D boxColl;
    private float curPlayerHp;
    private void Awake()
    {
        boxColl = GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
        gameManager = GameManager.Instance;
        nextSceneClass = NextSceneClass.Instance;
    }

    //private void OnTriggerStay2D(Collider2D collision)
    //{
    //    if (Input.GetKeyDown(KeyCode.UpArrow) && collision.gameObject.tag == GameTag.Player.ToString())
    //    {
    //        gameManager.ToStage(toScene);
    //    }
    //    //if (Input.GetKeyDown(KeyCode.UpArrow) && collision.gameObject.tag == GameTag.Player.ToString())
    //    //{
    //    //    Player playerSc = GetComponentInParent<Player>();
    //    //    playerSc.ToStage(toScene);
    //    //}
    //}

    private void Update()
    {
        setPlayerHp();
        checkPlayer();
    }

    private void setPlayerHp()
    {
        GameObject objPlayerUI = GameObject.Find("PlayerUI");
        if (objPlayerUI == null) return;
        PlayerUI scUI = objPlayerUI.GetComponent<PlayerUI>();
        (Slider playerHp, TMP_Text textHp) HpData = scUI.GetPlayerHp();
        curPlayerHp = HpData.playerHp.value;

    }

    private void checkPlayer()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Collider2D playerColl = Physics2D.OverlapBox(boxColl.bounds.center, boxColl.bounds.size, 0, LayerMask.GetMask("BodyHitBox"));
            if (playerColl != null)
            {
                nextSceneClass.setCurHp(curPlayerHp);
                gameManager.ToStage(toScene);
            }
        }
    }
}

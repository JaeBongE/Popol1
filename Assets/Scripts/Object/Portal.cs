using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] GameManager.enumScene toScene;
    GameManager gameManager;
    BoxCollider2D boxColl;

    private void Awake()
    {
        boxColl = GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
        gameManager = GameManager.Instance;
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
        checkPlayer();
    }

    private void checkPlayer()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Collider2D playerColl = Physics2D.OverlapBox(boxColl.bounds.center, boxColl.bounds.size,0, LayerMask.GetMask("BodyHitBox"));
            if(playerColl != null) 
            {
                gameManager.ToStage(toScene);
            }
        }
    }
}

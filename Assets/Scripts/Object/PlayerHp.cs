using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHp : MonoBehaviour
{
    Slider slider;
    private float uiHp;
    Player player;
    void Awake()
    {
        slider = GetComponent<Slider>();

    }

    private void Start()
    {

    }

    void Update()
    {
      
    }

    public void SetPlayerHp(float _curHp, float _maxHp)
    {
    }
}

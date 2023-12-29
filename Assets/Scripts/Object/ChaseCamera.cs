using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseCamera : MonoBehaviour
{
    GameObject objPlayer;

    private void Awake()
    {
        objPlayer = GameObject.Find("Player");
    }

    private void Start()
    {
        
    }

    void Update()
    {
        if (objPlayer == null) return;

        Vector3 pos = objPlayer.transform.position;
        pos.z = -10;
        transform.position = pos;
    }
}

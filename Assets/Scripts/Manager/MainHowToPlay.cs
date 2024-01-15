using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainHowToPlay : MonoBehaviour
{
    [SerializeField] Button exitHowToPlay;


    void Update()
    {
        exitHowToPlay.onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
        });
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameObject.SetActive(false);
        }
    }
}

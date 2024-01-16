using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Victory : MonoBehaviour
{
    [SerializeField] Button goToMain;

    void Awake()
    {
        goToMain.onClick.AddListener(() =>
        {
            Time.timeScale = 1f;
            SceneManager.LoadSceneAsync(0);
        });
    }
}

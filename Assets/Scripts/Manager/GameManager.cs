using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using static GameManager;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public enum enumScene
    {
        Stage1,
        Stage2,
    }

    [SerializeField] GameObject gameOverMenu;
    [SerializeField] Button retryButton;
    [SerializeField] Button exitButton;
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
        gameOverMenu.SetActive(false);
        retryButton.onClick.AddListener(() =>
        {
            SceneManager.LoadSceneAsync((int)enumScene.Stage1);
            Time.timeScale = 1f;
        });
        exitButton.onClick.AddListener(() =>
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        });
    }

    private void Start()
    {
    }


    public void GameOver()
    {
        gameOverMenu.SetActive(true);
        Invoke("stopGame", 0.85f);
    }

    private void stopGame()
    {
        Time.timeScale = 0f;
    }
}


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

    GameObject gameOverMenu;
    Button retryButton;
    Button exitButton;

    Slider playerHp;

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
        //        gameOverMenu.SetActive(false);
        //        retryButton.onClick.AddListener(() =>
        //        {
        //            SceneManager.LoadSceneAsync((int)enumScene.Stage1);
        //            Time.timeScale = 1f;
        //        });
        //        exitButton.onClick.AddListener(() =>
        //        {
        //#if UNITY_EDITOR
        //            UnityEditor.EditorApplication.isPlaying = false;
        //#else
        //            Application.Quit();
        //#endif
        //        });
    }

    private void Update()
    {
        setGameOverMenu();
        getPlayerHp();
        //GameOver();
    }

    private void setGameOverMenu()
    {
        if (gameOverMenu == null)
        {
            GameObject objGameOverMenu = GameObject.Find("GameOverMenu");
            if (objGameOverMenu == null) return;

            GameOverMenu scGameOverMenu = objGameOverMenu.GetComponent<GameOverMenu>();
            gameOverMenu = scGameOverMenu.ShowGameOverMenu();
            (Button _retryButton, Button _exitButton) gameOverButton = scGameOverMenu.ShowGameOverButton();
            retryButton = gameOverButton._retryButton;
            exitButton = gameOverButton._exitButton;

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

        if (playerHp.value <= 0)
        {
            GameOver();
        }
    }
    private void getPlayerHp()
    {
        if (playerHp == null)
        {
            GameObject objPlayerUI = GameObject.Find("PlayerUI");
            if (objPlayerUI == null) return;
            PlayerUI scUI = objPlayerUI.GetComponent<PlayerUI>();
            playerHp = scUI.GetPlayerHp();
        }

    }

    private void GameOver()
    {
        gameOverMenu.SetActive(true);
        Invoke("stopGame", 0.85f);

    }

    private void stopGame()
    {
        Time.timeScale = 0f;
    }
}


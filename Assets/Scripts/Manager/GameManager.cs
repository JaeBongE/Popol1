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
        MainMenu,
        Stage1,
        Stage2,
        Stage3,
        Boss,
    }

    GameObject gameOverMenu;
    Button retryButton;
    Button exitButton;
    GameObject howToPlay;

    Slider playerHp;
    TMP_Text textHp;

    Slider bossHp;
    GameObject vicTory;
    [SerializeField] GameObject bossUI;

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
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 0f;
            howToPlay.SetActive(true);
        }
        getBossHp();
    }

    private void setGameOverMenu()
    {
        if (gameOverMenu == null)
        {
            GameObject objPlayerUI = GameObject.Find("PlayerUI");
            if (objPlayerUI == null) return;

            PlayerUI scUI = objPlayerUI.GetComponent<PlayerUI>();
            gameOverMenu = scUI.ShowGameOverMenu();
            (Button _retryButton, Button _exitButton) gameOverButton = scUI.ShowGameOverButton();
            retryButton = gameOverButton._retryButton;
            exitButton = gameOverButton._exitButton;
            howToPlay = scUI.getHowToPlay();
            howToPlay.SetActive(false);
            vicTory = scUI.getVictory();
            vicTory.SetActive(false);
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

    }
    private void getPlayerHp()
    {
        if (playerHp == null)
        {
            GameObject objPlayerUI = GameObject.Find("PlayerUI");
            if (objPlayerUI == null) return;
            PlayerUI scUI = objPlayerUI.GetComponent<PlayerUI>();
            (Slider playerHp, TMP_Text textHp) HpData = scUI.GetPlayerHp();
            playerHp = HpData.playerHp;
        }
        if (playerHp.value <= 0)
        {
            GameOver();
        }
    }

    private void getBossHp()
    {
        if (bossHp == null)
        {
            GameObject bossUI = GameObject.Find("BossUI");

            if (bossUI == null)
            {
                return;
            }
            BossUI scBossUI = bossUI.GetComponent<BossUI>();
            bossHp = scBossUI.getBossHp();
        }

        if (bossHp.value <= 0)
        {
            vicTory.SetActive(true);
            Invoke("stopGame", 1f);
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

    public void ToStage(enumScene _scene)
    {
        int nextStage = (int)_scene;
        SceneManager.LoadSceneAsync(nextStage);
    }
}


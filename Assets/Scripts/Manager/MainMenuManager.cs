using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField] Button startButton;
    [SerializeField] Button howToButton;
    [SerializeField] Button quitButton;
    [SerializeField] GameObject howToPlayMenu;
    [SerializeField] Button exitHowToPlay;
    NextSceneClass nextSceneClass;
    
    void Awake()
    {
        Time.timeScale = 1f;
        mainMenu();
    }

    private void Start()
    {
        nextSceneClass = NextSceneClass.Instance;
    }
    private void mainMenu()
    {
        startButton.onClick.AddListener(() =>
        {
            nextSceneClass.setCurHp(10f);
            SceneManager.LoadSceneAsync(1);
        });

        howToButton.onClick.AddListener(() =>
        {
            howToPlayMenu.SetActive(true);
        });

        quitButton.onClick.AddListener(() =>
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif

        });

    }
}

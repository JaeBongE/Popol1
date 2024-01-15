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
    
    void Awake()
    {
        mainMenu();
    }

    private void mainMenu()
    {
        startButton.onClick.AddListener(() =>
        {
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

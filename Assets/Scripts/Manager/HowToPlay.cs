using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HowToPlay : MonoBehaviour
{
    [SerializeField] Button exitHowToPlay;
    [SerializeField] Button quitGameButton;

    private void Awake()
    {
        goToMainMenu();
    }

    void Update()
    {
        exitHowToPlay.onClick.AddListener(() =>
        {
            Time.timeScale = 1f;
            gameObject.SetActive(false);
        });

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 1f;
            gameObject.SetActive(false);
        }
    }

    private void goToMainMenu()
    {
        quitGameButton.onClick.AddListener(() =>
        {
            Time.timeScale = 1f;
            SceneManager.LoadSceneAsync(0);
        });
    }

}

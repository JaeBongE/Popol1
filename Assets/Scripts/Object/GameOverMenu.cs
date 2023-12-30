using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameOverMenu : MonoBehaviour
{
    [SerializeField] GameObject gameOverMenu;
    [SerializeField] Button retryButton;
    [SerializeField] Button exitButton;

    public GameObject ShowGameOverMenu()
    {
        return gameOverMenu;
    }

    public (Button _retryButton, Button _exitButton) ShowGameOverButton()
    {
        return (retryButton, exitButton);
    }
}

using HmsPlugin;
using HuaweiMobileServices.Game;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameStarterScript : MonoBehaviour
{
    public GameObject gameOverPanel;

    public void PlayGame()
    {
        if (Time.timeScale == 0f)
        {
            Time.timeScale = 1f;
        }
        
        checkAndFixPanel();
        
        if (HMSAccountManager.Instance.IsSignedIn)
            SceneManager.LoadScene("Game");
        else
        {
            print("Not signed in!");
        }

        GameManager.score = 0;
        GameManager.rockCount = 5;
    }

    public void PlayGameWithParameters(int score = 0, int rockcount = 5)
    {
        if (Time.timeScale == 0f)
        {
            Time.timeScale = 1f;
        }

        checkAndFixPanel();

        if (HMSAccountManager.Instance.IsSignedIn)
            SceneManager.LoadScene("Game");
        else
        {
            print("Not signed in!");
        }

        GameManager.score = score;
        GameManager.rockCount = rockcount;
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void ShowAchievements()
    {
        HMSAchievementsManager.Instance.ShowAchievements();
    }

    public void ShowLeaderboards()
    {
        HMSLeaderboardManager.Instance.ShowLeaderboards();
    }

    public void ShowSavedGames()
    {
        HMSSaveGameManager.Instance.ShowArchive();
    }

    public void PauseStarter()
    {
        if (Time.timeScale == 0f)
        {
            Time.timeScale = 1f;
        }
        else
        {
            Time.timeScale = 0f;
        }
        checkAndFixPanel();
    }

    void checkAndFixPanel()
    {
        if (SceneManager.GetActiveScene().name == "Game")
        {
            if (!gameOverPanel.activeSelf)
            {
                gameOverPanel.SetActive(true);
            }
            else
            {
                gameOverPanel.SetActive(false);
            }
        }
    }

    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonFunctions : MonoBehaviour
{
    public void LoadScene(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void OpenLeaderboard()
    {
        ScoreManager.instance.displayLocalScores();
    }

    public void OpenGlobalLeaderboard()
    {
        ScoreManager.instance.DisplayGlobalScores();
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
    }

    public void togglePanel(GameObject panel)
    {
        panel.SetActive(!panel.activeInHierarchy);
    }
}

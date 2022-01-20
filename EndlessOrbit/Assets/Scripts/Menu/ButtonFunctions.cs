using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonFunctions : MonoBehaviour
{
    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        PlayerManager.instance.AddGamesPlayed();
        Time.timeScale = 1f;
    }

    public void LoadScene()
    {
        SceneManager.LoadScene(TitleScreenManager.instance.GetMode());
        PlayerManager.instance.AddGamesPlayed();
        Time.timeScale = 1f;
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1f;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void OpenShopInventory()
    {
        PlayerManager.instance.OpenShopInventory();
    }

}

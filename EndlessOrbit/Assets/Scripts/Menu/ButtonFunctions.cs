using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonFunctions : MonoBehaviour
{
    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadScene()
    {
        SceneManager.LoadScene(TitleScreenManager.instance.GetMode());
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void OpenShopInventory()
    {
        PlayerManager.instance.OpenShopInventory();
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonFunctions : MonoBehaviour
{
    public void LoadScene()
    {
        SceneManager.LoadScene(TitleScreenManager.instance.GetMode());
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("Menu");
    }



}

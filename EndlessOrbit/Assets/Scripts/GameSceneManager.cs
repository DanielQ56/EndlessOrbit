using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    public static GameSceneManager sceneManager;

    void Awake()
    {
        if (sceneManager == null)
        {
            DontDestroyOnLoad(gameObject);
            sceneManager = this;
        }
        else if (sceneManager != this)
        {
            Destroy(gameObject);
        }
    }

    void LoadSceneByIndex(int index)
    {
        SceneManager.LoadScene(index);
    }

    public void LoadPlayScene()
    {
        LoadSceneByIndex(1);
    }

    public void LoadTitleScreen()
    {
        LoadSceneByIndex(0);
    }
}
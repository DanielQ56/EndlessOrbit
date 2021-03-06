﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum BackGesture
{
    PopupPanel,
    Pause,
    Game,
    Menu
}

public class BackGestureComponent : MonoBehaviour
{
    [SerializeField] BackGesture GestureType;

    bool CanGoBack = true;

    private void OnEnable()
    {
        //Debug.Log("Adding " + this.gameObject.name + " to Android Manager List");
        AndroidManager.instance.AddComponentToList(this);
    }

    public void GoBack()
    {
        if (CanGoBack)
        {
            switch (GestureType)
            {
                case BackGesture.Pause:
                    MainGameManager.instance.PauseGame();
                    break;
                case BackGesture.PopupPanel:
                    this.gameObject.SetActive(false);
                    break;
                case BackGesture.Game:
                    Debug.Log("Back gesture type GAME");
                    this.GetComponent<ButtonFunctions>().LoadMenu();
                    break;
                case BackGesture.Menu:
                    TitleScreenManager.instance.TryToExit();
                    break;
            }
        }
    }

    public void CanUseBackGesture(bool b)
    {
        CanGoBack = b;
    }

    private void OnDisable()
    {
        Debug.Log("Removing " + this.gameObject.name + " to Android Manager List");
        AndroidManager.instance.RemoveComponentFromList(this);
    }
}

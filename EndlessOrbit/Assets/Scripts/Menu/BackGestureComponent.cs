using System.Collections;
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

    private void OnEnable()
    {
        AndroidManager.instance.AddComponentToList(this);
    }

    public void GoBack()
    {
        switch(GestureType)
        {
            case BackGesture.Pause:
                MainGameManager.instance.PauseGame();
                break;
            case BackGesture.PopupPanel:
                this.gameObject.SetActive(false);
                break;
            case BackGesture.Game:
                this.GetComponent<ButtonFunctions>().LoadMenu();
                break;
            case BackGesture.Menu:
                break;
        }
    }

    private void OnDisable()
    {
        AndroidManager.instance.RemoveComponentFromList(this);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreenManager : MonoBehaviour
{

    [SerializeField] GameObject TitleScreen;
    [SerializeField] GameObject ControlsScreen;
    [SerializeField] TutorialPlayer player;

    private void Start()
    {
        TitleScreen.SetActive(true);
        ControlsScreen.SetActive(false);
    }

    public void toggleTitleScreen()
    {
        TitleScreen.SetActive(!TitleScreen.activeInHierarchy);
    }

    public void toggleControlsScreen()
    {
        ControlsScreen.SetActive(!ControlsScreen.activeInHierarchy);
        player.ToggleTesting();
    }
}

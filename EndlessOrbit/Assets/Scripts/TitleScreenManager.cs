using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreenManager : MonoBehaviour
{

    [SerializeField] GameObject TitleScreen;
    [SerializeField] GameObject ControlsScreen;
    [SerializeField] GameObject ScoresScreen;

    // Start is called before the first frame update
    void Start()
    {

    }
    
    public void toggleTitleScreen()
    {
        TitleScreen.SetActive(!TitleScreen.activeSelf);
    }

    public void toggleControlsScreen()
    {
        ControlsScreen.SetActive(!ControlsScreen.activeSelf);
    }

    public void toggleScoresScreen()
    {
        ScoresScreen.SetActive(!ScoresScreen.activeSelf);
    }
}

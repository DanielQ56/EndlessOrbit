using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TitleScreenManager : MonoBehaviour
{
    public static TitleScreenManager instance = null;

    [SerializeField] TextMeshProUGUI currentMode;

    string selectedmode = "Normal";

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }


    public void UpdateModeText()
    {
        if(selectedmode == "Normal")
        {
            selectedmode = "Unstable";
        }
        else
        {
            selectedmode = "Normal";
        }
        currentMode.text = selectedmode;
    }

    public string GetMode()
    {
        return selectedmode;
    }
}

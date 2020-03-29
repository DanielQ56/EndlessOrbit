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
        GoogleAds.instance.RequestBanner();
    }

    public void UpdateModeText()
    {
        currentMode.text = selectedmode;
    }

    public void ChangeMode(string mode)
    {
        selectedmode = mode;
    }

    public string GetMode()
    {
        return selectedmode;
    }
}

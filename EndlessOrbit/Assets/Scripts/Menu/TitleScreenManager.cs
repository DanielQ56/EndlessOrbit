﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TitleScreenManager : MonoBehaviour
{
    public static TitleScreenManager instance = null;


    string selectedmode = "Normal";

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }


    public void UpdateMode(string mode)
    {
        selectedmode = mode;
    }

    public string GetMode()
    {
        return selectedmode;
    }
}

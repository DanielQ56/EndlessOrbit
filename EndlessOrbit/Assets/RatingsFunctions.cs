using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatingsFunctions : MonoBehaviour
{
    private string googlePlayID = "com.StudioNiku.EndlessOrbit";
    private string appleStoreID = "";

    private string googlePlayURL = "market://details?id=";
    private string appleStoreURL = "";

    public void OpenStoreURL ()
    {
        Debug.Log("Opening store URL");
        Application.OpenURL(GetStoreURL());
    }

    private string GetStoreURL ()
    {
        return googlePlayURL + googlePlayID;
    }
}
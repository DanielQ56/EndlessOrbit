using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreenManager : MonoBehaviour
{

    private void Awake()
    {
        GoogleAds.instance.RequestBanner();
    }
}

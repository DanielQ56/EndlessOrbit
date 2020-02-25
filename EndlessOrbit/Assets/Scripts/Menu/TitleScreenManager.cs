using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreenManager : MonoBehaviour
{

    private void Start()
    {
        GoogleAds.instance.RequestBanner();
    }
}

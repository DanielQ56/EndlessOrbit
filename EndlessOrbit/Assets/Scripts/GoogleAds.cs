using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class GoogleAds : MonoBehaviour
{
    public static GoogleAds instance;

    string appID = "ca-app-pub-1176054287451959~9608087394";

    private BannerView bannerView;
    private string bannerID = "	ca-app-pub-3940256099942544/6300978111";

    private InterstitialAd fullScreenAd;
    private string fullScreenAdID = "	ca-app-pub-3940256099942544/1033173712";

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        MobileAds.Initialize(appID);
        RequestFullScreenAd();
        
    }

    public void RequestBanner()
    {
        bannerView = new BannerView(bannerID, AdSize.Banner, AdPosition.Bottom);

        AdRequest request = new AdRequest.Builder().Build();
        foreach(string s in request.TestDevices)
        {
            Debug.Log(s);
        }
        bannerView.LoadAd(request);
        bannerView.Show();
        Debug.Log("Requesting banner ad");
    }

    public void HideBanner()
    {
        bannerView.Hide();
    }

    public void RequestFullScreenAd()
    {
        fullScreenAd = new InterstitialAd(fullScreenAdID);

        AdRequest request = new AdRequest.Builder().Build();

        fullScreenAd.LoadAd(request);


    }

    public void ShowFullScreenAd()
    {
        if(fullScreenAd.IsLoaded())
        {
            fullScreenAd.Show();
            RequestFullScreenAd();
        }
        else
        {
            Debug.Log("Full screen ad not loaded");
            RequestFullScreenAd();
        }
    }
}

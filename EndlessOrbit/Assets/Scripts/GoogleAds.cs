using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using System;

public class GoogleAds : MonoBehaviour
{
    public static GoogleAds instance;

    string appID = "ca-app-pub-1176054287451959~9608087394";

    private BannerView bannerView;
    private string bannerID = "ca-app-pub-3940256099942544/6300978111";

    private InterstitialAd fullScreenAd;
    private string fullScreenAdID = "ca-app-pub-3940256099942544/1033173712";

    private RewardBasedVideoAd rewardedAd;
    private string rewardedAdID = "	ca-app-pub-3940256099942544/5224354917";

    bool showAds = true;

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
        if (showAds)
        {
            MobileAds.Initialize(appID);
            RequestFullScreenAd();

            rewardedAd = RewardBasedVideoAd.Instance;

            rewardedAd.OnAdLoaded += HandleRewardBasedVideoLoaded;
            rewardedAd.OnAdFailedToLoad += HandleRewardBasedVideoFailedToLoad;
            rewardedAd.OnAdRewarded += HandleRewardBasedVideoRewarded;
            rewardedAd.OnAdClosed += HandleRewardBasedVideoClosed;


            RequestRewardedAd();
        }      
    }

    #region Reward Ad
    public void RequestRewardedAd()
    {
        AdRequest request = new AdRequest.Builder().Build();
        rewardedAd.LoadAd(request, rewardedAdID);
    }

    public void ShowRewardedAd()
    {
        if(rewardedAd.IsLoaded())
        {
            rewardedAd.Show();
        }
        else
        {
            Debug.Log("Rewarded ad not loaded");
        }
    }

    public void HandleRewardBasedVideoLoaded(object sender, EventArgs args)
    {

    }

    public void HandleRewardBasedVideoFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {

    }

    public void HandleRewardBasedVideoRewarded(object sender, Reward args)
    {
        
    }

    public void HandleRewardBasedVideoClosed(object sender, EventArgs args)
    {

    }

    #endregion

    #region Banner
    public void RequestBanner()
    {
        bannerView = new BannerView(bannerID, AdSize.Banner, AdPosition.Bottom);

        AdRequest request = new AdRequest.Builder().Build();

        bannerView.LoadAd(request);
        bannerView.Show();
    }

    public void HideBanner()
    {
        bannerView.Hide();
    }

    #endregion

    #region Fullscreen
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
    #endregion

}

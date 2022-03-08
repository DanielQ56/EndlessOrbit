using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RatingsFunctions : MonoBehaviour
{
    private string googlePlayID = "com.StudioNiku.EndlessOrbit";
    private string appleStoreID = "";

    private string googlePlayURL = "market://details?id=";
    private string appleStoreURL = "";

    [SerializeField] GameObject RatingPanel;
    [SerializeField] GameObject ThankYouPanel;
    [SerializeField] Button Ratings;
    [SerializeField] Button Rated;
    [SerializeField] Button Thanks;


    private void Awake()
    {
        //Ratings.onClick.AddListener(OpenRatingsPanel);
        Rated.onClick.AddListener(RateProcedure);
        Thanks.onClick.AddListener(ThankYouProcedure);
    }

    /***
    void OpenRatingsPanel()
    {
        Debug.Log("RatingsTest");
        if (ScoreManager.instance.HasNotRated())
        {
            RatingPanel.SetActive(true);
            ThankYouPanel.SetActive(false);
        }
        else
        {
            ThankYouPanel.SetActive(true);
            RatingPanel.SetActive(false);
        }
    }
    ***/

    void RateProcedure()
    {
        OpenStoreURL();
        SwapPanels();
        PlayerManager.instance.AddStars(100);
    }

    void ThankYouProcedure()
    {
        Achievements.instance.Rate();
    }

    private void OnDestroy()
    {
        //Ratings.onClick.RemoveListener(OpenRatingsPanel);
        Rated.onClick.RemoveListener(RateProcedure);
        Thanks.onClick.RemoveListener(ThankYouProcedure);
    }

    public void OpenStoreURL()
    {
        Application.OpenURL(GetStoreURL());
    }

    private string GetStoreURL()
    {
        return googlePlayURL + googlePlayID;
    }

    public void SwapPanels()
    {
        RatingPanel.SetActive(false);
        ThankYouPanel.SetActive(true);
        ScoreManager.instance.SwapRatings();
    }

    public void OpenRatingsPanel()
    {
        //Debug.Log("OpenRatingTest: " + ScoreManager.instance.HasNotRated());
        if (ScoreManager.instance.HasNotRated())
        {
            RatingPanel.SetActive(true);
            ThankYouPanel.SetActive(false);
        }
        else
        {
            ThankYouPanel.SetActive(true);
            RatingPanel.SetActive(false);
        }
    }
}
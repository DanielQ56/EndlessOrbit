using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HowToPlayButton : MonoBehaviour
{
    private void Awake()
    {
        this.GetComponent<Button>().onClick.AddListener(DisplayHowToPlay);
    }

    void DisplayHowToPlay()
    {
        ScoreManager.instance.OpenHowToPlay();
    }

    private void OnDestroy()
    {
        this.GetComponent<Button>().onClick.RemoveListener(DisplayHowToPlay);
    }
}

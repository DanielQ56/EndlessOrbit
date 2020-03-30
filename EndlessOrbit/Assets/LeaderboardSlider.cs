using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class LeaderboardSlider : MonoBehaviour
{
    Slider slider;

    private void Awake()
    {
        slider = this.GetComponent<Slider>();
        slider.onValueChanged.AddListener(ChangeLeaderboards);
    }

    private void OnEnable()
    {
        slider.value = slider.minValue;
    }

    void ChangeLeaderboards(float value)
    {
        ScoreManager.instance.ChangeLeaderboards((int)value, (MainGameManager.instance != null ? MainGameManager.instance.isUnstableMode(): false));
    }


    private void OnDestroy()
    {
        slider.onValueChanged.RemoveListener(ChangeLeaderboards);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementsDisplay : MonoBehaviour
{
    [SerializeField] Button Checkbox;

    private void Awake()
    {
        Checkbox.onClick.AddListener(OpenAchievements);
    }

    void OpenAchievements()
    {
        Achievements.instance.DisplayAchievements();
    }

    private void OnDestroy()
    {
        Checkbox.onClick.RemoveListener(OpenAchievements);
    }
}

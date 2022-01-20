using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class StatsDisplay : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI gamesPlayed;
    [SerializeField] TextMeshProUGUI orbitsTraversed;
    [SerializeField] TextMeshProUGUI starsEarned;
    [SerializeField] TextMeshProUGUI starsSpent;

    private void OnEnable()
    {
        UpdateText();
    }

    void UpdateText()
    {
        gamesPlayed.text = PlayerManager.instance.GetGamesPlayed().ToString();
        orbitsTraversed.text = PlayerManager.instance.GetOrbitsTraversed().ToString();
        starsEarned.text = PlayerManager.instance.GetSilverStarsTotal().ToString();
        starsSpent.text = (PlayerManager.instance.GetSilverStarsTotal() - PlayerManager.instance.GetSilverStars()).ToString();
    }

}

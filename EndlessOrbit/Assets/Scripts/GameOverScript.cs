using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameOverScript : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] GameObject scoreLine;

    public void GameOver(int score)
    {
        //AUDIO (can move)
        if (!AudioManager.instance.muted)
            AudioManager.instance.Play("GameOver");

        gameOverPanel.SetActive(true);
        scoreText.text = score.ToString();
        scoreLine.SetActive(false);
    }
}

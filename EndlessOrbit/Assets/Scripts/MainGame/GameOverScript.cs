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

        if(gameOverPanel != null)
            gameOverPanel.SetActive(true);
        if(scoreText != null)
            scoreText.text = score.ToString();
        if(scoreLine != null)
            scoreLine.SetActive(false);
    }
}

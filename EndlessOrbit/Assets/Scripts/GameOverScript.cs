﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameOverScript : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] GameObject gameOverPanel;

    public void GameOver(int score)
    {
        gameOverPanel.SetActive(true);
        scoreText.text = score.ToString();
    }
}
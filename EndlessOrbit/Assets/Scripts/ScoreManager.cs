using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager scoreManager;

    int[] scores = new int[10];
    
    void Awake()
    {
        if (scoreManager == null)
        {
            DontDestroyOnLoad(gameObject);
            scoreManager = this;
        }
        else if (scoreManager != this)
        {
            Destroy(gameObject);
        }
        LoadScores();

        // temp
        /*scores[0] = 20;
        scores[1] = 5;
        scores[2] = 100;
        scores[3] = 50;
        scores[4] = 5;
        scores[5] = 500;
        scores[6] = 550;
        scores[7] = 300;
        scores[8] = 80;
        scores[9] = 25;
        Array.Sort(scores);*/

        Debug.Log(Application.persistentDataPath);
    }

    void LoadScores()
    {
        if(File.Exists(Application.persistentDataPath + "/scores.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "scores.dat", FileMode.Open);

            Scores topScores = (Scores)bf.Deserialize(file);
            file.Close();

            scores = topScores.scores;
        }
    }

    public void SaveScore(int score)
    {
        if (score <= scores[0])
        {
            return;
        }

        scores[0] = score;
        Array.Sort(scores);

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/scores.dat", FileMode.Create);

        Scores topScores = new Scores();
        topScores.scores = scores;

        bf.Serialize(file, topScores);
        file.Close();
    }

    public void displayScores(Text scoreText)
    {
        String s = "";
        for (int i = 9; i >= 0; i--)
        {
            s += $"{(10 - i)} -- {scores[i]}\n\n";
        }

        scoreText.text = s;
    }
}

[Serializable]
class Scores
{
    public int[] scores = new int[10];
}
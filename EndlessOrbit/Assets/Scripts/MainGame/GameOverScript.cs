using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameOverScript : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreAmount;
    [SerializeField] TextMeshProUGUI scoreHeader;
    [SerializeField] TextMeshProUGUI highScoreText;
    [SerializeField] TextMeshProUGUI starsAmount;
    [SerializeField] TextMeshProUGUI starsHeader;
    [SerializeField] GameObject StarIcon;
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] GameObject otherUI;
    [SerializeField] GameObject scoreLine;
    [SerializeField] AudioClip GameoverEffect;

    bool skip = false;


    private void Start()
    {
        gameOverPanel.gameObject.SetActive(false);
    }

    public void GameOver(int score, int collectedStars, bool isUnstable)
    {
        AudioManager.instance.PlayEffect(GameoverEffect);

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
            StarIcon.SetActive(false);
            highScoreText.gameObject.SetActive(false);
            starsAmount.gameObject.SetActive(false);
            starsHeader.gameObject.SetActive(false);
            otherUI.SetActive(false);
            StartCoroutine(TallyScore(score, collectedStars,isUnstable));
        }
        if(scoreLine != null)
            scoreLine.SetActive(false);
    }

    private void Update()
    {
        if((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) || Input.GetKeyDown(KeyCode.Mouse0))
        {
            skip = true;
        }
    }

    IEnumerator TallyScore(int score, int collectedStars, bool isUnstable)
    {
        skip = false;
        int tempScore = 0;
        int incAmount = score / 20;
        scoreAmount.text = tempScore.ToString();
        yield return new WaitForSeconds(0.2f);
        while(tempScore < score && !skip)
        {
            scoreAmount.text = tempScore.ToString();
            yield return null;
            tempScore += incAmount;
        }
        scoreAmount.text = score.ToString();
        highScoreText.gameObject.SetActive(true);
        if (score > ScoreManager.instance.GetHighScore(isUnstable))
        {
            scoreHeader.text = "New High Score!";
        }
        else
        {
            scoreHeader.text = "Score:";
        }
        ScoreManager.instance.RecordScore(score, isUnstable);
        highScoreText.text = "Highest Score: " + ScoreManager.instance.GetHighScore(isUnstable).ToString();
        StartCoroutine(TallyStars(collectedStars));
    }

    IEnumerator TallyStars(int collectedStars)
    {
        skip = false;
        starsAmount.gameObject.SetActive(true);
        starsHeader.gameObject.SetActive(true);
        StarIcon.SetActive(true);
        int currStars = PlayerManager.instance.GetSilverStars();
        int newAmount = currStars + collectedStars;
        starsAmount.text = currStars.ToString();
        yield return new WaitForSeconds(0.2f);
        while(currStars < newAmount && !skip)
        {
            starsAmount.text = (++currStars).ToString();
            yield return new WaitForSeconds(0.1f);
        }
        starsAmount.text = newAmount.ToString();
        PlayerManager.instance.AddStars(collectedStars);
        otherUI.SetActive(true);
    }

}

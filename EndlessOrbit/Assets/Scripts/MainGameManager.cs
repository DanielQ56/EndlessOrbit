using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class MainGameManager : MonoBehaviour
{
    [SerializeField] Camera mainCam;
    [SerializeField] Text scoreText;
    [SerializeField] Transform planetParent;
    [SerializeField] Transform startingPlanet;
    [SerializeField] List<GameObject> planets;
    [Range(1, 3)] public int maxNumOfPlanets;
    [SerializeField] GameOverScript gameOver;
    [SerializeField] GameObject PausePanel;
    [SerializeField] ParticleSystem playerParticles;
    [SerializeField] HighScoreLine score;


    public UnityEvent increaseSpeed;

    public static MainGameManager instance;

    int currentScore = 0;

    bool movingCamera = false;

    bool playerIsAlive = true;

    bool SpeedIncreased = false;

    bool displayHighScoreLine = false;

    GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        increaseSpeed = new UnityEvent();
        if(instance == null)
        {
            instance = this;
        }

        updateCameraPosition(startingPlanet);
    }

    private void Update()
    {
        if (playerIsAlive)
        {
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
            {
                PauseGame();
            }
        }

    }


    public void PauseGame()
    {
        Time.timeScale = 1 - Time.timeScale;
        PausePanel.SetActive(!PausePanel.activeInHierarchy);
    }

    public void GameOver()
    {
        playerIsAlive = false;
        gameOver.GameOver(currentScore);
        ScoreManager.instance.SaveScore(currentScore);
    }

    public void AttachedToNewPlanet(Transform newPlanet)
    {
        Debug.Log("attached");
        updateCameraPosition(newPlanet);
        UpdateScore(100);
        IncreasePlayerSpeed();
    }

    public bool IsMovingCamera()
    {
        return movingCamera;
    }

    void IncreasePlayerSpeed()
    {
        if(currentScore > 0 && currentScore % 1000 == 0)
        {
            increaseSpeed.Invoke();
            playerParticles.gameObject.transform.position = player.transform.position - new Vector3(0, 0.2f, 0);
            ParticleSystem.MainModule main = playerParticles.main;
            main.startColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
            playerParticles.Play();
        }
    }

    void UpdateScore(int value)
    {
        currentScore += value;
        scoreText.text = currentScore.ToString();
        if (currentScore == ScoreManager.instance.GetHighScore() && currentScore > 0)
            displayHighScoreLine = true;
        SpeedIncreased = false;
    }

    void updateCameraPosition(Transform newPlanet)
    {
        StartCoroutine(UpdateCamPos(newPlanet));
    }

    void GenerateNextPlanet(Transform newPlanet)
    {
        if (planetParent.childCount == 1)
        {
            int numPlanetsToSpawn = 1;
            float width = mainCam.orthographicSize * ((float)Screen.width / (float)Screen.height) - 3;
            float gap = ((width * 2) / numPlanetsToSpawn);
            for (int i = 0; i < numPlanetsToSpawn; ++i)
            {
                GameObject clone = Instantiate(planets[Random.Range(0, planets.Count)], planetParent);
                clone.transform.position = new Vector3(Random.Range(-width + gap * i, -width + gap * (i + 1)),
                    Random.Range(mainCam.transform.position.y + mainCam.orthographicSize/3, mainCam.transform.position.y + mainCam.orthographicSize -1), 0);

            }
        }
    }


    IEnumerator UpdateCamPos(Transform newPlanet)
    {
        movingCamera = true;
        Vector3 pos = new Vector3(0, newPlanet.position.y + mainCam.orthographicSize, -10);
        while (Mathf.Abs(mainCam.transform.position.y - pos.y) > 3)
        {
            mainCam.transform.position = Vector3.Lerp(mainCam.transform.position, pos, 3 * Time.deltaTime);
            yield return null;
        }
        GenerateNextPlanet(newPlanet);
        movingCamera = false;
        if(displayHighScoreLine)
        {
            score.gameObject.SetActive(true);
            displayHighScoreLine = false;
        }

    }


    public void PassedHighScore()
    {
        if(score.gameObject.activeInHierarchy)
        {
            score.HighScore();
        }
    }




}

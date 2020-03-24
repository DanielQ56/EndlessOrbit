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
    [SerializeField] ParticleSystem playerParticles;
    [SerializeField] HighScoreLine score;
    [SerializeField] GameObject asteroidPrefab;
    [SerializeField] float xOffset;
    [SerializeField] float yOffset;


    public UnityEvent increaseSpeed;

    public static MainGameManager instance;


    bool movingCamera = false;

    bool playerIsAlive = true;

    bool SpeedIncreased = false;

    bool displayHighScoreLine = false;

    float width, height;

    GameObject player;


    #region Setup

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        increaseSpeed = new UnityEvent();
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        SetBounds();
        GoogleAds.instance.HideBanner();
    }

    private void Start()
    {
        updateCameraPosition(startingPlanet);
        currentGStars = 0;
        currentSStars = 0;
        timer = asteroidTimer;
    }

    void SetBounds()
    {
        width = mainCam.ScreenToWorldPoint(new Vector2(mainCam.pixelWidth, mainCam.pixelHeight)).x - mainCam.ScreenToWorldPoint(Vector2.zero).x;
        height = (mainCam.ScreenToWorldPoint(new Vector2(mainCam.pixelWidth, mainCam.pixelHeight)).y - mainCam.ScreenToWorldPoint(Vector2.zero).y)/2;
    }

    #endregion

    #region In Game Routine

    [SerializeField] float asteroidTimer;
    float timer;

    bool timerPaused = false;

    private void Update()
    {
        if (playerIsAlive)
        {
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
            {
                PauseGame();
            }
            AsteroidCycle();

        }

    }

    void AsteroidCycle()
    {
        if (timer >= 0f)
        {
            if (!timerPaused)
                timer -= Time.deltaTime;
        }
        else
        {
            SpawnAsteroid();
            timer = asteroidTimer;
        }
    }

    void SpawnAsteroid()
    {
        GameObject asteroid = Instantiate(asteroidPrefab);
        float asteroidHeight = asteroid.GetComponent<CircleCollider2D>().radius * asteroid.transform.localScale.x;
        Vector3 startPos, endPos;
        if(Random.value < 0.51f)
        {
            startPos = new Vector3(mainCam.transform.position.x - (width / 2), Random.Range(planetParent.GetChild(0).transform.position.y + asteroidHeight, planetParent.GetChild(1).transform.position.y - asteroidHeight));
            endPos = new Vector3(mainCam.transform.position.x + (width / 2), Random.Range(planetParent.GetChild(0).transform.position.y + asteroidHeight, planetParent.GetChild(1).transform.position.y - asteroidHeight));
        }
        else
        {
            startPos = new Vector3(mainCam.transform.position.x + (width / 2), Random.Range(planetParent.GetChild(0).transform.position.y + asteroidHeight, planetParent.GetChild(1).transform.position.y - asteroidHeight));
            endPos = new Vector3(mainCam.transform.position.x - (width / 2), Random.Range(planetParent.GetChild(0).transform.position.y + asteroidHeight, planetParent.GetChild(1).transform.position.y - asteroidHeight));
        }
        asteroid.GetComponent<Asteroid>().CreateAsteroid(startPos, endPos);
    }

    #endregion

    #region Time
    [SerializeField] GameOverScript gameOver;
    [SerializeField] GameObject PausePanel;

    public void PauseGame()
    {
        Time.timeScale = 1 - Time.timeScale;
        PausePanel.SetActive(!PausePanel.activeInHierarchy);
    }

    public void GameOver()
    {
        playerIsAlive = false;
        gameOver.GameOver(currentScore);
        ScoreManager.instance.RecordScore(currentScore);
        PlayerManager.instance.AddStars(currentGStars, currentSStars);
        GoogleAds.instance.ShowFullScreenAd();
    }
    #endregion

    #region Hit Planet

    public void AttachedToNewPlanet(Transform newPlanet)
    {
        updateCameraPosition(newPlanet);
        UpdateScore(100);
        AddCoins();
        IncreasePlayerSpeed();
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
    #endregion

    #region Camera Movement

    public bool IsMovingCamera()
    {
        return movingCamera;
    }

    void updateCameraPosition(Transform newPlanet)
    {
        StartCoroutine(UpdateCamPos(newPlanet));
    }

    IEnumerator UpdateCamPos(Transform newPlanet)
    {
        timerPaused = true;
        movingCamera = true;
        float botY = mainCam.ScreenToWorldPoint(Vector2.zero).y;
        Vector3 pos = new Vector3(0, newPlanet.position.y + height, -10);
        while (Mathf.Abs(mainCam.transform.position.y - pos.y) > 3)
        {
            mainCam.transform.position = Vector3.Lerp(mainCam.transform.position, pos, 3 * Time.deltaTime);
            yield return null;
        }
        GenerateNextPlanet(newPlanet);
        movingCamera = false;
        timerPaused = false;
        if (displayHighScoreLine)
        {
            score.gameObject.SetActive(true);
            displayHighScoreLine = false;
        }

    }

    #endregion

    #region Planet Generation
    void GenerateNextPlanet(Transform newPlanet)
    {
        if (planetParent.childCount == 1)
        {
            int numPlanetsToSpawn = 1;
            Vector3 camPos = mainCam.transform.position;
            for (int i = 0; i < numPlanetsToSpawn; ++i)
            {
                GameObject clone = Instantiate(planets[Random.Range(0, planets.Count)], planetParent);
                DetermineOffset(clone.transform);
                clone.transform.position = new Vector3(Random.Range((camPos.x - width / 2) + xOffset, (camPos.x + width / 2) - xOffset),
                    Random.Range(camPos.y + yOffset, camPos.y + height - yOffset), 0);
                //clone.transform.position = new Vector3(Random.Range(-width + gap * i, -width + gap * (i + 1)),
                //Random.Range(mainCam.transform.position.y + mainCam.orthographicSize/3, mainCam.transform.position.y + mainCam.orthographicSize -1), 0);

            }
        }
    }

    void DetermineOffset(Transform planet)
    {
        switch (planet.transform.localScale.x)
        {
            case 18:
                xOffset = 2.6f;
                break;
            case 16:
                xOffset = 2.6f;
                break;
            case 14:
                xOffset = 2.4f;
                break;
            case 12:
                xOffset = 2.4f;
                break;
            case 8:
                xOffset = 1.8f;
                break;
        }
    }

    #endregion

    #region Score/Currency
    int currentScore = 0;
    int currentGStars = 0;
    int currentSStars = 0;

    void UpdateScore(int value)
    {
        currentScore += value;
        scoreText.text = currentScore.ToString();
        if (currentScore == ScoreManager.instance.GetHighScore() && currentScore > 0)
            displayHighScoreLine = true;
        SpeedIncreased = false;
    }

    void AddCoins()
    {
        if (currentScore > ScoreManager.instance.GetHighScore() && currentScore > 0)
            currentGStars += 1;
        else
            currentSStars += (currentScore % 500 == 0 && currentScore > 0 ? 1 : 0);
    }

    public void PassedHighScore()
    {
        if(score.gameObject.activeInHierarchy)
        {
            score.HighScore();
        }
    }
    #endregion





}

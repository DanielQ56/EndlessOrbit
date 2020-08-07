using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class MainGameManager : MonoBehaviour
{
    [SerializeField] Camera mainCam;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] bool isUnstable;


    public delegate void TimeStopped();
    public static TimeStopped StopTime;

    public delegate void TimeResumed();
    public static TimeResumed ResumeTime;

    public UnityEvent increaseSpeed;

    public static MainGameManager instance;


    bool movingCamera = false;

    bool playerIsAlive = true;

    bool SpeedIncreased = false;

    bool displayHighScoreLine = false;

    float width, height;

    GameObject player;


    #region Setup

    void Awake()
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

    void Start()
    {
        updateCameraPosition(startingPlanet);
        currentSStars = 0;
        timer = asteroidTimer;
    }

    void SetBounds()
    {
        width = mainCam.ScreenToWorldPoint(new Vector2(mainCam.pixelWidth, mainCam.pixelHeight)).x - mainCam.ScreenToWorldPoint(Vector2.zero).x;
        height = (mainCam.ScreenToWorldPoint(new Vector2(mainCam.pixelWidth, mainCam.pixelHeight)).y - mainCam.ScreenToWorldPoint(Vector2.zero).y) / 2;
        
    }

    #endregion

    #region In Game Routine

    [SerializeField] float asteroidTimer;
    [SerializeField] GameObject asteroidPrefab;
    [SerializeField] GameObject asteroidIndicator;

    float timer;

    bool timerPaused = false;
    bool spawningAsteroid = false;

      void Update()
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
        if (timer >= 2f)
        {
            if (!timerPaused)
                timer -= Time.deltaTime;
        }
        else
        {
            if(!spawningAsteroid)
            {
                StartCoroutine(IndicateSpawn());
            }
        }
    }

    IEnumerator IndicateSpawn()
    {
        spawningAsteroid = true;
        asteroidIndicator.SetActive(true);

        Vector3 startPos, endPos;
        GameObject currPlanet = planetParent.GetChild(0).gameObject;
        GameObject nextPlanet = planetParent.GetChild(1).gameObject;
        if (Random.value < 0.51f)
        {
            startPos = new Vector3(mainCam.transform.position.x - (width / 2),
                Random.Range(currPlanet.transform.position.y + currPlanet.GetComponent<CircleCollider2D>().radius * currPlanet.transform.localScale.x,
                nextPlanet.transform.position.y - nextPlanet.GetComponent<CircleCollider2D>().radius * nextPlanet.transform.localScale.x));
            endPos = new Vector3(mainCam.transform.position.x + (width / 2), Random.Range(currPlanet.transform.position.y + currPlanet.GetComponent<CircleCollider2D>().radius * currPlanet.transform.localScale.x,
                nextPlanet.transform.position.y - nextPlanet.GetComponent<CircleCollider2D>().radius * nextPlanet.transform.localScale.x));

            asteroidIndicator.transform.position = startPos + Vector3.right * 0.5f;
        }
        else
        {
            startPos = new Vector3(mainCam.transform.position.x + (width / 2), Random.Range(currPlanet.transform.position.y + currPlanet.GetComponent<CircleCollider2D>().radius * currPlanet.transform.localScale.x,
                nextPlanet.transform.position.y - nextPlanet.GetComponent<CircleCollider2D>().radius * nextPlanet.transform.localScale.x));
            endPos = new Vector3(mainCam.transform.position.x - (width / 2), Random.Range(currPlanet.transform.position.y + currPlanet.GetComponent<CircleCollider2D>().radius * currPlanet.transform.localScale.x,
                nextPlanet.transform.position.y - nextPlanet.GetComponent<CircleCollider2D>().radius * nextPlanet.transform.localScale.x));

            asteroidIndicator.transform.position = startPos + Vector3.left * 0.5f;
        }


        float time = 2f;
        while(time > 0f)
        {
            yield return new WaitForSeconds(time * 0.2f);
            asteroidIndicator.SetActive(false);
            time -= Mathf.Clamp(time * 0.2f, 0.05f, 2f);
            yield return new WaitForSeconds(time * 0.2f);
            asteroidIndicator.SetActive(true);
            time -= Mathf.Clamp(time * 0.2f, 0.05f, 2f);
            if (!playerIsAlive)
            {
                asteroidIndicator.SetActive(false);
                spawningAsteroid = false;
                yield break;
            }
        }
        asteroidIndicator.SetActive(false);
        SpawnAsteroid(startPos, endPos);
        spawningAsteroid = false;
        timer = asteroidTimer;
    }

    void SpawnAsteroid(Vector3 startPos, Vector3 endPos)
    { 
        GameObject asteroid = Instantiate(asteroidPrefab);
        asteroid.GetComponent<Asteroid>().CreateAsteroid(startPos, endPos);
    }

    #endregion

    #region Time
    [SerializeField] GameOverScript gameOver;
    [SerializeField] GameObject PausePanel;
    [SerializeField] GameObject ContinuePanel;
    [SerializeField] GameObject RewardPanel;
    [SerializeField] GameObject UnableToLoadPanel;

    bool hasUsedContinue = false;

    bool CanContinue = false;

    public void PauseGame()
    {
        Time.timeScale = 1 - Time.timeScale;
        PausePanel.SetActive(!PausePanel.activeInHierarchy);
    }

    public void GameOver()
    {
        if(StopTime != null)
            StopTime.Invoke();
        playerIsAlive = false;
        if(!hasUsedContinue)
        {
            ContinuePanel.SetActive(true);
        }
        else
        {
            FinalGameOver();
        }
    }

    public void FinalGameOver()
    {
#if UNITY_EDITOR
        ShowGameOverPanel();
#else
        if(GoogleAds.instance.ShouldShowAds())
        {
            GoogleAds.instance.ShowFullScreenAd();
        }
        else
        {
            ShowGameOverPanel();
        }
#endif
    }

    public void ShowGameOverPanel()
    {
        if (gameOver != null)
            gameOver.GameOver(currentScore, currentSStars, isUnstable);
    }

    public void WatchAdForContinue()
    {
#if UNITY_EDITOR
        RewardedContinue();
#else
        GoogleAds.instance.ShowRewardedAd();
#endif
    }

    public void Continue()
    {
        if(ResumeTime != null)
        {
            ResumeTime.Invoke();
        }
        hasUsedContinue = true;
        timer = asteroidTimer;
        playerIsAlive = true;
    }

    public void RewardedContinue()
    {
        CanContinue = true;
    }

    private void OnApplicationPause(bool pause)
    {
        if(!pause && CanContinue)
        {
            RewardPanel.SetActive(true);
            CanContinue = false;
        }
    }

    public void UnableToLoadVideo()
    {
        UnableToLoadPanel.SetActive(true);
    }

    public void ForceDetach()
    {
        player.GetComponent<PlayerController>().Detach();
    }
#endregion

#region Hit Planet
    [SerializeField] ParticleSystem playerParticles;

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
        if(isUnstable)
        {
            newPlanet.GetComponent<UnstableCelestialBody>().Attached(player);
        }
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

    [SerializeField] Transform planetParent;
    [SerializeField] Transform startingPlanet;
    [SerializeField] List<GameObject> planets;
    [SerializeField] List<GameObject> unstablePlanets;

    void GenerateNextPlanet(Transform newPlanet)
    {
        if (planetParent.childCount == 1)
        {
            int numPlanetsToSpawn = 1;
            Vector3 camPos = mainCam.transform.position;
            for (int i = 0; i < numPlanetsToSpawn; ++i)
            {
                GameObject clone = Instantiate((isUnstable ? unstablePlanets[Random.Range(0, planets.Count)] : planets[Random.Range(0, planets.Count)]), planetParent);
                DetermineOffset(clone);
                if(isUnstable)
                {
                    clone.GetComponent<UnstableCelestialBody>().DecrementStableTimer(currentScore / 1000);
                }
            }
        }
    }

    void DetermineOffset(GameObject planet)
    {

        CircleCollider2D coll = planet.GetComponent<CircleCollider2D>();


        Debug.Log("X is between: " + (-width / 2 + (coll.radius * planet.transform.localScale.x) + PlayerController.instance.GetXWidth()) + " and " + (width / 2 - (coll.radius * planet.transform.localScale.x) - PlayerController.instance.GetXWidth()));

        float x = Random.Range(-width / 2 + (coll.radius * planet.transform.localScale.x) + PlayerController.instance.GetXWidth(), width / 2 - (coll.radius * planet.transform.localScale.x) - PlayerController.instance.GetXWidth());


        float y = Random.Range(mainCam.transform.position.y + height / 3, mainCam.transform.position.y + height - (coll.radius * planet.transform.localScale.x));



        planet.transform.position = new Vector3(x, y, 0);
    }

#endregion

#region Score/Currency
    [SerializeField] HighScoreLine score;

    int currentScore = 0;
    int currentSStars = 0;

    void UpdateScore(int value)
    {
        currentScore += value;
        scoreText.text = currentScore.ToString();
        if (currentScore == ScoreManager.instance.GetHighScore(isUnstable) && currentScore > 0)
            displayHighScoreLine = true;
        SpeedIncreased = false;
    }

    void AddCoins()
    {
        if (currentScore > ScoreManager.instance.GetHighScore(isUnstable) && currentScore > 0)
            currentSStars += 5;
        else
            currentSStars += (currentScore % 300 == 0 && currentScore > 0 ? 1 : 0);
    }

    public void PassedHighScore()
    {
        if(score.gameObject.activeInHierarchy)
        {
            score.HighScore();
        }
    }
#endregion

    public bool isUnstableMode()
    {
        return isUnstable;
    }





}

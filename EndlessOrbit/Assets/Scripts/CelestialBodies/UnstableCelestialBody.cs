using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnstableCelestialBody : CelestialBody
{
    [SerializeField] ParticleSystem explosion;
    [SerializeField] float stableTimer;
    [SerializeField] float shakeInc;
    [SerializeField] float timerDec;

    Vector2 origin;

    float maxTimer;
    float timer;
    float currShakeMult;

    bool crumbling = false;
    bool shaking = false;
    bool playerAttached = false;
    bool playerWasAttached = false;
    bool timeStopped = false;

    GameObject player;

    SpriteRenderer rend;

    Color originalColor;
    Color ogLineColor;

    protected override void Start()
    {
        base.Start();
        timer = maxTimer;
        origin = this.transform.position;
        currShakeMult = shakeInc;
        rend = GetComponent<SpriteRenderer>();
        originalColor = rend.color;
        if(!isStartingBody)
            ogLineColor = this.GetComponent<LineRenderer>().material.color;
        MainGameManager.StopTime += this.StopTime;
        PlayerController.PlayerDetached += Detached;
        MainGameManager.ResumeTime += this.Continue;
    }

    private void Update()
    {
        if (!isStartingBody && playerAttached && !timeStopped)
        {
            if (timer > 0f)
            {
                Debug.Log("Should be shaking");
                if (!shaking)
                {
                    StartCoroutine(Shake());
                }
                timer -= Time.deltaTime;
            }
            else
            {
                if (!crumbling)
                {
                    MainGameManager.instance.ForceDetach();
                    Crumble();
                }
            }
        }
    }

    void Detached()
    {
        if (!isStartingBody && playerAttached)
        {
            playerAttached = false;
            playerWasAttached = true;
            Crumble();
        }
    }

    void StopTime()
    {
        timeStopped = true;
    }

    void Continue()
    {
        Debug.Log("HI");
        crumbling = false;
        shaking = false;
        timeStopped = false;
        playerAttached = playerWasAttached;
        this.transform.position = origin;
        rend.color = originalColor;
        if(!isStartingBody)
            this.GetComponent<LineRenderer>().material.color = ogLineColor;
        timer = maxTimer;
        currShakeMult = shakeInc;
    }

    IEnumerator Shake()
    {
        shaking = true;
        Vector2 newLoc = origin + Random.insideUnitCircle * currShakeMult;
        this.transform.position = newLoc;
        currShakeMult += shakeInc;
        yield return new WaitForSeconds(0.1f);
        shaking = false;
    }

    void Crumble()
    {
        crumbling = true;
        ParticleSystem.MainModule main = explosion.main;
        main.startColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        explosion.Play();
        rend.color = Color.black;
        this.GetComponent<LineRenderer>().material.color = Color.black;
        playerAttached = false;
    }

    public void DecrementStableTimer(int level)
    {
        maxTimer = stableTimer * Mathf.Pow(timerDec, level);
    }

    public void Attached(GameObject player)
    {
        playerAttached = true;
        this.player = player;
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if(!playerAttached)
            base.OnTriggerEnter2D(collision);
    }

    protected override void OnBecameInvisible()
    {
        base.OnBecameInvisible();
    }

    private void OnDestroy()
    {
        MainGameManager.StopTime -= this.StopTime;
        PlayerController.PlayerDetached -= this.Detached;
        MainGameManager.ResumeTime -= Continue;
    }
}

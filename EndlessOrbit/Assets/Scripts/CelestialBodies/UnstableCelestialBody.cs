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

    LineRenderer line;

    Color originalColor;
    Color ogLineColor;
    Color transparent;

    protected override void Start()
    {
        base.Start();
        timer = maxTimer;
        origin = this.transform.position;
        currShakeMult = shakeInc;
        transparent = new Color(0f, 0f, 0f, 0f);
        if (!isStartingBody)
        {
            line = this.GetComponent<LineRenderer>();
            line.material = new Material(Shader.Find("Unlit/Texture"));
        }
        MainGameManager.StopTime += this.StopTime;
        PlayerController.PlayerDetached += Detached;
        MainGameManager.ResumeTime += this.Continue;
    }

    protected override void Update()
    {
        base.Update();
        if (!isStartingBody && playerAttached && !timeStopped)
        {
            if (timer > 0f)
            {
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
        crumbling = false;
        shaking = false;
        timeStopped = false;
        playerAttached = playerWasAttached;
        this.transform.position = origin;
        if (!isStartingBody)
            line.enabled = true;
        timer = maxTimer;
        currShakeMult = shakeInc;
        foreach (Transform t in this.transform)
        {
            t.gameObject.SetActive(true);
        }
    }

    IEnumerator Shake()
    {
        shaking = true;
        Vector2 randomLoc = Random.insideUnitCircle * currShakeMult;
        float minMagnitude = (Vector2.one * (currShakeMult - shakeInc)).sqrMagnitude;
        Vector2 newLoc = origin + (randomLoc.sqrMagnitude < minMagnitude ? randomLoc.normalized * minMagnitude : randomLoc);
        this.transform.position = newLoc;
        currShakeMult += shakeInc;
        yield return new WaitForSeconds(0.1f);
        shaking = false;
    }

    void Crumble()
    {
        crumbling = true;

        foreach (Transform t in this.transform)
        {
            t.gameObject.SetActive(false);
        }
        explosion.gameObject.SetActive(true);
        ParticleSystem.MainModule main = explosion.main;
        
        main.startColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        explosion.Play();
        line.enabled = false;
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

    protected override void CheckForCollision()
    {
        if (!playerAttached)
            base.CheckForCollision();
    }

    public override void MakeEasier()
    {
        base.MakeEasier();
        timer = maxTimer * 1.7f;
    }

    /*
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if(!playerAttached)
            base.OnTriggerEnter2D(collision);
    }*/


    private void OnDestroy()
    {
        MainGameManager.StopTime -= this.StopTime;
        PlayerController.PlayerDetached -= this.Detached;
        MainGameManager.ResumeTime -= Continue;
    }
}

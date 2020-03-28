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
    bool timeStopped = false;

    GameObject player;

    SpriteRenderer rend;

    protected override void Start()
    {
        base.Start();
        timer = maxTimer;
        Debug.Log("Timer: " + timer);
        origin = this.transform.position;
        currShakeMult = shakeInc;
        rend = GetComponent<SpriteRenderer>();
        MainGameManager.StopTime += this.StopTime;
        PlayerController.PlayerDetached += Detached;
    }

    private void Update()
    {
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
            Crumble();
        }
    }

    void StopTime()
    {
        timeStopped = true;
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
    }
}

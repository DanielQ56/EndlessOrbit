using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField] float minSpeed;
    [SerializeField] float maxSpeed;

    Vector3 end;
    float speed;

    bool timeStopped = false;

    public void CreateAsteroid(Vector3 startPos, Vector3 endPos)
    {
        Debug.Log("Creating Asteroid");
        this.transform.position = startPos;
        end = endPos - startPos;
        speed = Random.Range(minSpeed, maxSpeed);
        MainGameManager.StopTime += this.StopTime;
    }


    void Update()
    {
        if(!timeStopped)
            transform.Translate(end * speed * Time.deltaTime);
    }

    void StopTime()
    {
        timeStopped = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(this.gameObject);
    }

    private void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField] float minSpeed;
    [SerializeField] float maxSpeed;

    Vector3 end;
    float speed;

    public void CreateAsteroid(Vector3 startPos, Vector3 endPos)
    {
        Debug.Log("Creating Asteroid");
        this.transform.position = startPos;
        end = endPos - startPos;
        speed = Random.Range(minSpeed, maxSpeed);
    }


    void Update()
    {
        transform.Translate(end * speed * Time.deltaTime);
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

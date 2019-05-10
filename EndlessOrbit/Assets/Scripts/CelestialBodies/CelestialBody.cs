using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CelestialBody : MonoBehaviour
{
    [SerializeField] float minRadius;
    [SerializeField] float maxRadius;
    [SerializeField] Sprite[] sprites;

    float radius;
    CircleCollider2D m_collider;
    // Start is called before the first frame update
    void Start()
    {
        radius = Random.Range(minRadius, maxRadius);
        m_collider = GetComponent<CircleCollider2D>();
        m_collider.radius = radius;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerController>().NewBodyToOrbit(transform);
        }
    }
}

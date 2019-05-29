using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CelestialBody : MonoBehaviour
{
    CircleCollider2D m_collider;
    // Start is called before the first frame update
	
	void Awake()
	{
	}
	
    void Start()
    {
        //radius = Random.Range(minRadius, maxRadius);
        m_collider = GetComponent<CircleCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerController>().NewBodyToOrbit(transform);
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    //TEMP [Draws a wireframe sphere as a gizmo to show orbiting radius/hitbox]
    void OnDrawGizmos()
	{
		Gizmos.color = Color.white;
        //Gizmos.DrawWireSphere(transform.position, m_collider.radius*5);
	}

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CelestialBody : MonoBehaviour
{
    [SerializeField] bool isStartingBody = false;

    CircleCollider2D m_collider;
    // Start is called before the first frame update

	
	void Awake()
	{
	}
	
    void Start()
    {
        //radius = Random.Range(minRadius, maxRadius);
        m_collider = GetComponent<CircleCollider2D>();
        DrawColliderCircle();
    }

    void DrawColliderCircle()
    {
        if (!isStartingBody)
        {
            float radius = m_collider.radius;
            int segments = 360;
            LineRenderer line = gameObject.AddComponent<LineRenderer>();
            line.useWorldSpace = false;
            line.startWidth = line.endWidth = 0.03f;
            line.positionCount = segments + 1;

            int pointCount = line.positionCount;
            Vector3[] points = new Vector3[pointCount];
            for (int i = 0; i < pointCount; ++i)
            {
                float rad = Mathf.Deg2Rad * (i * 360f / segments);
                points[i] = new Vector3(Mathf.Sin(rad) * radius, Mathf.Cos(rad) * radius);
            }
            line.SetPositions(points);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerController>().NewBodyToOrbit(transform);
            MainGameManager.instance.PassedHighScore();
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

#if UNITY_EDITOR
    //TEMP [Draws a wireframe sphere as a gizmo to show orbiting radius/hitbox]
    void OnDrawGizmos()
	{
		Gizmos.color = Color.white;
        //Gizmos.DrawWireSphere(transform.position, m_collider.radius*transform.localScale.x);
	}
#endif
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CelestialBody : MonoBehaviour
{
    [SerializeField] protected bool isStartingBody = false;


    protected CircleCollider2D m_collider;
    protected bool alreadyHadPlayer = false;

    float height;
    Camera mainCam;

    private void Awake()
    {
        m_collider = GetComponent<CircleCollider2D>();
        mainCam = Camera.main;
    }

    protected virtual void Start()
    {
        DrawColliderCircle();
        height = (mainCam.ScreenToWorldPoint(new Vector2(mainCam.pixelWidth, mainCam.pixelHeight)).y - mainCam.ScreenToWorldPoint(Vector2.zero).y) / 2;
    }

    protected virtual void Update()
    {
        CheckOutOfBounds();
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
            line.material.color = Color.white;
            for (int i = 0; i < pointCount; ++i)
            {
                float rad = Mathf.Deg2Rad * (i * 360f / segments);
                points[i] = new Vector3(Mathf.Sin(rad) * radius, Mathf.Cos(rad) * radius);
            }
            line.SetPositions(points);
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && !alreadyHadPlayer)
        {
            collision.gameObject.GetComponent<PlayerController>().NewBodyToOrbit(transform);
            alreadyHadPlayer = true;
            MainGameManager.instance.PassedHighScore();
        }
    }
    
    void CheckOutOfBounds()
    {
        if (this.transform.position.y + m_collider.radius * this.transform.localScale.y < mainCam.transform.position.y - height)
        {
            Destroy(this.gameObject);
        }
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

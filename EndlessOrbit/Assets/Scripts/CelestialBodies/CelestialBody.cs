using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CelestialBody : MonoBehaviour
{
    [SerializeField] protected bool isStartingBody = false;
    [SerializeField] GameObject Face;


    protected CircleCollider2D m_collider;
    protected bool alreadyHadPlayer = false;

    float height;
    Camera mainCam;

    float FaceDistance;

    float radiusMultiplier = 1.3f;

    protected virtual void Awake()
    {
        m_collider = GetComponent<CircleCollider2D>();
        mainCam = Camera.main;
        FaceDistance = (Face == null ? 0 : Mathf.Abs(Face.transform.localPosition.y));
        DrawColliderCircle();
        height = (mainCam.ScreenToWorldPoint(new Vector2(mainCam.pixelWidth, mainCam.pixelHeight)).y - mainCam.ScreenToWorldPoint(Vector2.zero).y) / 2;
    }


    protected virtual void Update()
    {
        CheckOutOfBounds();
        FollowPlayer();
    }

    private void FixedUpdate()
    {
        if(!isStartingBody)
            CheckForCollision();
    }

    void FollowPlayer()
    {
        if(Face != null)
            Face.transform.localPosition = (PlayerController.instance.transform.position - this.transform.position).normalized * FaceDistance;
    }


    void DrawColliderCircle()
    {
        if (!isStartingBody)
        {
            float radius = m_collider.radius;
            int segments = 360;

            LineRenderer line;
            if((line = gameObject.GetComponent<LineRenderer>()) == null)
                line = gameObject.AddComponent<LineRenderer>();
            line.useWorldSpace = false;
            line.startWidth = line.endWidth = 0.03f;
            line.positionCount = segments + 1;
            line.SetColors(Color.white, Color.white);
            int pointCount = line.positionCount;
            Vector3[] points = new Vector3[pointCount];
            for (int i = 0; i < pointCount; ++i)
            {
                float rad = Mathf.Deg2Rad * (i * 360f / segments);
                points[i] = new Vector3(Mathf.Sin(rad) * radius, Mathf.Cos(rad) * radius);
            }
            line.SetPositions(points);
            line.material = new Material(Shader.Find("Unlit/Texture"));
        }
    }

    protected virtual void CheckForCollision()
    {
        
        Collider2D coll = Physics2D.OverlapCircle(this.transform.position, m_collider.radius * transform.localScale.x, (1 << LayerMask.NameToLayer("Player")));
        if(coll != null && !alreadyHadPlayer && CheckDistanceFromPlanet(coll.gameObject.GetComponent<PlayerController>()))
        {
            coll.transform.position = this.transform.position + (coll.transform.position - this.transform.position).normalized * m_collider.radius * transform.localScale.x;
            coll.gameObject.GetComponent<PlayerController>().NewBodyToOrbit(this.transform);
            alreadyHadPlayer = true;
            MainGameManager.instance.PassedHighScore();
        }
    }

    public virtual void MakeEasier(int score)
    {
        m_collider.radius *= radiusMultiplier - (((radiusMultiplier - 1f) / 5f) * (score / 100));
        DrawColliderCircle();
    }

    bool CheckDistanceFromPlanet(PlayerController pc)
    {
        Vector3 pos = pc.gameObject.transform.position;
        return Vector3.Magnitude(pos - this.transform.position) <= m_collider.radius * transform.localScale.x + pc.GetXWidth() / 2;
    }
    /*
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && !alreadyHadPlayer)
        {
            collision.gameObject.GetComponent<PlayerController>().NewBodyToOrbit(transform);
            alreadyHadPlayer = true;
            MainGameManager.instance.PassedHighScore();
        }
    }*/
    
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

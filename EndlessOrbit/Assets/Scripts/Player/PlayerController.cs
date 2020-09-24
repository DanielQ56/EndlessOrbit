using UnityEngine;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Collections.Generic;

public enum PlayerState
{
    Tethered,
    Free
}

public enum Direction
{
    Right,
    Left,
    Up,
    Down
}

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance = null;

    [SerializeField] float rotationAngle;
    [SerializeField] float spinAngle;
    [SerializeField] Transform initialBody;
    [SerializeField] float MaxSpeed;
    [SerializeField] float angleIncreaseValue;
    [SerializeField] float lineLength;
    [SerializeField] int tutorialAmount;
    [SerializeField] AudioClip detached;
    [SerializeField] AudioClip hit;

    public delegate void Detached();
    public static Detached PlayerDetached;

    float width, height;
    Camera mainCam;

    int direction = 1;

    float movementAngle;

    bool tethered = true;

    LineRenderer line;
    TrailRenderer trail;

    Transform BodyToRotateAround;

    Vector3 posToMoveTowards;

    CircleCollider2D coll;

    PlayerState state;
    Direction horizontal;

    bool stillAlive = true;

    int linesLeft;

    private void Awake()
    {
        instance = this;
        coll = this.GetComponent<CircleCollider2D>();
        trail = this.GetComponentInChildren<TrailRenderer>();
        mainCam = Camera.main;
        SetBounds();
        this.gameObject.layer = LayerMask.NameToLayer("Player");
    }

    void SetBounds()
    {
        width = mainCam.ScreenToWorldPoint(new Vector2(mainCam.pixelWidth, mainCam.pixelHeight)).x - mainCam.ScreenToWorldPoint(Vector2.zero).x;
        height = (mainCam.ScreenToWorldPoint(new Vector2(mainCam.pixelWidth, mainCam.pixelHeight)).y - mainCam.ScreenToWorldPoint(Vector2.zero).y) / 2;

    }

    // Start is called before the first frame update
    void Start()
    {
        MainGameManager.instance.increaseSpeed.AddListener(IncreaseSpeedListener);
        MainGameManager.ResumeTime += Continue;
        BodyToRotateAround = initialBody;
        state = PlayerState.Tethered;
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        sprite.sprite = PlayerManager.instance.GetSelectedSprite();
        line = GetComponent<LineRenderer>();
        line.material = new Material(Shader.Find("Unlit/Texture"));
        line.SetColors(Color.white, Color.white);
        linesLeft = tutorialAmount;

    }

    // Update is called once per frame
    void Update()
    {
        if (stillAlive && !MainGameManager.instance.IsMovingCamera())
        {
            //DrawLine();
            CheckDetach();
            Move();
            Rotate();
            CheckOutOfBounds();
        }
    }


    #region Input

    void CheckDetach()
    {
#if UNITY_EDITOR
        if (((Input.touchCount > 0 && !IsPointerOverObject() || Input.GetKeyDown(KeyCode.Mouse0)) && state == PlayerState.Tethered  && Time.timeScale > 0))
        {
            if(EventSystem.current.currentSelectedGameObject == null)
                Detach();
        }
#else
                if ((Input.touchCount > 0 && state == PlayerState.Tethered && !IsPointerOverObject() && Time.timeScale > 0))
        {
            if(EventSystem.current.currentSelectedGameObject == null)
                Detach();
        }
#endif
    }

    bool IsPointerOverObject()
    {
        if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId) && Input.GetTouch(0).phase == TouchPhase.Began)
            return true;
        return false;

    }

#endregion

#region Movement

    Vector3 prevPos;


    void Rotate()
    {
        transform.Rotate(Vector3.forward, spinAngle * direction * Time.deltaTime);
    }

    void Move()
    {
        switch(state)
        {

            case PlayerState.Tethered:
                MoveAroundBody();
                break;
            case PlayerState.Free:
                MoveStraight();
                break;
        }
    }


    public void Detach()
    {
        if (state == PlayerState.Tethered)
        {
            AudioManager.instance.PlayEffect(detached);
            if(PlayerDetached != null)
                PlayerDetached.Invoke();
            state = PlayerState.Free;
            Vector3 relativePosition = CalculatePosToMoveTowards();
            movementAngle = Mathf.Tan(Mathf.Abs((posToMoveTowards.y - relativePosition.y) / (posToMoveTowards.x - relativePosition.x)));
            prevPos = this.transform.position;
            AssignDirection(posToMoveTowards);
            linesLeft -= 1;
        }
    } 

    void MoveStraight()
    {
        transform.Translate( Vector3.ClampMagnitude(posToMoveTowards, MaxSpeed) * Time.deltaTime, Space.World);
    }

    void MoveAroundBody()
    {
        this.transform.RotateAround(BodyToRotateAround.position, Vector3.forward, rotationAngle * Time.deltaTime);
    }

    Vector3 CalculatePosToMoveTowards()
    {
        Vector3 relativePosition = new Vector3(this.transform.position.x - BodyToRotateAround.transform.position.x,
this.transform.position.y - BodyToRotateAround.transform.position.y, 0);
        float inverseSlope = (relativePosition.x != 0 ? (-1f / (relativePosition.y / relativePosition.x)) : 0);
        float b = relativePosition.y - (relativePosition.x * inverseSlope);
        float x = relativePosition.x + (direction == 1 ? -1 : 1) * Mathf.Sign(relativePosition.y) * 5;
        posToMoveTowards = new Vector3(x, (inverseSlope * x + b), 0) * 5f;
        return relativePosition;
    }

#endregion

#region New Planet

    public void NewBodyToOrbit(Transform newBody)
    {
        AudioManager.instance.PlayEffect(hit);
        AssignNewAngleAndDirection(newBody);
        BodyToRotateAround = newBody;
        state = PlayerState.Tethered;
        MainGameManager.instance.AttachedToNewPlanet(newBody);
        line.positionCount = 0;

    }
    
    public void IncreaseSpeedListener()
    {
        rotationAngle += (Mathf.Sign(rotationAngle) * angleIncreaseValue);
    }
#endregion


#region Death

    void Continue()
    {
        this.transform.position = prevPos;
        state = PlayerState.Tethered;
        stillAlive = true;
        if(linesLeft >= 0)
        {
            linesLeft += 1;
        }
        StartCoroutine(EnableTrail());
    }

    IEnumerator EnableTrail()
    {
        trail.Clear();
        yield return null;
        trail.emitting = true;
    }

    void CheckOutOfBounds()
    {
        if(this.transform.position.x +(coll.radius * this.transform.localScale.x) < mainCam.transform.position.x  - width / 2 || this.transform.position.x - (coll.radius * this.transform.localScale.x) > mainCam.transform.position.x + width / 2 
            || this.transform.position.y + (coll.radius * this.transform.localScale.y) < mainCam.transform.position.y - height || this.transform.position.y - (coll.radius * this.transform.localScale.y) > mainCam.transform.position.y + height)
        {
            Dead();
        }
    }

    void Dead()
    {
        if (stillAlive)
        {
            stillAlive = false;
            line.positionCount = 0;
            MainGameManager.instance.GameOver();
            trail.emitting = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Asteroid"))
        {
            Dead();
        }
    }


    private void OnDestroy()
    {
        MainGameManager.ResumeTime -= Continue;
        MainGameManager.instance.increaseSpeed.RemoveListener(IncreaseSpeedListener);
    }

#endregion

    void DrawLine()
    {
        if (linesLeft > 0 && state == PlayerState.Tethered)
        {
            CalculatePosToMoveTowards();
            Vector3[] points = new Vector3[2];
            points[0] = transform.position;
            points[1] = BodyToRotateAround.transform.InverseTransformDirection(posToMoveTowards) * lineLength;
            line.positionCount = 2;
            line.SetPositions(points);
        }
    }

#region Assigning The New Direction 
    void AssignNewAngleAndDirection(Transform body)
    {
        float posX = body.position.x - transform.position.x;
        if(Mathf.Abs(Mathf.Rad2Deg * movementAngle) % 90 < 45f)
        {
            switch(horizontal)
            {
                case Direction.Left:
                    if (posX < 0)
                    {
                        direction = 1;
                    }
                    else
                    {
                        direction = -1;
                    }
                    break;
                
                case Direction.Right:
                    if (posX < 0)
                    {
                        direction = 1;
                    }
                    else
                    {
                        direction = -1;
                    }
                    break;
            }
        }
        else
        {
            switch (horizontal)
            {
                case Direction.Left:
                    if (posX < 0)
                    {
                        direction = 1;
                    }
                    else
                    {
                        direction = -1;
                    }
                    break;

                case Direction.Right:
                    if (posX < 0)
                    {
                        direction = 1;
                    }
                    else
                    {
                        direction = -1;
                    }
                    break;
            }
        }
        rotationAngle = Mathf.Abs(rotationAngle) * direction;
    }

    void AssignDirection(Vector3 pos)
    {
        float posX = pos.x - transform.position.x;
        float posY = pos.y - transform.position.y;

        if(posX < 0)
        {
            horizontal = Direction.Left;
        }
        else
        {
            horizontal = Direction.Right;
        }
    }
#endregion

    public float GetXWidth()
    {
        return coll.radius * this.transform.localScale.x;
    }
}

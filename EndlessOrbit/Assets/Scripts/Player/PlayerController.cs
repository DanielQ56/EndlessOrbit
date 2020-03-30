using UnityEngine;
using UnityEngine.EventSystems;

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
    [SerializeField] float rotationAngle;
    [SerializeField] float spinAngle;
    [SerializeField] Transform initialBody;
    [SerializeField] float MaxSpeed;
    [SerializeField] float angleIncreaseValue;
    [SerializeField] float lineLength;
    [SerializeField] int tutorialAmount;

    public delegate void Detached();
    public static Detached PlayerDetached;

    int direction = 1;

    float movementAngle;

    bool tethered = true;

    LineRenderer line;

    Transform BodyToRotateAround;

    Vector3 posToMoveTowards;

    PlayerState state;
    Direction horizontal;

    bool stillAlive = true;

    int linesLeft;

    // Start is called before the first frame update
    void Start()
    {
        MainGameManager.instance.increaseSpeed.AddListener(IncreaseSpeedListener);
        BodyToRotateAround = initialBody;
        state = PlayerState.Tethered;
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        sprite.sprite = PlayerManager.instance.GetSelectedSprite();
        line = GetComponent<LineRenderer>();
        linesLeft = tutorialAmount;

    }

    // Update is called once per frame
    void Update()
    {
        if (stillAlive && !MainGameManager.instance.IsMovingCamera())
        {
            CheckDetach();
            Move();
            Rotate();
        }
    }

    #region Input

    void CheckDetach()
    {
        if ((Input.touchCount > 0 && state == PlayerState.Tethered && !IsPointerOverObject() && Time.timeScale > 0) || Input.GetKeyDown(KeyCode.Mouse0))
        {
            if(EventSystem.current.currentSelectedGameObject == null)
                Detach();
        }
    }

    bool IsPointerOverObject()
    {
        if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId) && Input.GetTouch(0).phase == TouchPhase.Began)
            return true;
        return false;

    }

    #endregion

    #region Movement

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
            if (!AudioManager.instance.muted)
                AudioManager.instance.Play("Space");
            if(PlayerDetached != null)
                PlayerDetached.Invoke();
            state = PlayerState.Free;
            Vector3 relativePosition = new Vector3(this.transform.position.x - BodyToRotateAround.transform.position.x,
                this.transform.position.y - BodyToRotateAround.transform.position.y, 0);
            float inverseSlope = (relativePosition.x != 0 ? (-1f / (relativePosition.y / relativePosition.x)) : 0);
            float b = relativePosition.y - (relativePosition.x * inverseSlope);
            float x = relativePosition.x + (direction == 1 ? -1 : 1) * Mathf.Sign(relativePosition.y) * 5;
            posToMoveTowards = new Vector3(x, (inverseSlope * x + b), 0) * 5f;
            movementAngle = Mathf.Tan(Mathf.Abs((posToMoveTowards.y - relativePosition.y) / (posToMoveTowards.x - relativePosition.x)));
            AssignDirection(posToMoveTowards);
            DrawLine();
        }
    } 

    void MoveStraight()
    {
        Debug.Log(posToMoveTowards);
        transform.Translate( BodyToRotateAround.InverseTransformDirection(Vector3.ClampMagnitude(posToMoveTowards, MaxSpeed)) * Time.deltaTime, BodyToRotateAround.transform);
    }

    void MoveAroundBody()
    {
        this.transform.RotateAround(BodyToRotateAround.position, Vector3.forward, rotationAngle * Time.deltaTime);
    }

    #endregion

    #region New Planet

    public void NewBodyToOrbit(Transform newBody)
    {
        //AUDIO (can move)
        if (!AudioManager.instance.muted)
            AudioManager.instance.Play("Hit");
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

    void Dead()
    {
        if (stillAlive)
        {
            stillAlive = false;
            MainGameManager.instance.GameOver();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Asteroid"))
        {
            Dead();
        }
    }

    private void OnBecameInvisible()
    {
        Dead();
    }

    #endregion

    void DrawLine()
    {
        if (linesLeft > 0)
        {
            Vector3[] points = new Vector3[2];
            points[0] = transform.position;
            points[1] = BodyToRotateAround.transform.InverseTransformDirection(posToMoveTowards) * lineLength;
            Debug.Log(points[1]);
            line.positionCount = 2;
            line.SetPositions(points);
            linesLeft -=1;
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
}

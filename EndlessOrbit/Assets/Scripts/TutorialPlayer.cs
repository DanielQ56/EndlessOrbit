using UnityEngine;
using UnityEngine.EventSystems;


public class TutorialPlayer : MonoBehaviour
{
    [SerializeField] int velocity;
    [SerializeField] float rotationAngle;
    [SerializeField] Transform initialBody;
    [SerializeField] float MaxSpeed;

    int direction = 1;

    float movementAngle;

    bool tethered = true;

    Transform BodyToRotateAround;

    Vector3 posToMoveTowards;
    Vector3 originalPosition;

    PlayerState state;
    Direction horizontal;

    bool testing = false;

    LineRenderer line;

    // Start is called before the first frame update
    void Start()
    {
        BodyToRotateAround = initialBody;
        state = PlayerState.Tethered;
        line = GetComponent<LineRenderer>();

    }


    // Update is called once per frame
    void Update()
    {
        if (testing)
        {
            CheckDetach();
            Move();
        }
    }

    void CheckDetach()
    {
        if ((Input.GetKeyDown(KeyCode.Space) || Input.touches.Length > 0 || Input.GetKeyDown(KeyCode.Mouse0)) && state == PlayerState.Tethered)
        {
            if (EventSystem.current != null  && !IsPointerOverObject())
                Detach();
        }
    }

    bool IsPointerOverObject()
    {
        //check mouse
        if (EventSystem.current.IsPointerOverGameObject())
            return true;
   
        //check touch
        if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
        {
            if (EventSystem.current.IsPointerOverGameObject(Input.touches[0].fingerId))
                return true;
        }
   
        return false;
    }

    void Move()
    {
        switch (state)
        {

            case PlayerState.Tethered:
                MoveAroundBody();
                break;
            case PlayerState.Free:
                MoveStraight();
                break;
        }
    }

    void Detach()
    {

        state = PlayerState.Free;
        Vector3 relativePosition = new Vector3(this.transform.position.x - BodyToRotateAround.transform.position.x,
            this.transform.position.y - BodyToRotateAround.transform.position.y, 0);
        float inverseSlope = -1f / (relativePosition.y / (relativePosition.x + (relativePosition.x > 0 ? 0.5f : -0.5f)));
        float b = relativePosition.y - (relativePosition.x * inverseSlope);
        float x = relativePosition.x + (direction == 1 ? -1: 1) * Mathf.Sign(relativePosition.y) * 5;
        posToMoveTowards = new Vector3(x, (inverseSlope * x + b), 0);
        movementAngle = Mathf.Tan(Mathf.Abs((posToMoveTowards.y - transform.position.y) / (posToMoveTowards.x - transform.position.x)));
        originalPosition = transform.position;
        AssignDirection(posToMoveTowards);
        DrawLine();
    }

    float getDecreasedSlope(float posx, float posy)
    {
        if (posx > 0)
        {
            return (posy > 0 ? (direction == -1 ? 0.4f : -0.4f) : (direction == -1 ? 8f / 4f : 4f / 8f));
        }
        else
        {
            return (posy > 0 ? (direction == -1 ? 8f / 4f : 4f / 8f) : (direction == -1 ? 4f / 8f : 8f / 4f));
        }
    }

    void MoveStraight()
    {
        Debug.Log(posToMoveTowards);
        transform.Translate(Vector3.ClampMagnitude(posToMoveTowards, MaxSpeed) * Time.deltaTime, BodyToRotateAround);
    }

    void MoveAroundBody()
    {
        this.transform.RotateAround(BodyToRotateAround.position, Vector3.forward, rotationAngle * Time.deltaTime);
    }

    void DrawLine()
    {
        Vector3[] points = new Vector3[2];
        points[0] = transform.position;
        points[1] = transform.position + posToMoveTowards * 2;
        line.positionCount = 2;
        line.SetPositions(points);
    }

    public void ToggleTesting()
    {
        testing = !testing;
    }

    public void PlayerReset()
    {
        BodyToRotateAround = initialBody;
        state = PlayerState.Tethered;
        this.transform.position = originalPosition;
        line.positionCount = 0;
    }

    public void ChangeDirection()
    {
        if (state == PlayerState.Tethered)
        {
            rotationAngle *= -1;
            direction *= -1;
        }
    }

    #region Assigning The New Direction 
    void AssignNewAngleAndDirection(Transform body)
    {
        float posX = body.position.x - transform.position.x;
        if (Mathf.Abs(Mathf.Rad2Deg * movementAngle) % 90 < 44f)
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

        if (posX < 0)
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

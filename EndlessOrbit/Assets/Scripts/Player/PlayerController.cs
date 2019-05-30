using UnityEngine;

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
    [SerializeField] int velocity;
    [SerializeField] float rotationAngle;
    [SerializeField] Transform initialBody;
    [SerializeField] float MaxSpeed;

    int direction = 1;

    float movementAngle;

    bool tethered = true;

    Transform BodyToRotateAround;

    Vector3 posToMoveTowards;

    PlayerState state;
    Direction horizontal;

    bool stillAlive = true;

    // Start is called before the first frame update
    void Start()
    {
        BodyToRotateAround = initialBody;
        state = PlayerState.Tethered;
    }

    // Update is called once per frame
    void Update()
    {
        if (stillAlive)
        {
            if (Input.GetKeyDown(KeyCode.Space) && state == PlayerState.Tethered)
            {
                Detach();
            }
            Move();
        }
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

    void Detach()
    {
        state = PlayerState.Free;
        Vector3 relativePosition = new Vector3(this.transform.position.x - BodyToRotateAround.transform.position.x,
            this.transform.position.y - BodyToRotateAround.transform.position.y, 0);
        float inverseSlope = -1 / (relativePosition.y / relativePosition.x);
        Debug.Log(inverseSlope);
        float b = relativePosition.y - (relativePosition.x * inverseSlope);
        float x = relativePosition.x - Mathf.Sign(relativePosition.y) * 5;
        posToMoveTowards = new Vector3(x, (inverseSlope * x + b), 0);
        movementAngle = Mathf.Tan(Mathf.Abs(posToMoveTowards.y / posToMoveTowards.x));
        AssignDirection(posToMoveTowards);
    }

    void MoveStraight()
    {
         transform.Translate( Vector3.ClampMagnitude(posToMoveTowards, MaxSpeed) * direction * Time.deltaTime, BodyToRotateAround.transform);
    }

    void MoveAroundBody()
    {
        this.transform.RotateAround(BodyToRotateAround.position, Vector3.forward, rotationAngle * Time.deltaTime);
    }

    public void NewBodyToOrbit(Transform newBody)
    {
        AssignNewAngleAndDirection(newBody);
        BodyToRotateAround = newBody;
        state = PlayerState.Tethered;
        MainGameManager.instance.AttachedToNewPlanet(newBody);
    }

    private void OnBecameInvisible()
    {
        stillAlive = false;
        MainGameManager.instance.GameOver();
    }

    #region Assigning The New Direction 
    void AssignNewAngleAndDirection(Transform body)
    {
        float posX = body.position.x - transform.position.x;
        Debug.Log(Mathf.Abs(Mathf.Rad2Deg * movementAngle) % 90 + " " + horizontal.ToString());
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

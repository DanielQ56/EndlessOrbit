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

    bool tethered = true;

    Transform BodyToRotateAround;

    Vector3 posToMoveTowards;

    PlayerState state;

    Direction horizontal;
    Direction vertical;

    // Start is called before the first frame update
    void Start()
    {
        BodyToRotateAround = initialBody;
        state = PlayerState.Tethered;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && state == PlayerState.Tethered)
        {
            Detach();
        }
        Move();
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
        float b = relativePosition.y - (relativePosition.x * inverseSlope);
        float x = relativePosition.x - Mathf.Sign(relativePosition.y) * velocity;
        posToMoveTowards = new Vector3(x, (inverseSlope * x + b), 0);
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
    #region Assigning The New Direction 
    void AssignNewAngleAndDirection(Transform body)
    {
        float distX = Mathf.Abs(transform.position.x - body.position.x);
        float distY = Mathf.Abs(transform.position.y - body.position.y);
        if(distX < distY)
        {
            switch(horizontal)
            {
                case Direction.Left:
                    switch (vertical)
                    {

                        case Direction.Up:
                            direction = -1;
                            break;
                        case Direction.Down:
                            direction = 1;
                            break;
                    }
                    break;
                
                case Direction.Right:
                    switch (vertical)
                    { 
                        case Direction.Up:
                            direction = 1;
                            break;
                        case Direction.Down:
                            direction = -1;
                            break;
                    }
                    break;
            }
        }
        else
        {
            switch(vertical)
            {
                case Direction.Up:
                    switch(horizontal)
                    {
                        case Direction.Right:
                            direction = -1;
                            break;
                        case Direction.Left:
                            direction = 1;
                            break;
                    }
                    break;
                case Direction.Down:
                    switch (horizontal)
                    {
                        case Direction.Right:
                            direction = 1;
                            break;
                        case Direction.Left:
                            direction = -1;
                            break;
                    }
                    break;
            }
        }
        rotationAngle = Mathf.Abs(rotationAngle) * direction;
        Debug.Log(rotationAngle + " " + horizontal.ToString() + " " + vertical.ToString());
    }

    #endregion

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
        if (posY < 0)
        {
            vertical = Direction.Down;
        }
        else
        {
            vertical = Direction.Up;
        }
    }
}

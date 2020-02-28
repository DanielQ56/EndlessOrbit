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
    [SerializeField] int velocity;
    [SerializeField] float rotationAngle;
    [SerializeField] float spinAngle;
    [SerializeField] Transform initialBody;
    [SerializeField] float MaxSpeed;
    [SerializeField] float angleIncreaseValue;

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
        MainGameManager.instance.increaseSpeed.AddListener(IncreaseSpeedListener);
        BodyToRotateAround = initialBody;
        state = PlayerState.Tethered;
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        sprite.sprite = PlayerCustomization.instance.playerSprite;
        sprite.color = PlayerCustomization.instance.playerColor;

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
            state = PlayerState.Free;
            Vector3 relativePosition = new Vector3(this.transform.position.x - BodyToRotateAround.transform.position.x,
                this.transform.position.y - BodyToRotateAround.transform.position.y, 0);
            float inverseSlope = (relativePosition.x != 0 ? (-1f / (relativePosition.y / relativePosition.x)) : 0);//(relativePosition.x + (relativePosition.x > 0 ? 0.5f : -0.5f)));
            float b = relativePosition.y - (relativePosition.x * inverseSlope);
            float x = relativePosition.x + (direction == 1 ? -1 : 1) * Mathf.Sign(relativePosition.y) * 5;
            posToMoveTowards = new Vector3(x, (inverseSlope * x + b), 0);
            movementAngle = Mathf.Tan(Mathf.Abs((posToMoveTowards.y - transform.position.y) / (posToMoveTowards.x - transform.position.x)));
            AssignDirection(posToMoveTowards);
        }
    } 
    /*void Detach() OLD DETACH JIC WE NEED IT AGAIN
    {
        //Audio (can move)
        
        state = PlayerState.Free;
        Vector3 relativePosition = new Vector3(this.transform.position.x - BodyToRotateAround.transform.position.x,
            this.transform.position.y - BodyToRotateAround.transform.position.y, 0);
        float inverseSlope = -1f / (relativePosition.y / (relativePosition.x + (relativePosition.x > 0 ? 0.5f : -0.5f)));
        float b = relativePosition.y - (relativePosition.x * inverseSlope);
        float x = relativePosition.x + (direction == 1 ? -1 : 1) * Mathf.Sign(relativePosition.y) * 5;
    }*/

    void MoveStraight()
    {
        if(Vector3.Magnitude(posToMoveTowards) < velocity)
        {
            posToMoveTowards *= 2;
        }
         transform.Translate( Vector3.ClampMagnitude(posToMoveTowards, MaxSpeed)  * Time.deltaTime, BodyToRotateAround.transform);
    }

    void MoveAroundBody()
    {
        this.transform.RotateAround(BodyToRotateAround.position, Vector3.forward, rotationAngle * Time.deltaTime);
    }

    public void NewBodyToOrbit(Transform newBody)
    {
        //AUDIO (can move)
        if (!AudioManager.instance.muted)
            AudioManager.instance.Play("Hit");

        AssignNewAngleAndDirection(newBody);
        BodyToRotateAround = newBody;
        state = PlayerState.Tethered;
        MainGameManager.instance.AttachedToNewPlanet(newBody);
    }
    
    public void IncreaseSpeedListener()
    {
        Debug.Log("Increased");
        rotationAngle += (Mathf.Sign(rotationAngle) * angleIncreaseValue);
    }

    private void OnBecameInvisible()
    {
        stillAlive = false;
        Debug.Log("dead");
        MainGameManager.instance.GameOver();
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

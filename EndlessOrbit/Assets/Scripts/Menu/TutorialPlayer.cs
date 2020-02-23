using UnityEngine;
using UnityEngine.EventSystems;


public class TutorialPlayer : MonoBehaviour
{
    [SerializeField] int velocity;
    [SerializeField] float rotationAngle;
    [SerializeField] Transform initialBody;
    [SerializeField] float MaxSpeed;
    [SerializeField] float orbitRadius;

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
        this.transform.position = new Vector3(initialBody.transform.position.x, initialBody.transform.position.y + orbitRadius, initialBody.transform.position.z);
        movementAngle = rotationAngle;
    }


    // Update is called once per frame
    void Update()
    {
        if (testing)
        {
            Move();
        }
    }


    public void UpdateSpeed(float multi)
    {
        movementAngle = rotationAngle * multi;
    }

    #region Movement

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

    public void Detach()
    {
        if (state == PlayerState.Tethered)
        {
            state = PlayerState.Free;
            Vector3 relativePosition = new Vector3(this.transform.position.x - BodyToRotateAround.transform.position.x,
                this.transform.position.y - BodyToRotateAround.transform.position.y, 0);
            float inverseSlope = (relativePosition.x != 0 ? (-1f / (relativePosition.y / relativePosition.x)) : 0);//(relativePosition.x + (relativePosition.x > 0 ? 0.5f : -0.5f)));
            float b = relativePosition.y - (relativePosition.x * inverseSlope);
            float x = relativePosition.x + (direction == 1 ? -1 : 1) * Mathf.Sign(relativePosition.y) * 5;
            posToMoveTowards = new Vector3(x, (inverseSlope * x + b), 0);
            originalPosition = transform.position;
            DrawLine();
        }
    }

    public void UpdatePositionInOrbit(float rad)
    {
        float angle = Mathf.Atan(transform.position.y / transform.position.x);
        transform.position = initialBody.transform.position + new Vector3(Mathf.Cos(angle) * rad, Mathf.Sin(angle) * rad, transform.position.z);

    }

    void MoveStraight()
    {
        transform.Translate(Vector3.ClampMagnitude(posToMoveTowards, MaxSpeed) * Time.deltaTime, BodyToRotateAround);
    }

    void MoveAroundBody()
    {
        this.transform.RotateAround(BodyToRotateAround.position, Vector3.forward, movementAngle * Time.deltaTime);
    }

    void DrawLine()
    {
        Vector3[] points = new Vector3[2];
        points[0] = transform.position;
        points[1] = transform.position + posToMoveTowards * 2;
        line.positionCount = 2;
        line.SetPositions(points);
    }

    #endregion



    #region Testing
    public void ToggleTesting()
    {
        testing = !testing;
    }

    public void ChangeDirection()
    {
        if (state == PlayerState.Tethered)
        {
            movementAngle *= -1;
            direction *= -1;
        }
    }

    public void PlayerReset()
    {
        BodyToRotateAround = initialBody;
        state = PlayerState.Tethered;
        this.transform.position = originalPosition;
        line.positionCount = 0;
    }
    #endregion

}

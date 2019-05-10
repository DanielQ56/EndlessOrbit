using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public enum PlayerState
    {
        Tethered,
        Free
    }


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
        if (state == PlayerState.Tethered)
        {
            MoveAroundBody();
        }
        else if(state == PlayerState.Free)
        {
            MoveStraight();
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
    }

    void MoveStraight()
    {
         transform.Translate( Vector3.ClampMagnitude(posToMoveTowards, MaxSpeed) * Time.deltaTime, BodyToRotateAround.transform);
    }

    void MoveAroundBody()
    {
        this.transform.RotateAround(BodyToRotateAround.position, Vector3.forward, rotationAngle * Time.deltaTime);
    }

    public void NewBodyToOrbit(Transform newBody)
    {
        //direction = (transform.position.x < newBody.transform.position.x ? -1 : 1);
        //rotationAngle = Mathf.Abs(rotationAngle) * direction;
        BodyToRotateAround = newBody;
        state = PlayerState.Tethered;
    }
}

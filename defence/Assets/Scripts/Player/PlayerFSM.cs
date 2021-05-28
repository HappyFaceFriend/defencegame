using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFSM : FSMBase
{
    public Vector2 LastInputVector {  get { return lastInputVector; } }

    DirectionMovement movementComponent;
    FlipObjectToPoint spriteFlipComponent;
    [SerializeField]
    PlayerHand hand;
    Vector2 inputVector;
    Vector2 lastInputVector;



    new void Awake()
    {
        base.Awake();
        movementComponent = GetComponent<DirectionMovement>();
        movementComponent.direction = inputVector;
        inputVector = Vector2.zero;
        spriteFlipComponent = GetComponent<FlipObjectToPoint>();
        lastInputVector = new Vector2(1, 0);


    }
    void Update()
    {
        inputVector = Vector2.zero;
        if (Input.GetKey(KeyCode.W))
            inputVector.y += 1;
        if (Input.GetKey(KeyCode.S))
            inputVector.y -= 1;
        if (Input.GetKey(KeyCode.A))
            inputVector.x -= 1;
        if (Input.GetKey(KeyCode.D))
            inputVector.x += 1;
        if (inputVector != Vector2.zero)
            lastInputVector = inputVector;
        if (inputVector.x != 0 && inputVector.y != 0)
            inputVector *= 0.707f; // same as *=1.414f
        movementComponent.direction = inputVector;



        spriteFlipComponent.targetPoint = transform.position + VectorUtils.Vec2toVec3(inputVector);
    }
    public void SetIsHolding(bool isHolding)
    {
        SetBoolParam("IsHolding", isHolding);
    }
    IEnumerator Idle()
    {
        do
        {
            if (inputVector != Vector2.zero)
                SetState(State.Walk);
            yield return null;
        } while (!isNewState);
    }
    IEnumerator Walk()
    {
        EnableComponent(movementComponent);
        do
        {
            if (inputVector == Vector2.zero)
                SetState(State.Idle);
            yield return null;
        } while (!isNewState);
        movementComponent.direction = inputVector;
        DisableComponent(movementComponent);
    }
}

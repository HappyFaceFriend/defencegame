using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFSM : FSMBase
{
    public Vector2 LastInputVector {  get { return lastInputVector; } }
    public State CurrentState { get { return currentState; } }
    public OtherState CurrentOtherState 
    { 
        get {
            if (currentState != State.Other)
                return OtherState.NotOther;
            return currentOtherState;
        }
    }

    DirectionMovement movementComponent;
    FlipObjectToPoint spriteFlipComponent;
    [SerializeField]
    PlayerHand hand;
    Vector2 inputVector;
    Vector2 lastInputVector;
    public enum OtherState
    { Holding=0, BatteryCharging=1, NotOther=10}
    [SerializeField] OtherState currentOtherState;


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
        //movement related
        inputVector = Vector2.zero;
        if (Input.GetKey(KeyCode.UpArrow))
            inputVector.y += 1;
        if (Input.GetKey(KeyCode.DownArrow))
            inputVector.y -= 1;
        if (Input.GetKey(KeyCode.LeftArrow))
            inputVector.x -= 1;
        if (Input.GetKey(KeyCode.RightArrow))
            inputVector.x += 1;
        if (inputVector != Vector2.zero)
            lastInputVector = inputVector;
        if (inputVector.x != 0 && inputVector.y != 0)
            inputVector *= 0.707f; // same as *=1.414f
        movementComponent.direction = inputVector;
        spriteFlipComponent.targetPoint = transform.position + VectorUtils.Vec2toVec3(inputVector);

        //hold & put down
        if (Input.GetKeyDown(KeyCode.Space))
        {
            hand.SpacePressed();
        }
    }
    public void SetIsHolding(bool isHolding)
    {
        SetBoolParam("IsHolding", isHolding);
    }
    public void SetState(OtherState otherState)
    {
        currentOtherState = otherState;
        if (animator != null)
            animator.SetInteger("OtherState", (int)currentOtherState);
        SetState(State.Other);
    }
    IEnumerator Other()
    {
        while (true)
        {
            isNewState = false;
            yield return StartCoroutine(currentOtherState.ToString());
            if (currentState != State.Other)
                break;
        }
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
    IEnumerator BatteryCharging()
    {
        do
        {
            yield return null;
        } while (!isNewState);
    }
}

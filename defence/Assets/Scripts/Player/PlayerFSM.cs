using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFSM : MonoBehaviour
{
    public Vector2 LastInputVector {  get { return lastInputVector; } }
    public State CurrentState { get { return currentState; } }
    public bool IsHolding { get { return isHolding; } set { isHolding = value; } }

    [ReadOnly][SerializeField] State currentState;
    [Header("References")]
    [SerializeField] Transform imageTransform;
    [SerializeField] PlayerHand hand;
    DirectionMovement movementComponent;
    FlipObjectToPoint spriteFlipComponent;
    Vector2 inputVector;
    Vector2 lastInputVector;
    bool isHolding;
    [Header("Player Stats")]
    [SerializeField] float batteryChargeSpeed;


    public enum State
    {
        Idle = 0, Walk = 1, HoldingIdle = 2, HoldingWalk=3, BatteryCharging=4
    }
    

    bool isNewState;
    new Rigidbody2D rigidbody;
    MovementController movementController;

    Animator animator;
    SpriteRenderer spriteRenderer;

    protected void Awake()
    {
        currentState = State.Idle;
        rigidbody = GetComponent<Rigidbody2D>();
        movementController = gameObject.AddComponent<MovementController>();
        animator = imageTransform.GetComponent<Animator>();
        spriteRenderer = imageTransform.GetComponent<SpriteRenderer>();
        movementComponent = GetComponent<DirectionMovement>();
        movementComponent.direction = inputVector;
        inputVector = Vector2.zero;
        spriteFlipComponent = GetComponent<FlipObjectToPoint>();
        lastInputVector = new Vector2(1, 0);
    }

    protected void Start()
    {
        StartCoroutine(FSMMain());
    }
    IEnumerator FSMMain()
    {
        while (true)
        {
            isNewState = false;
            yield return StartCoroutine(currentState.ToString());
        }
    }
    public void SetState(State state)
    {
        currentState = state;
        isNewState = true;
        if (animator != null)
            animator.SetInteger("State", (int)currentState);
    }
    void Update()
    {
        //movement related
        inputVector = Vector2.zero;
        if(currentState != State.BatteryCharging)
        {
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
        }

        //hold & put down
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (hand.SelectedObject!=null && !hand.IsHoldingObject && hand.SelectedObject is HoldableObject)
                hand.HoldSelectedObject();
            else if (hand.SelectedBackground == GridBackground.Empty && hand.IsHoldingObject && hand.SelectedObject == null)
                hand.PutDownObject();
            else if(hand.SelectedObject!=null && !hand.IsHoldingObject && hand.SelectedObject is InteractableObject)
            {
                (hand.SelectedObject as InteractableObject).Interact();
            }
        }
        if (Input.GetKey(KeyCode.C))
        {
            if (hand.SelectedObject!= null && hand.SelectedObject is TowerFSMBase)
            {
                if(CurrentState != PlayerFSM.State.BatteryCharging && !isHolding)
                    SetState(PlayerFSM.State.BatteryCharging);
                hand.ChargeTowerInFront(batteryChargeSpeed);
            }
                
        }
        else if (Input.GetKeyUp(KeyCode.C))
        {
            if (CurrentState == PlayerFSM.State.BatteryCharging)
                SetState(PlayerFSM.State.Idle);
        }
    }
    IEnumerator Idle()
    {
        do
        {
            if (isHolding)
            {
                SetState(State.HoldingIdle);
                break;
            }
            if (inputVector != Vector2.zero)
                SetState(State.Walk);
            yield return null;
        } while (!isNewState);
    }
    IEnumerator Walk()
    {
        movementComponent.enabled = true;
        do
        {
            if (isHolding)
            {
                SetState(State.HoldingWalk);
                break;
            }
            if (inputVector == Vector2.zero)
                SetState(State.Idle);
            yield return null;
        } while (!isNewState);
        movementComponent.direction = inputVector;
        movementComponent.enabled = false;
    }
    IEnumerator HoldingIdle()
    {
        do
        {
            if (!isHolding)
            {
                SetState(State.Idle);
                break;
            }
            if (inputVector != Vector2.zero)
                SetState(State.HoldingWalk);
            yield return null;
        } while (!isNewState);
    }
    IEnumerator HoldingWalk()
    {
        movementComponent.enabled = true;
        do
        {
            if (!isHolding)
            {
                SetState(State.Walk);
                break;
            }
            if (inputVector == Vector2.zero)
                SetState(State.HoldingIdle);
            yield return null;
        } while (!isNewState);
        movementComponent.direction = inputVector;
        movementComponent.enabled = false;
    }
    IEnumerator BatteryCharging()
    {
        do
        {
            yield return null;
        } while (!isNewState);
    }
}

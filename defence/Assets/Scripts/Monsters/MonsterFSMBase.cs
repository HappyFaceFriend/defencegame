using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonsterFSMBase : MonoBehaviour
{
    public enum State
    {
        Idle=0, Walk=1, Attack=2, Dead = 3, EndOfRoad = 4
    }
    public State CurrentState { get { return currentState; } }

    [Header("Base References")]
    [SerializeField] protected Transform imageTransform;

    [Header("Base Stats")]
    [ReadOnly] [SerializeField] protected State currentState;
    [SerializeField] protected float maxHp;
    [SerializeField] protected float currentHp;
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected Transform hpBarPosition;

    protected SpriteRenderer spriteRenderer;
    protected StatusBar hpBar;

    protected bool isNewState;
    new protected Rigidbody2D rigidbody;
    protected MovementController movementController;
    protected RouteMovement movementComponent;
    protected Animator animator;


    public void Init(StatusBar hpBar)
    {
        currentHp = maxHp;
        this.hpBar = hpBar;
        hpBar.Init(maxHp, transform, hpBarPosition.localPosition);
        SetState(State.Walk);
    }

    public void SetMoveSpeed(float moveSpeed)
    {
        this.moveSpeed = moveSpeed;
        movementComponent.MoveSpeed = moveSpeed;
    }
    public void MultiplyMoveSpeed(float moveSpeedAlpha)
    {
        this.moveSpeed *= moveSpeedAlpha;
        movementComponent.MoveSpeed *= moveSpeedAlpha;
    }
    public void TakeDamage(float damage)
    {
        currentHp -= damage;
        hpBar.Value = currentHp;
        if (currentHp <= 0)
        {
            currentHp = 0;
            SetState(State.Dead);
        }
    }

    protected void Awake()
    {

        rigidbody = GetComponent<Rigidbody2D>();
        movementController = gameObject.AddComponent<MovementController>();
        movementComponent = GetComponent<RouteMovement>();
        movementComponent.MoveSpeed = moveSpeed;
        animator = imageTransform.GetComponent<Animator>();
        spriteRenderer = imageTransform.GetComponent<SpriteRenderer>();
    }
    protected void Start()
    {
        StartCoroutine(FSMMain());
    }
    private void OnDestroy()
    {
        if(hpBar != null)
            Destroy(hpBar.gameObject);
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
        animator.SetInteger("State", (int)state);
        currentState = state;
        isNewState = true;
    }
    IEnumerator Dead()
    {
        do
        {
            yield return null;
            if (AnimUtils.IsDone(animator,"Dead"))
                Destroy(gameObject);
        } while (!isNewState);
    }
}

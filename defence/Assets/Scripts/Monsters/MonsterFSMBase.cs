using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonsterFSMBase : MonoBehaviour
{
    public enum State
    {
        Idle=0, Walk=1, Attack=2, Dead = 3
    }
    public State CurrentState { get { return currentState; } }
    [ReadOnly][SerializeField] protected State currentState;
    protected bool isNewState;

    protected Rigidbody2D rigidbody;
    protected MovementController movementController;

    [Header("Base References")]
    [SerializeField] protected Transform imageTransform;
    public Animator animator;
    protected SpriteRenderer spriteRenderer;

    [Header("Base Stats")]
    [SerializeField] protected float maxHp;
    [SerializeField] protected float moveSpeed;
    [ReadOnly] [SerializeField] protected float currentHp;


    protected void Awake()
    {

        rigidbody = GetComponent<Rigidbody2D>();
        movementController = gameObject.AddComponent<MovementController>();
        animator = imageTransform.GetComponent<Animator>();
        spriteRenderer = imageTransform.GetComponent<SpriteRenderer>();
        SetState(State.Walk);

        currentHp = maxHp;
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
        animator.SetInteger("State", (int)state);
        currentState = state;
        isNewState = true;
    }
    public void TakeDamage(float damage)
    {
        currentHp -= damage;
        if(currentHp <= 0)
        {
            SetState(State.Dead);
        }
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

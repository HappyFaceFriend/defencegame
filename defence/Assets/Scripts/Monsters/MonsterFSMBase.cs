using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonsterFSMBase : MonoBehaviour
{
    public enum State
    {
        Idle=0, Walk=1, Attack=2
    }
    [ReadOnly][SerializeField] protected State currentState;
    protected bool isNewState;

    protected Rigidbody2D rigidbody;
    protected MovementController movementController;

    [Header("Base References")]
    [SerializeField] protected Transform imageTransform;
    protected Animator animator;
    protected SpriteRenderer spriteRenderer;


    protected void Awake()
    {
        currentState = State.Idle;

        rigidbody = GetComponent<Rigidbody2D>();
        movementController = gameObject.AddComponent<MovementController>();
        animator = imageTransform.GetComponent<Animator>();
        spriteRenderer = imageTransform.GetComponent<SpriteRenderer>();
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
        if(animator != null)
            animator.SetInteger("State", (int)currentState);
    }
}

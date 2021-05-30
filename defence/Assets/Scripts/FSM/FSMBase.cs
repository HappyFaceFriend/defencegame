using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FSMBase : MonoBehaviour
{
    public enum State
    {
        Idle=0, Walk=1, Attack=2, Other=10
    }
    [SerializeField] protected State currentState;
    [SerializeField] protected bool isNewState;

    protected Rigidbody2D rigidbody;
    protected MovementController movementController;

    [SerializeField]
    protected Transform imageTransform;
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
    public void SetIntParam(string paramName, int i)
    {
        if (animator != null)
            animator.SetInteger(paramName, i);
    }
    public void SetBoolParam(string paramName, bool b)
    {
        if (animator != null)
            animator.SetBool(paramName, b);
    }
    protected void SetAnimTrigger(string triggerName)
    {
        if (animator != null)
            animator.SetTrigger(triggerName);
    }

    protected void DisableComponent(MonoBehaviour component)
    {
        component.enabled = false;
    }
    protected void EnableComponent(MonoBehaviour component)
    {
        component.enabled = true;
    }
    private void FixedUpdate()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerFSMBase : MonoBehaviour
{

    public float Battery { get { return battery; } }
    public Transform ImagesTransform{ get { return imagesTransform; } }

    [Header("Base References")]
    [SerializeField] protected LevelManager levelManager;
    [SerializeField] protected Transform imagesTransform;
    [SerializeField] protected Transform batteryOut;


    [Header("Base Stats")]
    [ReadOnly][SerializeField] float battery = 100;
    [SerializeField] protected bool worksWhileHolding;

    public enum State
    {
        Idle = 0, Attack = 1, BatteryOut = 2, Held = 3
    }
    [ReadOnly] [SerializeField] protected State currentState;
    protected bool isNewState;


    
    protected Animator animator;

    protected void Awake()
    {
        currentState = State.Idle;
        animator = imagesTransform.GetComponent<Animator>();

    }

    protected void Start()
    {
        StartCoroutine(FSMMain());
    }
    protected void Update()
    {
        if(battery <= 0)
            batteryOut.gameObject.SetActive(true);
        else
            batteryOut.gameObject.SetActive(false);
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

    public void SetBattery(float battery)
    {
        this.battery = battery;
        if(battery<=0)
        {
            this.battery = 0;
            SetState(State.BatteryOut);
        }
        if (battery >= 100)
            this.battery = 100;
    }

    public void SetHeldByPlayer(bool isHeld)
    {
        if (isHeld && !worksWhileHolding)
            SetState(State.Held);  
        else
            SetState(State.Idle);
    }
    protected virtual IEnumerator BatteryOut()
    {
        do
        {
            if (Battery > 10)
                SetState(State.Idle);
            yield return null;
        } while (!isNewState);
    }
    protected virtual IEnumerator Held()
    {
        do
        {
            yield return null;
        } while (!isNewState);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerFSMBase : HoldableObject
{
    public enum State
    {
        Idle = 0, Attack = 1, BatteryOut = 2, Held = 3, Ready = 4
    }
    public float Battery { get { return battery; } }

    [Header("Base References")]
    [SerializeField] protected Transform imagesTransform;
    [SerializeField] protected Transform batteryOut;
    [SerializeField] protected Transform energyBarPosition;

    [Header("Base Stats")]
    [ReadOnly] [SerializeField] float battery = 100;
    [ReadOnly] [SerializeField] protected State currentState;
    [SerializeField] protected bool worksWhileHolding;

    protected LevelManager levelManager;
    protected Animator animator;
    protected bool isNewState;
    protected StatusBar energyBar;

    public void Init(LevelManager levelManager, StatusBar energyBar)
    {
        this.levelManager = levelManager;
        this.energyBar = energyBar;
        energyBar.Init(100, transform, energyBarPosition.localPosition);
    }
    protected void Awake()
    {
        animator = imagesTransform.GetComponent<Animator>();
        imageCopyTransform = imagesTransform;
        SetState(State.Ready);
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
        energyBar.Value = battery;
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
    protected virtual IEnumerator Ready()
    {
        do
        {
            yield return null;
        } while (!isNewState);
    }

}

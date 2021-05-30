using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerFSMBase : FSMBase
{
    [SerializeField] protected LevelManager levelManager;

    [SerializeField] protected bool worksWhileHolding;

    public float Battery { get { return battery; } }

    [SerializeField] float battery = 100;
    public enum OtherState
    { Held=0, BatteryOut=1, NotOther=10 }
    protected OtherState currentOtherState;

    new void Awake()
    {
        base.Awake();
    }

    public void SetBattery(float battery)
    {
        this.battery = battery;
        if(battery<=0)
        {
            battery = 0;
            SetState(OtherState.BatteryOut);
        }
    }

    public void SetHeldByPlayer(bool isHeld)
    {
        if (isHeld && !worksWhileHolding)
            SetState(OtherState.Held);  
        else
            SetState(State.Idle);
    }
    protected void SetState(OtherState otherState)
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
    IEnumerator BatteryOut()
    {
        do
        {
            if (Battery > 10)
                SetState(State.Idle);
            yield return null;
        } while (!isNewState);
    }
}

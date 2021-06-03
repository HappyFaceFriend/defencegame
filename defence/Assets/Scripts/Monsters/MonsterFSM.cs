using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterFSM : MonsterFSMBase
{
    FlipSpriteToPoint spriteFlipComponent;
    Vector3 nextTargetPoint;


    new void Awake()
    {
        base.Awake();
        spriteFlipComponent = GetComponent<FlipSpriteToPoint>();

    }
    void Update()
    {
        spriteFlipComponent.targetPoint = transform.position + VectorUtils.Vec2toVec3(movementComponent.Direction);
    }
    IEnumerator Idle()
    {
        do
        {
            SetState(State.Walk);
            yield return null;
        } while (!isNewState);
    }
    IEnumerator Walk()
    {
        movementComponent.enabled = true;
        do
        {
            if (movementComponent.IsEndOfRoute)
                SetState(State.EndOfRoad);
            yield return null;
        } while (!isNewState);
        movementComponent.enabled = false;
    }
    IEnumerator EndOfRoad()
    {
        do
        {
            yield return null;
        } while (!isNewState);
    }
}

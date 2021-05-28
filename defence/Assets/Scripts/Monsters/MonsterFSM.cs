using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterFSM : FSMBase
{
    RouteMovement movementComponent;
    FlipSpriteToPoint spriteFlipComponent;
    Vector3 nextTargetPoint;


    new void Awake()
    {
        base.Awake();
        movementComponent = GetComponent<RouteMovement>();
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
        EnableComponent(movementComponent);
        do
        {
            yield return null;
        } while (!isNewState);
        DisableComponent(movementComponent);
    }
}

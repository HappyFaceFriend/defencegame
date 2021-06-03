using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RouteMovement : MovementBase
{
    public RouteManager routeManager {  set { _routeManager = value; } }
    public Vector2 Direction { get { return direction; } }
    public bool IsEndOfRoute { get { return isEndOfRoute; } }

    RouteManager _routeManager;
    Vector3 currentTarget;
    int currentIndex;
    bool isEndOfRoute;
    Vector2 direction;
    void Awake()
    {
        isEndOfRoute = false;
        currentIndex = 0;
    }
    private void Start()
    {
        SetNextPoint(); 
    }

    void SetNextPoint()
    {
        currentIndex++;
        if(currentIndex >= _routeManager.TargetCount)
        {
            MoveSpeed = 0;
            currentIndex--;
            isEndOfRoute = true;
        }
        currentTarget = _routeManager.GetTargetPoint(currentIndex);
    }

    public override Vector2 ApplyMovement(bool isPlayingKnockback)
    {
        Vector3 moveDelta = Vector3.MoveTowards(transform.position, currentTarget, MoveSpeed * Time.fixedDeltaTime) - transform.position;
        if (moveDelta == Vector3.zero)
        {
            SetNextPoint();
        }
        direction = moveDelta;

        return moveDelta;
    }
}

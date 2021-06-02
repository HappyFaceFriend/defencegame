using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RouteMovement : MovementBase
{
    public RouteManager routeManager {  set { _routeManager = value; } }

    public Vector2 Direction { get { return direction; } }

    RouteManager _routeManager;
    Vector3 currentTarget;
    int currentIndex;
    Vector2 direction;
    void Awake()
    {
        currentIndex = -1;
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

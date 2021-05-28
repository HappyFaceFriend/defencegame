using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RouteMovement : MovementBase
{
    public RouteManager routeManager {  set { _routeManager = value; } }

    [SerializeField] float _moveSpeed;
    public float moveSpeed { get { return _moveSpeed; } set { _moveSpeed = value; } }
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
            moveSpeed = 0;
            currentIndex--;
        }
        currentTarget = _routeManager.GetTargetPoint(currentIndex);
    }

    public override Vector2 ApplyMovement(bool isPlayingKnockback)
    {
        Vector3 moveDelta = Vector3.MoveTowards(transform.position, currentTarget, moveSpeed * Time.fixedDeltaTime) - transform.position;
        if (moveDelta == Vector3.zero)
        {
            SetNextPoint();
        }
        direction = moveDelta;

        return moveDelta;
    }
}

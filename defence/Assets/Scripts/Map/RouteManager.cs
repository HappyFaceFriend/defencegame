using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RouteManager : MonoBehaviour
{
    [SerializeField] Transform [] targetPoints;

    public int TargetCount { get { return targetPoints.Length; } }

    public Vector3 GetTargetPoint(int index)
    {
        return targetPoints[index].position;
    }

}

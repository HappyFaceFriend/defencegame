using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionMovement : MovementBase
{
    [SerializeField]
    float _moveSpeed;
    public float moveSpeed { get { return _moveSpeed; } set { _moveSpeed = value; } }
    public Vector2 direction { get; set; }


    public override Vector2 ApplyMovement(bool isPlayingKnockback)
    {
        return direction * moveSpeed * Time.fixedDeltaTime;
    }
}

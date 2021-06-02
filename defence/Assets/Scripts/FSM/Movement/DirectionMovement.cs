using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionMovement : MovementBase
{

    public Vector2 direction { get; set; }


    public override Vector2 ApplyMovement(bool isPlayingKnockback)
    {
        return direction * MoveSpeed * Time.fixedDeltaTime;
    }
}

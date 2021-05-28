using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovementBase : MonoBehaviour
{
    /// <summary>
    /// returns a movement which is added at FixedUpdate(). this function is called every FixedUpdate().
    /// </summary>
    public abstract Vector2 ApplyMovement(bool isPlayingKnockback);
}

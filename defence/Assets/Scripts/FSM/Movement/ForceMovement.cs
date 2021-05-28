using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ForceMovement : MovementBase
{
    public bool IsPlaying { get { return enabled; } }
    [SerializeField]
    float duration;
    [SerializeField]
    float distance;


    float eTime;
    Vector2 direction;

    public void Begin(Vector2 direction)
    {
        eTime = 0;
        enabled = true;
        this.direction = direction;
    }
    void OnDisable()
    {
        eTime = 0;
    }
    abstract protected float EaseFunction(float t);
    public override Vector2 ApplyMovement(bool isPlayingKnockback)
    {
        float moveAmount = Mathf.Lerp(0, distance, EaseFunction((eTime + Time.fixedDeltaTime / duration) / duration)) -
                Mathf.Lerp(0, distance, EaseFunction(eTime / duration));
        eTime += Time.fixedDeltaTime / duration;
        if (eTime >= duration)
        {
            enabled = false;
            return Vector2.zero;
        }
        return direction * moveAmount;
    }
    
}

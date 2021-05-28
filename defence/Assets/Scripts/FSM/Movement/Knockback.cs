using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockback : MovementBase
{
    public enum Type { Slow, Quick }

    public Vector2 direction { get; set; }

    public bool IsPlaying { get; set; }
    float targetDistance;

    float duration = 0.3f;
    float eTime = 0f;

    public void ApplyKnockback(Vector2 direction, float distance, Type type)
    {
        if (type == Type.Quick)
            duration = 0.3f;
        else if (type == Type.Slow)
            duration = 0.5f;
        this.direction = direction;
        targetDistance = distance;
        eTime = 0;
        IsPlaying = true;
    }


    float Easeout(float time)
    {
        return -(time - 1) * (time - 1) + 1;
    }

    public override Vector2 ApplyMovement(bool isPlayingKnockback)
    {
        if (IsPlaying)
        {
            float moveAmount = Mathf.Lerp(0, targetDistance, Easeout((eTime + Time.fixedDeltaTime / duration) / duration)) -
                Mathf.Lerp(0, targetDistance, Easeout(eTime / duration));
            eTime += Time.fixedDeltaTime / duration;
            if (eTime >= duration)
            {
                IsPlaying = false;
                return Vector2.zero;
            }
            return direction * moveAmount;
        }
        else
            return Vector2.zero;
    }
}

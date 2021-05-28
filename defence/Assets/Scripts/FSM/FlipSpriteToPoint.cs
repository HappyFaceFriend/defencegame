using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipSpriteToPoint : MonoBehaviour
{
    public Vector2 targetPoint { get; set; }

    [SerializeField]
    SpriteRenderer spriteRenderer = null;

    void Update()
    {
        if (targetPoint.x > transform.position.x)
        {
            spriteRenderer.flipX = false;
        }
        //    
        else if (targetPoint.x < transform.position.x)
        {
            spriteRenderer.flipX = true;
        }
        //    
    }
}

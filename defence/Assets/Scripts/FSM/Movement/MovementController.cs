using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    List<MovementBase> movementList = new List<MovementBase>();
    //Knockback knockback;

    Rigidbody2D rigidbody;
    void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        movementList.AddRange(GetComponents<MovementBase>());
        if (movementList.Count == 0)
            this.enabled = false;
        //knockback = GetComponent<Knockback>();
    }
    void FixedUpdate()
    {
        Vector2 moveVector = Vector2.zero;
        foreach(MovementBase movement in movementList)
        {
            if(movement.enabled)
            {

                //moveVector += movement.ApplyMovement(knockback.IsPlaying);
                moveVector += movement.ApplyMovement(false);
            }
        }
        rigidbody.MovePosition(rigidbody.position + moveVector);
    }

}

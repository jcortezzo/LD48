using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MovableEntity
{ 
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        
        if (IsMovingUp())
            rb.AddForce(COUNTER_JUMP_FORCE * Vector2.down * rb.mass);

    }


    private void OnCollisionStay2D(Collision2D collision)
    {
        bool colliedFloor = collision.gameObject.CompareTag("Floor");
        if (colliedFloor)
        {
            //if (colliedEnemy) bounce = false;
            foreach (ContactPoint2D point in collision.contacts)
            {
                if (point.normal.y >= CAN_JUMP_THRESHHOLD)
                {
                    timeSinceGrounded = 0f;
                }

            }
        }
    }

}

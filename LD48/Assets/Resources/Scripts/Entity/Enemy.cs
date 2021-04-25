using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MovableEntity
{
    Collider2D collider;

    [HeaderAttribute("Gender")]
    [SerializeField] private bool isGirl;
    [SerializeField] private bool randomGender;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        collider = GetComponent<Collider2D>();
        Random r = new Random();
        if (randomGender) isGirl = Random.Range(0f, 1f) > 0.5f;
    }

    // Update is called once per frame
    public override void Update()
    {
        anim.SetBool("isGirl", isGirl);
        base.Update();
    }

    public override void FixedUpdate()
    {
        // don't move if someone is right in front of you
        RaycastHit2D[] results = new RaycastHit2D[1];
        int hits = collider.Cast(
                new Vector2(rb.velocity.x > 0 ?  1 : 
                            rb.velocity.x < 0 ? -1 : 0, 0),
                results, 0.1f, false
        );
        if (hits == 0)
        {
            base.FixedUpdate();
        }
        else
        {
            SetDirection(Direction.IDLE);
        }
        
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

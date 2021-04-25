using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MovableEntity
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
        // Buffer for space presses
        if (Input.GetKeyDown(KeyCode.Space))
        {
            timeSinceJumpKeyPressed = 0f;
        }
        else
            timeSinceJumpKeyPressed += Time.deltaTime;

        // Checking if spacebar held (no buffer)
        if (Input.GetKeyDown(KeyCode.Space))
            spacebarHeld = true;
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            spacebarHeld = false;
            // Switches from red spacebar to white spacebar
        }

        timeSinceGrounded += Time.deltaTime; // janky :(

        anim.SetFloat("VerticalSpeed", rb.velocity.y);
        anim.SetBool("CanJump", (timeSinceJumpKeyPressed < JUMP_PRESS_BUFFER && timeSinceGrounded < COYOTE_BUFFER));
        anim.SetBool("IsInDialogue", DialogueManager.Instance.IsInDialogue);
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        if (stunTimer > 0)
        {
            return;
        }

        if (timeSinceJumpKeyPressed < JUMP_PRESS_BUFFER && timeSinceGrounded < COYOTE_BUFFER)
            Jump();

        // counter force
        // space bar hold = no counter, release = fall quicker
        // if bounce, then no counter
        if (IsMovingUp() && (!spacebarHeld)) 
            rb.AddForce(COUNTER_JUMP_FORCE * Vector2.down * rb.mass);

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        bool colliedFloor = collision.gameObject.CompareTag("Floor");
        bool colliedEnemy = collision.gameObject.CompareTag("Enemy");
        if (colliedFloor || colliedEnemy)
        {
            foreach (ContactPoint2D point in collision.contacts)
            {
                if (point.normal.y >= CAN_JUMP_THRESHHOLD)
                {
                    timeSinceGrounded = 0f;
                    if (colliedEnemy)
                    {
                        return;
                    }
                }
                
            }
        }

        if (colliedEnemy && !DialogueManager.Instance.IsInDialogue)
        {
            DialogueManager.Instance.SetDialogueEntities(this, collision.gameObject.GetComponent<MovableEntity>());
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        bool colliedFloor = collision.gameObject.CompareTag("Floor");
        bool colliedEnemy = collision.gameObject.CompareTag("Enemy");
        if (colliedFloor || colliedEnemy)
        {
            //if (colliedEnemy) bounce = false;
            foreach (ContactPoint2D point in collision.contacts)
            {
                if (point.normal.y >= CAN_JUMP_THRESHHOLD)
                {
                    timeSinceGrounded = 0f;
                    if (colliedEnemy)
                    {
                        Jump();
                        collision.gameObject.GetComponent<DialogueEntity>().DisplaySentence("Oucchh");
                        //return;
                    }
                }

            }
        }
    }

    public void Stun(float n)
    {
        stunTimer = n;
        prevVelocity = rb.velocity;
        rb.velocity = Vector2.zero;
        rb.gravityScale = 0;
    }
}

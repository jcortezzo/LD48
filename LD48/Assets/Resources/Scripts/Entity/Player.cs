using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MovableEntity
{

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        facingRight = true;
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

        if (isTalking)
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
                
                
                //if(point.normal.x >= CAN_TALK_THRESHOLD)
                //{
                //    if (colliedEnemy)
                //    {
                //        return;
                //    }
                //}
            }
        }
        float enemyHeight = collision.gameObject.GetComponent<SpriteRenderer>().bounds.size.y;
        // no collision if player is higher than enemy
        if (colliedEnemy && this.transform.position.y > collision.transform.position.y + enemyHeight / 2)
        {
            return;
        }

        // no collision if player is ahead of enemy
        // if game direction is moving to the right, then checking for p.x > e.x
        // if game direction is moving to the left, then checking for p.x < e.x
        if (colliedEnemy && ((GlobalManager.Instance.gameDirection == Direction.RIGHT && this.transform.position.x > collision.transform.position.x) ||
                            (GlobalManager.Instance.gameDirection == Direction.LEFT && this.transform.position.x < collision.transform.position.x)))
        {
            return;
        }

        if (colliedEnemy && !DialogueManager.Instance.IsInDialogue)
        {
            DialogueManager.Instance.SetDialogueEntities(this, collision.gameObject.GetComponent<MovableEntity>());        }
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
                        PooProgress.Instance.PushPooDeeper();
                        collision.gameObject.GetComponent<DialogueEntity>().DisplaySentence("Oucchh");
                        //return;
                    }
                }

            }
        }
    }
}

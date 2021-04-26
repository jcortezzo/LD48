using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MovableEntity
{
    private Collider2D enemyCollider;
    [SerializeField] private float distance;
    [HeaderAttribute("Gender")]
    [SerializeField] private bool isGirl;
    [SerializeField] private bool randomGender;
    RaycastHit2D hit;
    private bool isSomethingInFront;
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        enemyCollider = GetComponent<Collider2D>();
        Random r = new Random();
        if (randomGender) isGirl = Random.Range(0f, 1f) > 0.5f;
        facingRight = true;

        movingDirection = GlobalManager.Instance.gameDirection == Direction.RIGHT ? Direction.LEFT : Direction.RIGHT;
    }

    // Update is called once per frame
    public override void Update()
    {
        anim.SetBool("isGirl", isGirl);
        base.Update();
        Vector3 offsetDir = new Vector2(rb.velocity.x > 0 ? 1 : rb.velocity.x < 0 ? -1 : 0, 0);
        hit = Physics2D.Raycast(this.transform.position, offsetDir, distance);

        Debug.DrawRay(this.transform.position, offsetDir * distance);
    }

    public override void FixedUpdate()
    {
        // don't move if someone is right in front of you


        if(isSomethingInFront)
        {
            //Debug.Log("not moving: " + hit.collider.name + (this.transform.position));
            //Debug.Log("not moving: ");
            SetDirection(Direction.IDLE);
        }
        else
        {
            //Debug.Log("moving left");
            if(!isTalking) SetDirection(movingDirection);
            base.FixedUpdate();
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


    private void OnTriggerEnter2D(Collider2D collision)
    {
        bool colliedPlayer = collision.CompareTag("Player");
        bool colliedEnemy = collision.CompareTag("Enemy");
        if (colliedPlayer || colliedEnemy)
        {
            isSomethingInFront = true;
            if (colliedPlayer && !DialogueManager.Instance.IsInDialogue)
            {
                DialogueManager.Instance.SetDialogueEntities(collision.gameObject.GetComponent<MovableEntity>(), this);
            }
        }
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("Enemy"))
        {
            isSomethingInFront = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("Enemy"))
        {
            isSomethingInFront = false;
        }
    }
}

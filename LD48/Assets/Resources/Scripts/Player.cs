using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;

    [SerializeField] private float speed;
    [SerializeField] private float JUMP_FORCE;  // readonly
    [SerializeField] private float COUNTER_JUMP_FORCE;  // readonly
    private bool facingRight;

    [SerializeField] private Direction movingDirection = Direction.IDLE;

    private const float CAN_JUMP_THRESHHOLD = 0.05f;
    private const float JUMP_PRESS_BUFFER = 0.1f;
    private const float COYOTE_BUFFER = 0.1f;

    private bool spacebarHeld;
    private float timeSinceGrounded;  // e.g. canJump
    private float timeSinceJumpKeyPressed; // tracks when spacebar is pressed used w/ buffering
    private bool bounce;
    
    public bool isTalking;  // While a player is talking they can't walk or jump


    public float stunTimer = 0f;

    private Animator anim;

    private Vector2 prevVelocity;
    private float prevGravity;

    [SerializeField]
    private GameObject fadeDead;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        facingRight = true;
        spacebarHeld = false;
        timeSinceGrounded = float.PositiveInfinity;
        timeSinceJumpKeyPressed = float.PositiveInfinity;
        rb.freezeRotation = true;
        prevVelocity = Vector2.zero;
        prevGravity = rb.gravityScale;
        bounce = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (stunTimer > 0)
        {
            stunTimer -= Time.deltaTime;
            if (stunTimer <= 0)
            {
                Debug.Log(prevGravity);
                rb.velocity = prevVelocity;
                rb.gravityScale = prevGravity;
            }
            else
            {
                return;
            }
        }

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
    }

    private void FixedUpdate()
    {
        if (stunTimer > 0)
        {
            return;
        }
        Vector2 newVeloc = AutoMove(movingDirection);

        rb.velocity = isTalking ? new Vector2(0, rb.velocity.y) : newVeloc;
        if (timeSinceJumpKeyPressed < JUMP_PRESS_BUFFER && timeSinceGrounded < COYOTE_BUFFER)
            Jump();

        // counter force
        // space bar hold = no counter, release = fall quicker
        // if bounce, then no counter
        if (IsMovingUp() && (!spacebarHeld || !bounce)) 
            rb.AddForce(COUNTER_JUMP_FORCE * Vector2.down * rb.mass);

    }

    public void SetDirection(Direction direction)
    {
        movingDirection = direction;
    }
    
    public void Die()
    {

    }

    public bool IsStunned()
    {
        return stunTimer > 0;
    }

    public Vector2 AutoMove(Direction dir)
    {
        float horizontal = 0;
        if (dir == Direction.IDLE) horizontal = 0;
        else if (dir == Direction.LEFT) horizontal = -1;
        else horizontal = 1;
        return Walk(horizontal);
    }

    private Vector2 Walk(float horizontal)
    {
        anim.SetFloat("HorizontalSpeed", Mathf.Abs(horizontal));
        if (horizontal != 0)
        {
            bool prev = facingRight;
            facingRight = horizontal > 0;
            int flip = prev == facingRight ? 1 : -1;
            transform.localScale = new Vector3(flip * transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
        return new Vector2(horizontal * speed * Time.deltaTime, rb.velocity.y);
    }

    private Vector2 Walk()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        return Walk(horizontal);
    }


    void Jump()
    {
        if (isTalking) return;
        anim.SetTrigger("Jump");
        timeSinceGrounded = float.PositiveInfinity;  // Prevents us from double-jumping
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(JUMP_FORCE * Vector2.up * rb.mass, ForceMode2D.Impulse);
    }

    public bool CanJump()
    {
        return timeSinceJumpKeyPressed < JUMP_PRESS_BUFFER && timeSinceGrounded < COYOTE_BUFFER;
    }

    private bool IsMovingUp()
    {
        return Vector2.Dot(rb.velocity, Vector2.up) > 0;
    }

    public bool IsRight()
    {
        return facingRight;
    }

    public void DisableMovement()
    {
        isTalking = true;
    }

    public void EnableMovement()
    {
        isTalking = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        bool colliedFloor = collision.gameObject.CompareTag("Floor");
        bool colliedEnemy = collision.gameObject.CompareTag("Enemy");
        if (colliedFloor || colliedEnemy)
        {
            if (colliedEnemy) bounce = false; 
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
            DialogueManager.Instance.SetDialogueEntities(this, collision.gameObject.GetComponent<DialogueEntity>());
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
                        bounce = true;
                        Jump();
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

public enum Direction
{
    LEFT, RIGHT, IDLE
}
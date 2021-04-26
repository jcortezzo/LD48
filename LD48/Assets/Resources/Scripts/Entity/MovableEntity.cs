using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MovableEntity : MonoBehaviour
{
    public Rigidbody2D rb;

    [SerializeField] public float speed;
    [SerializeField] public float JUMP_FORCE;  // readonly
    [SerializeField] public float COUNTER_JUMP_FORCE;  // readonly
    public bool facingRight;
    private SpriteRenderer sr; 
    [SerializeField] private Direction movingDirection = Direction.IDLE;

    public const float CAN_JUMP_THRESHHOLD = 0.05f;
    public const float CAN_TALK_THRESHOLD = 0.05f;
    public const float JUMP_PRESS_BUFFER = 0.1f;
    public const float COYOTE_BUFFER = 0.1f;

    public bool spacebarHeld;
    public float timeSinceGrounded;  // e.g. canJump
    public float timeSinceJumpKeyPressed; // tracks when spacebar is pressed used w/ buffering

    public bool isTalking;  // While a player is talking they can't walk or jump
    public EntityType entityType;

    public float stunTimer = 0f;

    public Animator anim;

    public Vector2 prevVelocity;
    public float prevGravity;

    private RectTransform dialogueText;

    [SerializeField]
    private GameObject fadeDead;
    // Start is called before the first frame update
    public virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        dialogueText = GetComponentInChildren<RectTransform>();

        spacebarHeld = false;
        timeSinceGrounded = float.PositiveInfinity;
        timeSinceJumpKeyPressed = float.PositiveInfinity;
        rb.freezeRotation = true;
        prevVelocity = Vector2.zero;
        prevGravity = rb.gravityScale;
    }

    // Update is called once per frame
    public virtual void Update()
    {
        //if (stunTimer > 0)
        //{
        //    stunTimer -= Time.deltaTime;
        //    if (stunTimer <= 0)
        //    {
        //        Debug.Log(prevGravity);
        //        rb.velocity = prevVelocity;
        //        rb.gravityScale = prevGravity;
        //    }
        //    else
        //    {
        //        return;
        //    }
        //}
        if (isTalking) SetDirection(Direction.IDLE);
        if (transform.localScale.x < 0)
        {
            Debug.Log("scale fliped");
            dialogueText.transform.localScale = new Vector3(-1, dialogueText.transform.localScale.y, dialogueText.transform.localScale.z);
        }
        else
        {
            dialogueText.transform.localScale.Set(1, 1, 1);
        }

        timeSinceGrounded += Time.deltaTime; // janky :(

        anim.SetFloat("VerticalSpeed", rb.velocity.y);
        anim.SetFloat("HorizontalSpeed", Mathf.Abs(rb.velocity.x));
        anim.SetBool("CanJump", (timeSinceJumpKeyPressed < JUMP_PRESS_BUFFER && timeSinceGrounded < COYOTE_BUFFER));
    }

    public virtual void FixedUpdate()
    {
        if (stunTimer > 0)
        {
            return;
        }
        Vector2 newVeloc = AutoMove(movingDirection);

        rb.velocity = newVeloc;
        //rb.velocity = isTalking ? new Vector2(0, rb.velocity.y) : newVeloc;

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
        
        if (horizontal != 0)
        {
            Debug.Log(horizontal);
            bool prev = facingRight;
            facingRight = horizontal > 0;
            int flip = prev == facingRight ? 1 : -1;
            //sr.flipX = horizontal < 0;

            transform.localScale = new Vector3(flip * transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
        return new Vector2(horizontal * speed * Time.deltaTime, rb.velocity.y);
    }

    private Vector2 Walk()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        return Walk(horizontal);
    }


    public void Jump()
    {
        if (DialogueManager.Instance.IsInDialogue) return;  // isTalking previously
        anim.SetTrigger("Jump");
        timeSinceGrounded = float.PositiveInfinity;  // Prevents us from double-jumping
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(JUMP_FORCE * Vector2.up * rb.mass, ForceMode2D.Impulse);
    }

    public bool CanJump()
    {
        return timeSinceJumpKeyPressed < JUMP_PRESS_BUFFER && timeSinceGrounded < COYOTE_BUFFER;
    }

    public bool IsMovingUp()
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

    }

    private void OnCollisionStay2D(Collision2D collision)
    {

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

public enum EntityType 
{ 
    PLAYER, KAREN_FEMALE, KAREN_MALE, BIRD, DOG
}

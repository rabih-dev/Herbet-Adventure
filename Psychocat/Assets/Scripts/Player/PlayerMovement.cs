using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;

    [Header("Movement Values")]
    public float moveSpeed;
    public float jumpStrength;   
    private float horizontalAxis;

    [Header("Jumping Refinement")]
    public float fallMultiplier;
    public float hangTime;
    private float hangTimeCounter;
    public float jumpBufferTime;
    private float jumpBufferCounter;
    [Tooltip("0.5f is generally a good number")]
    [Range(0f,1f)]
    public float jumpCutAmount;

    
    
    //groundcheck stuff
    [Header("Ground Check")]
    [SerializeField] private Vector2 groundCheckOffset;
    private Vector2 groundCheckPos;
    [SerializeField] private float groundCheckRadius;
    private bool isOnGround;
    [SerializeField] private LayerMask platform;

    [HideInInspector] public bool canMove;

    private void Start()
    {
        canMove = true;
        hangTimeCounter = hangTime;
    }

    private void Update()
    {
        GettingInputs();
        JumpFall();
        HangTimeCheck();       
    }

    void FixedUpdate()
    {       
        GroundChecking();
        Walking();
        Jump();
    }

    #region functions

    void GettingInputs()
    {

        JumpBuffering();
        horizontalAxis = Input.GetAxisRaw("Horizontal");
        if (Input.GetButtonUp("Jump"))
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }
    }

    void JumpBuffering()
    {
        if (Input.GetButtonDown("Jump"))
            {
            jumpBufferCounter = jumpBufferTime;
            }

        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }
    }

    void Walking() 
    {
        if (canMove)
        {
            rb.velocity = new Vector2(horizontalAxis * moveSpeed, rb.velocity.y);
            if (rb.velocity.x > 0.01f)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }

            else if (rb.velocity.x < -0.01f)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
        }
        
    }

    void Jump()
    {
        if (canMove && hangTimeCounter > 0 && jumpBufferCounter > 0)
        {
            hangTimeCounter = 0;
            rb.velocity = (new Vector2(rb.velocity.x, jumpStrength));
        }      
    }

    void JumpFall()
    {
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier  -1) * Time.deltaTime;
        }
    }

    

    void HangTimeCheck()
    {
        if (isOnGround)
        {
            hangTimeCounter = hangTime;
        }

        else
        {
            hangTimeCounter -= Time.deltaTime;
        }
    }


    void GroundChecking()
    {
        groundCheckPos = new Vector2(transform.position.x + groundCheckOffset.x, transform.position.y + groundCheckOffset.y);
        isOnGround = Physics2D.OverlapCircle(groundCheckPos, groundCheckRadius, platform);       
    }

     public IEnumerator StoppingMovement(float howMuchTime)
    {
        rb.velocity = Vector2.zero;
        canMove = false;
        yield return new WaitForSeconds(howMuchTime);
        canMove = true;        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("MovingPlatform") && transform.position.y > collision.transform.position.y)
        {
            transform.SetParent(collision.transform);                          
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("MovingPlatform"))
        {
            transform.SetParent(null);   
        }
    }

    private void OnDrawGizmos()
    {
        //debug do groundcheck
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(groundCheckPos, groundCheckRadius);
    }
    #endregion functions

}

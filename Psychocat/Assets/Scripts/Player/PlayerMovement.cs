using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    private bool facingRight;

    [Header("Movement Values")]
    public float moveSpeed;
    public float jumpStrength;
    private float horizontalAxis;
    public float fallMultiplier;
    [Tooltip("0.5f is generally a good number")]
    [Range(0f, 1f)]
    public float jumpCutAmount;

    [Header("Double Jump")]
    public int amountOfJumps;
    private int jumpsLeft;

    [Header("Jump Buffer")]
    [SerializeField] private Transform bufferRange;
    private bool hasBufferedJump;

    [Header("WallJump")]
    [SerializeField] private Transform wallCheckerPos;
    [SerializeField] private float wallCheckerRadius;
    private bool isOnWall;
    [SerializeField] private float slideSpeed;
    private bool isWallSliding;
    [SerializeField] private Vector2 wallJumpForce;
    [SerializeField] private float exitWallTime;
    private float exitWallCounter;

    [Header("WallJumpBuffer")]
    [SerializeField] private float walljumpBufferTime;
    private float walljumpBufferCounter;


    
    //groundcheck stuff
    [Header("Ground Check")]
    [SerializeField] private Transform groundCheckPos;    
    [SerializeField] private float groundCheckRadius;
    private bool isOnGround;
    [SerializeField] private LayerMask platform;
    private bool wasOnGroundLastFrame;   

    [HideInInspector] public bool canMove;

    [Header("Animation")]
    [SerializeField] private Animator animator;

    private void Start()
    {
        canMove = true;
        hasBufferedJump = false;                
    }

    private void Update()
    {
        GettingInputs();      
        JumpFall();       
        JumpCheckings();
        Flip();
                      
        animator.SetFloat("Movement", Mathf.Abs((horizontalAxis)));
        if (wasOnGroundLastFrame && !isOnGround) //acabou de pular
        {
            animator.SetBool("isJumping",true);
        }
        else if(!wasOnGroundLastFrame && isOnGround)//acabou de cair
        {
            animator.SetBool("isJumping", false);
        }
       
    }

    void FixedUpdate()
    {        
        PositionChecking();       
        Walking();        
        WallSlide();
        WallJump();
    }

    #region functions

    void GettingInputs()
    {
        horizontalAxis = Input.GetAxisRaw("Horizontal");
        

        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0 && jumpsLeft >= 0) //cutting the jump
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }

        if (isWallSliding && Input.GetButtonDown("Jump") && !isOnGround)
        {
            walljumpBufferCounter = walljumpBufferTime;
            StartCoroutine(StoppingMovement(0.2f));
        }
        else 
        {
            walljumpBufferCounter -= Time.deltaTime;
        }
    }

    void JumpCheckings()
    {
        JumpBuffer();

        if (!wasOnGroundLastFrame && isOnGround)//acabou de cair
        {
            jumpsLeft = amountOfJumps;
        }

        if (Input.GetButtonDown("Jump") && jumpsLeft > 0)
        {
            Jump();
        }

        else if (isOnGround && hasBufferedJump)
        {            
            hasBufferedJump = false;
            Jump();
        }
    }

    void WallSlide()
    {
        if (isOnWall && rb.velocity.y <= 0 && !isOnGround)
        {
            rb.velocity = new Vector2(rb.velocity.x, -slideSpeed);
            isWallSliding = true;
            jumpsLeft = amountOfJumps;
        }

        else
        {
            isWallSliding = false;
        }

        if ((isWallSliding && horizontalAxis > 0 && !facingRight) || (isWallSliding && horizontalAxis < 0 && facingRight))
        {
            exitWallCounter += Time.deltaTime;
        }

        else 
        {
            exitWallCounter = 0;
        }
    }

    void WallJump()
    {       
        if (walljumpBufferCounter > 0)
        {
            walljumpBufferCounter = 0;
            rb.velocity = Vector2.zero;
            //Flip();
            if (facingRight)
            {
                 rb.AddForce(new Vector2(wallJumpForce.x * -1, wallJumpForce.y),ForceMode2D.Impulse);
               // rb.velocity = new Vector2(rb.velocity.x + wallJumpForce.x * -1,  rb.velocity.y + wallJumpForce.y);
            }
            else
            {
                 rb.AddForce(new Vector2(wallJumpForce.x * 1, wallJumpForce.y),ForceMode2D.Impulse);
               // rb.velocity = new Vector2(rb.velocity.x + wallJumpForce.x , rb.velocity.y + wallJumpForce.y);
            }
        }
    }

    void JumpBuffer()
    {
        bool iSInBufferRange = Physics2D.Linecast(transform.position, bufferRange.position, platform);
        if (Input.GetButtonDown("Jump") && iSInBufferRange && jumpsLeft < 1)
        {
            hasBufferedJump = true;
        }
    }

    void Walking() 
    {   
        if (canMove && (!isWallSliding || exitWallCounter >= exitWallTime))
        {
            rb.velocity = new Vector2(horizontalAxis * moveSpeed, rb.velocity.y);                     
        }

        if (rb.velocity.x > 0 && !facingRight)
        {
            facingRight = true;
        }

        else if (rb.velocity.x  < 0 && facingRight)
        {
            facingRight = false;
        }
        
    }

    void Flip()
    {
        if (facingRight)
        {
            transform.localScale = new Vector3(1,1,1);
        }

        else 
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }
    void Jump()
    {        
        rb.velocity = new Vector2(rb.velocity.x, 0);
        
        if (jumpsLeft > 1)
        {
           // rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y + jumpStrength);
            rb.AddForce(new Vector2(rb.velocity.x, jumpStrength),ForceMode2D.Impulse);
            print("pulaozao");
        }

        else 
        {
            // rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y + jumpStrength * jumpCutAmount);
            rb.AddForce(new Vector2(rb.velocity.x, jumpStrength * 0.8f), ForceMode2D.Impulse);
            print("segundo pulinho anaozinho");
        }

        jumpsLeft--;
    }

    void JumpFall()
    {
        if (rb.velocity.y < 0 && !isOnWall)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier  -1) * Time.deltaTime;
        }
    }

    void PositionChecking()
    {
        wasOnGroundLastFrame = isOnGround;
        isOnGround = Physics2D.OverlapCircle(groundCheckPos.position, groundCheckRadius, platform);       

        isOnWall = Physics2D.OverlapCircle(wallCheckerPos.position, wallCheckerRadius, platform);
        if (transform.localScale.x > 1)
        {
            facingRight = true;
        }
        else if (transform.localScale.x < 1)
        {
            facingRight = false;
        }
    }

     public IEnumerator StoppingMovement(float howMuchTime)
    {
        
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
        Gizmos.DrawWireSphere(groundCheckPos.position, groundCheckRadius);

        //debug do jump buffer
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, bufferRange.position);

        //debug do wallcheck
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(wallCheckerPos.position, wallCheckerRadius);


    }
    #endregion functions

}

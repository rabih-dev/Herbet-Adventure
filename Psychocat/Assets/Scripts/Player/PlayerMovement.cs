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
    [SerializeField] private Transform wallChecker;

    
    //groundcheck stuff
    [Header("Ground Check")]
    [SerializeField] private Transform groundCheckPos;    
    [SerializeField] private float groundCheckRadius;
    private bool isOnGround;
    [SerializeField] private LayerMask platform;

    [HideInInspector] public bool canMove;

    private void Start()
    {
        canMove = true;
        hasBufferedJump = false;
        amountOfJumps = amountOfJumps - 1; // corrigindo pois no meio do ar no primeiro pulo ele conta como se estivesse no chao e reseta a qtd de pulos;
        
    }

    private void Update()
    {
      
        GettingInputs();
        JumpFall();
        JumpCheckings();

    }

    void FixedUpdate()
    {       
        GroundChecking();
        Walking();
    }

    #region functions

    void GettingInputs()
    {
        
        horizontalAxis = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonUp("Jump")) //cutting the jump
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }
    }

    void JumpCheckings()
    {
        JumpBuffer();

        if (Input.GetButtonDown("Jump") && jumpsLeft > 0)
        {
            Jump();
            print("pulei normal");
        }

        else if (isOnGround && hasBufferedJump)
        {            
            hasBufferedJump = false;
            Jump();

            print("pulei no buffer");
        }
    }

    void JumpBuffer()
    {
        bool iSInBufferRange = Physics2D.Linecast(transform.position, bufferRange.position, platform);
        if (Input.GetButtonDown("Jump") && iSInBufferRange && jumpsLeft <= 0)
        {
            hasBufferedJump = true;
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
                jumpsLeft--;
                rb.velocity = (new Vector2(rb.velocity.x, jumpStrength));
    }

    void JumpFall()
    {
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier  -1) * Time.deltaTime;
        }
    }

    


    void GroundChecking()
    {       
        isOnGround = Physics2D.OverlapCircle(groundCheckPos.position, groundCheckRadius, platform);

        if (isOnGround)
        {
            jumpsLeft = amountOfJumps;
            
        }
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
        Gizmos.DrawWireSphere(groundCheckPos.position, groundCheckRadius);

        //debug do jump buffer
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, bufferRange.position);
    }
    #endregion functions

}

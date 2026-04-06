using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    //Movement
    public float moveSpeed = 5f;
    float horizontalMovement;

    //Jumping
    public float jumpPower = 10f;
    public int maxJumps = 2;
    private int jumpsRemaining;

    //Ground check
    public Transform groundCheckPos;
    public Vector2 groundCheckSize = new Vector2(0.5f, 0.05f);
    public LayerMask groundLayer;

    //Gravity
    public float baseGravity = 2f;
    public float maxFallSpeed = 18f;
    public float fallSpeedMultiplier = 2f;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        rb.linearVelocity = new Vector2(horizontalMovement * moveSpeed, rb.linearVelocity.y);
        GoundCheck();
        Gravity();
    }

    public void Gravity()
    {
        if(rb.linearVelocity.y < 0 )
        {
            rb.gravityScale = baseGravity * fallSpeedMultiplier; //Falls increasingly faster
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Max(rb.linearVelocity.y, -maxFallSpeed));//Maxes out fall speed
        }
        else
        {
            rb.gravityScale = baseGravity;
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        horizontalMovement = context.ReadValue<Vector2>().x;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if(jumpsRemaining > 0)
        {
            if(context.performed)
            {
                //Hold down = full height
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower);
                jumpsRemaining--;
            }
            else if (context.canceled)
            {
                //Tap = half height
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
                jumpsRemaining--;
            }
        }
        
    }

    private void GoundCheck()
    {
        if(Physics2D.OverlapBox(groundCheckPos.position, groundCheckSize, 0, groundLayer))
        {
            jumpsRemaining = maxJumps;
        }

    }

    private void OnDrawGizmoSelected()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireCube(groundCheckPos.position, groundCheckSize);
    }
}

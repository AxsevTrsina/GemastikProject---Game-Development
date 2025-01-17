using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
public class playerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Particle Layer")]
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sprite;

    [Header("Movement Layer")]
    public float speed = 10;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;
    [Range(1, 50)]
    public float jumpVelocity;

    [Header("Ground Layer")]
    public LayerMask groundLayer;
    public bool onGround = false;
    public float groundLength = 1f;
    public Vector3 colliderOffset;
    bool facingRight = true;






    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        onGround = Physics2D.Raycast(transform.position + colliderOffset, Vector2.down, groundLength, groundLayer) 
            || Physics2D.Raycast(transform.position - colliderOffset, Vector2.down, groundLength, groundLayer);
        float x = CrossPlatformInputManager.GetAxis("Horizontal");
        float y = CrossPlatformInputManager.GetAxis("Vertical");
        Vector2 dir = new Vector2(x, y);
        jump();
        betterJump();
        walk(dir);
        //Flip();

        if (x > 0f )
        {
            anim.SetBool("isWalking", true);
            sprite.flipX = true;

        }
        else if(x < 0f )
        {
            anim.SetBool("isWalking", true);
            sprite.flipX = false;
        }
        else{
            anim.SetBool("isWalking", false);
        }
    }

    private void walk(Vector2 dir)
    {
        rb.velocity = (new Vector2(dir.x * speed, rb.velocity.y));
        
    }
    private void jump()
    {
        if (CrossPlatformInputManager.GetButtonDown("Jump") && onGround)
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.up * jumpVelocity;
            anim.SetTrigger("takeOf");
        }
        if (onGround == false)
        {
            anim.SetBool("isJumping", true);
        }
        else if (onGround == true)
        {
            anim.SetBool("isJumping", false);
        }
    }
    private void betterJump()
    {
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) *Time.deltaTime;

        }
        else if (rb.velocity.y > 0 && !CrossPlatformInputManager.GetButton("Jump"))
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }
    //void Flip()
    //{
    //    Vector3 currentScale = gameObject.transform.localScale;
    //    currentScale.x *= -1;
    //    gameObject.transform.localScale = currentScale;

    //    facingRight = !facingRight;
    //}
    //private void Flip()
    //{
    //    // Rotate the player
    //    if (transform.localEulerAngles.y != 180 && !facingRight)
    //        transform.Rotate(0f, 180f, 0f);
    //    else if (transform.localEulerAngles.y != 0 && facingRight)
    //        transform.Rotate(0f, -180f, 0f);

    //    // player flip point of attck also flip is direction
    //    //transform.Rotate(0f, 180f, 0f);
    //}



    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position + colliderOffset, transform.position + colliderOffset + Vector3.down * groundLength);
        Gizmos.DrawLine(transform.position - colliderOffset, transform.position - colliderOffset + Vector3.down * groundLength);
    }

}

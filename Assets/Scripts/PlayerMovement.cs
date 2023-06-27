using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Movement : MonoBehaviour
{
    public float moveSpeed;
    public float jumpForce;
    public Transform ceilingCheck;
    public Transform floorCheck;
    public LayerMask groundObjects;
    public float checkRadius;

    private Rigidbody2D rigidBody;
    private Animator animator;
    private bool facingRight = true;
    private bool isJumping = false;
    private float moveDirection;
    private bool isGrounded;

    /// <summary>
    /// Called prior to start - build references
    /// </summary>
    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // get input
        GetInput();

        // Animate
        Animate();


    }

    private void FixedUpdate()
    {
        // Check if grounded
        isGrounded = Physics2D.OverlapCircle(floorCheck.position, checkRadius, groundObjects);
        // execute action
        Move();
    }

    private void Move()
    {
        rigidBody.velocity = new Vector2(moveDirection * moveSpeed, rigidBody.velocity.y);
        if (isJumping)
            rigidBody.AddForce(new Vector2(0, jumpForce));

        if(moveDirection == 0f)
        {
            animator.SetBool("moving", false);
        }
        else
        {
            // walking animation
            animator.SetBool("moving", true);

        }

        isJumping = false;
    }

    private void Animate()
    {
        if (moveDirection > 0 && !facingRight)
            changeDirection();
        else if (moveDirection < 0 && facingRight)
            changeDirection();
    }

    private void GetInput()
    {
        moveDirection = Input.GetAxis("Horizontal");
        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            // check to see if we are airborne..
            isJumping = true;
        }
        if(Input.GetButtonDown("Cancel"))
        {
            SceneManager.LoadScene(0);
        }
    }

    private void changeDirection()
    {
        facingRight = !facingRight;

        transform.Rotate(0, 180f, 0);
        
    }
}

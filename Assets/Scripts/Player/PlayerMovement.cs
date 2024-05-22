using System.Collections;
using System.Collections.Generic;
using System.Resources;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float walkSpeed;
    public float sprintSpeed;
    public float slideSpeed;
    public float wallRunSpeed;
    public float dashSpeed;
    public float swingSpeed;
    public float rocketJumpSpeed;
    private float desiredMoveSpeed;
    private float lastDesiredMoveSpeed;
    private float diferentSpeed;
    public float speedIncreaseMultipier;
    public float slopeIncreaseMultipier;
    public float groudDrag;
    public float maxYSpeed;

    [Header("Jumping")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;

    [Header("Crouching")]
    public float crouchSpeed;
    public float crouchYScale;
    private float startYscale;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatisGround;
    bool grounded;

    [Header("Slope Handling")]
    public float maxSloapAngle;
    private RaycastHit slopeHit;
    private bool exitingSlope;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode crouchKey = KeyCode.LeftControl;

    [Header("")]
    public Transform orientation;
    float horizInput;
    float vertInput;
    Vector3 moveDirection;
    Rigidbody rb;

    //the current state of the player
    public MovementState state;

    //all the movement states
    public enum MovementState
    {
        walking,
        sprinting,
        wallrunning,
        air,
        sliding,
        swinging,
        crouching,
        rocketjumping,
        dashing
    }

    public bool sliding;
    public bool wallRunning;
    public bool dashing;
    public bool swinging;
    public bool rocketJumping;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        readyToJump = true;

        //setting the starting y scale of the player for crouching
        startYscale = transform.localScale.y;
    }

    private void Update()
    {   
        MyInput();
        SpeedControl();
        StateHandler();

        //checking if there is ground
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatisGround);

        //handle the drag
        if (state == MovementState.walking || state == MovementState.sprinting || state == MovementState.crouching)
        {
            rb.drag = groudDrag;
        }
        else
        {
            rb.drag = 0;
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        horizInput = Input.GetAxisRaw("Horizontal");
        vertInput = Input.GetAxisRaw("Vertical");

        //Jumping
        if(Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;
            Jump();
            
            //makes so that you can jump without releasing the space key
            Invoke(nameof(ResetJump), jumpCooldown);
        }

        //Crouching
        if (Input.GetKeyDown(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
            //We do this because when shrinking the player he starts floating so we quickly add a force to push him to the ground
        }

        if (Input.GetKeyUp(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, startYscale, transform.localScale.z);
        }
    }

    private void StateHandler()
    {
        if (rocketJumping && !dashing)
        {
            state = MovementState.rocketjumping;
            desiredMoveSpeed = rocketJumpSpeed;
        }
        else if(dashing)
        {
            state = MovementState.dashing;
            desiredMoveSpeed = dashSpeed;
            diferentSpeed = dashSpeed;
        }
        else if(wallRunning)
        {
            state = MovementState.wallrunning;
            desiredMoveSpeed = wallRunSpeed;
            diferentSpeed = wallRunSpeed;
        }
        //slidiing
        else if (sliding)
        {
            state = MovementState.sliding;

            if(OnSlope() && rb.velocity.y < 0.1f)
            {
                desiredMoveSpeed = slideSpeed;
                diferentSpeed = slideSpeed;
            }
            else
            {
                desiredMoveSpeed = sprintSpeed;
                diferentSpeed = sprintSpeed;
            }
        }
        else if (swinging)
        {
            StopAllCoroutines();
            state = MovementState.swinging;
            desiredMoveSpeed = swingSpeed;
            diferentSpeed = swingSpeed;
        }
        else if (Input.GetKey(crouchKey))
        {
            state = MovementState.crouching;
            desiredMoveSpeed = crouchSpeed;
            diferentSpeed = crouchSpeed;
        }
        //Sprinting
        else if(grounded && Input.GetKey(sprintKey))
        {
            state = MovementState.sprinting;
            desiredMoveSpeed = sprintSpeed;
            diferentSpeed = sprintSpeed;
        }
        //Walking
        else if (grounded)
        {
            StopAllCoroutines();
            state = MovementState.walking;
            desiredMoveSpeed = walkSpeed;
            diferentSpeed = walkSpeed;
        }
        //Air
        else
        {
            state = MovementState.air;
        }

        if(Mathf.Abs(desiredMoveSpeed - lastDesiredMoveSpeed) > 6.5  && moveSpeed != 0 && state == MovementState.sliding)
        {
            StopAllCoroutines();
            StartCoroutine(SmoothlyLerpMoveSpeed());
        }
        else if (state == MovementState.rocketjumping)
        {
            StopAllCoroutines();
            StartCoroutine(SmoothlyLerpMoveSpeed());
        }
        else
        {
            moveSpeed = desiredMoveSpeed;
        }
        lastDesiredMoveSpeed = desiredMoveSpeed;
    }

    private IEnumerator SmoothlyLerpMoveSpeed()
    {
        float time = 0;
        float diference = Mathf.Abs(desiredMoveSpeed - moveSpeed);
        float startValue = moveSpeed;

        while (time < diference)
        {
            moveSpeed = Mathf.Lerp(startValue, desiredMoveSpeed, time/diference);
            if (OnSlope() || rocketJumping)
            {
                float slopeAngle = Vector3.Angle(Vector3.up, slopeHit.normal);
                float slopeAngleincrease = 1 + (slopeAngle / 90f);

                time += Time.deltaTime * speedIncreaseMultipier * slopeIncreaseMultipier * slopeAngleincrease;
            }
            else
            {
                time += Time.deltaTime * speedIncreaseMultipier;
            }
            yield return null;
        }
        
        moveSpeed = desiredMoveSpeed;
    }

    private void MovePlayer()
    {
        if (state == MovementState.dashing) return;
        if (state == MovementState.swinging) return;

        moveDirection = orientation.forward * vertInput + orientation.right * horizInput;

        //if on slope
        if (OnSlope() && !exitingSlope)
        {
            rb.AddForce(GetSlopeMoveDirection(moveDirection) * moveSpeed * 20f, ForceMode.Force);
            
            if (rb.velocity.y > 0)
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
        }

        //on ground
        else if(grounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        }
        else if (!grounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
        }

        //turn the gravity off while on slope so that you don't slide off
        if (!wallRunning)
        {
            rb.useGravity = !OnSlope();
        }
    }

    private void SpeedControl()
    {
        if(state != MovementState.sliding && state != MovementState.air && state != MovementState.rocketjumping && state != MovementState.dashing && rb.velocity.magnitude <= 7)
        {
            StopAllCoroutines();
        }


        // limiting speed on slope
        if (OnSlope() && !exitingSlope)
        {
            if (rb.velocity.magnitude > moveSpeed)
                rb.velocity = rb.velocity.normalized * moveSpeed;
        }

        // limiting speed on ground or in air
        else
        {
            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            // limit velocity if needed
            if (flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            }
        }
    }

    private void Jump()
    {
        exitingSlope = true;
        /*reset the y velocity so that 
        you always jump the same height*/
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;
        exitingSlope = false;
    }

    public bool OnSlope()
    {
        if(Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSloapAngle && angle != 0;
        }

        return false;
    }

    public Vector3 GetSlopeMoveDirection(Vector3 direction)
    {
        return Vector3.ProjectOnPlane(direction, slopeHit.normal).normalized;
    }

    public void StopAll()
    {
        StopAllCoroutines();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sliding : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;
    public Transform playerObj;
    private Rigidbody rb;
    private PlayerMovement pm;

    [Header("Sliding")]
    public float maxSlideTime;
    public float slideForce;
    private float slideTimer;

    public float slideYscale;
    private float startYscale;

    [Header("Input")]
    public KeyCode slidekey = KeyCode.LeftControl;
    private float horizontalInput;
    private float verticalInput;
    public bool wasOnSlope;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerMovement>();

        slideTimer = maxSlideTime;
        startYscale = playerObj.localScale.y;
    }

    private void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if(Input.GetKeyDown(slidekey) && (horizontalInput != 0 || verticalInput != 0))
        {
            StartSlide();
        }

        if(Input.GetKeyUp(slidekey) && pm.sliding)
        {
            StopSlide();
        }
    }

    private void FixedUpdate()
    {
        if(pm.sliding)
        {
            SlidingMovement();
        }
    }

    private void StartSlide()
    {
        pm.sliding = true;

        gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x, slideYscale, gameObject.transform.localScale.z);
        rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
        //We do this because when shrinking the player he starts floating so we quickly add a force to push him to the ground
    }

    private void SlidingMovement()
    {
        Vector3 inputDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // sliding normal
        if (!pm.OnSlope() || rb.velocity.y > -0.1f)
        {
            rb.AddForce(inputDirection.normalized * slideForce, ForceMode.Force);

            slideTimer -= Time.deltaTime;
        }

        // sliding down a slope
        else
        {
            rb.AddForce(pm.GetSlopeMoveDirection(inputDirection) * slideForce, ForceMode.Force);
            wasOnSlope = true;
        }

        if(rb.velocity.magnitude <= 10.1)
        {
            wasOnSlope = false;
        }

        if (slideTimer <= 0 && wasOnSlope == false)
        {
            StopSlide();
        }
    }

    private void StopSlide()
    {
        pm.sliding = false;
        gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x, startYscale, gameObject.transform.localScale.z);
        slideTimer = maxSlideTime;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dashing : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;
    public Transform playerCam;
    private Rigidbody rb;
    private PlayerMovement pm;
    public Animator dashAnimator;
    public ParticleSystem particle;

    [Header("Dashing")]
    public float dashForce;
    public float dashUpwardForce;
    public float dashDuration;

    [Header("Cooldown")]
    public float dashCd;
    public float dashCdTimer;

    [Header("Input")]
    public KeyCode dashKey = KeyCode.E;

    [Header("Settings")]
    public bool useCameraFoward = true;
    public bool allowAllDirections = true;
    public bool disableGravity = false;
    public bool resetVel = true;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(dashKey) && dashCdTimer <= 0)
        {
            Debug.Log("yes");
            pm.StopAll();
            Dash();
        }

        if(dashCdTimer > 0) 
        {
            dashCdTimer -= Time.deltaTime;
        }
    }

    private void Dash()
    {
        if (dashCdTimer > 0) 
        {
            return;
        }
        
        else dashCdTimer = dashCd;
        pm.dashing = true;
        dashAnimator.SetBool("dashed", true);

        Transform fowardT;

        if(useCameraFoward)
        {
            fowardT = playerCam;
        }
        else
        {
            fowardT = orientation;
        }

        Vector3 direction = GetDirection(fowardT);

        Vector3 forceToApply = direction * dashForce + orientation.up * dashUpwardForce;

        if(disableGravity)
        {
            rb.useGravity = false;
        }

        delayedForceToApply = forceToApply;

        Invoke(nameof(DelayedDashForce), 0.025f);

        Invoke(nameof(ResetDash), dashDuration);
    }

    private Vector3 delayedForceToApply;
    private void DelayedDashForce()
    {
        if(resetVel)
            rb.velocity = Vector3.zero;
        dashAnimator.SetBool("dashed", true);

        rb.AddForce(delayedForceToApply, ForceMode.Impulse);
    }

    private void ResetDash()
    {
        dashAnimator.SetBool("dashed", false);
        pm.dashing = false;
        if (disableGravity)
        {
            rb.useGravity = true;
        }
    }

    private Vector3 GetDirection(Transform fowardT)
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3();

        if(allowAllDirections)
        {
            direction = fowardT.forward * verticalInput + fowardT.right * horizontalInput;
        }
        else
        {
            direction = fowardT.forward;
        }

        if (verticalInput == 0 && horizontalInput == 0)
        {
            direction = fowardT.forward;
        }

        return direction.normalized;
    }
}

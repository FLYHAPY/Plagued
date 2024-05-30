using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Swing : MonoBehaviour
{
    [Header("References")]
    public LineRenderer lr;
    public Transform gunTip, cam, player;
    public LayerMask whatIsGrappleable;
    private PlayerMovement pm;
    private bool started;

    [Header("Swinging")]
    private float maxSwingDistance = 25f;
    private Vector3 swingPoint;
    private SpringJoint joint;
    private bool played;

    [Header("Input")]
    public KeyCode swingKey = KeyCode.Mouse0;

    [Header("References")]
    public Transform orientation;
    public Rigidbody rb;
    public float horizontalThrustForce;
    public float fowardThrustForce;
    public float extendCableSpeed;
    public AudioSource grappleSound;
    public AudioSource grappleHitSound;
    public AudioSource grappleStopSound;

    [Header("Prediction")]
    public RaycastHit predictionHit;
    public float predictionSphereCastRadius;
    public Transform predictionPoint;


    // Start is called before the first frame update
    void Start()
    {
        pm = GetComponent<PlayerMovement>();
    }

    private void LateUpdate()
    {
        DrawRope();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(swingKey)) 
        {
            StartSwing();
        }
        if (Input.GetKeyUp(swingKey))
        {
            StopSwing();
            if (started)
            {
                started = false;
                grappleStopSound.Play();
            }
        }

        CeckForSwingPoints();

        if (joint != null)
        {
            ODMGearMovement();
        }
    }

    private Vector3 currentGrapplePosition;

    private void StartSwing()
    {
        if (predictionHit.point == Vector3.zero) return;

        started = true;
        pm.swinging = true;
        swingPoint = predictionHit.point;
        joint = player.AddComponent<SpringJoint>();
        joint.autoConfigureConnectedAnchor = false;
        joint.connectedAnchor = swingPoint;

        float distanceFromPoint = Vector3.Distance(player.position, swingPoint);

        joint.maxDistance = distanceFromPoint * 0.8f;
        joint.minDistance = distanceFromPoint * 0.25f;

        joint.spring = 4.5f;
        joint.damper = 7f;
        joint.massScale = 4.5f;

        lr.positionCount = 2;
        currentGrapplePosition = gunTip.position;
        grappleSound.Play();

    }

    private void StopSwing()
    {
        played = false;
        pm.swinging = false;
        lr.positionCount = 0;
        Destroy(joint);
    }

    private void DrawRope()
    {
        if (joint == null) return;

        currentGrapplePosition = Vector3.Lerp(currentGrapplePosition, swingPoint, Time.deltaTime * 8f);

        lr.SetPosition(0, gunTip.position);
        lr.SetPosition(1, currentGrapplePosition);
        
        if(Vector3.Distance(currentGrapplePosition, swingPoint) < 0.1f && !played)
        {
            grappleSound.Stop();
            grappleHitSound.Play();
            played = true;
        }

    }

    private void ODMGearMovement()
    {
        if(Input.GetKey(KeyCode.D))
        {
            rb.AddForce(orientation.right * horizontalThrustForce * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.A))
        {
            rb.AddForce(-orientation.right * horizontalThrustForce * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.W))
        {
            rb.AddForce(orientation.forward * fowardThrustForce * Time.deltaTime);
        }

        if(Input.GetKey(KeyCode.Space)) 
        {
            Vector3 directionToPoint = swingPoint - transform.position;
            rb.AddForce(directionToPoint.normalized * fowardThrustForce * Time.deltaTime);

            float distanceFromPoint = Vector3.Distance(transform.position, swingPoint);

            joint.maxDistance = distanceFromPoint * 0.8f;
            joint.minDistance = distanceFromPoint * 0.25f;
        }

        if(Input.GetKey(KeyCode.S))
        {
            float extendDistanceFromPoint = Vector3.Distance(transform.position, swingPoint) + extendCableSpeed;

            joint.maxDistance = extendDistanceFromPoint * 0.8f;
            joint.minDistance = extendDistanceFromPoint * 0.25f;
        }
    }

    private void CeckForSwingPoints()
    {
        if(joint != null) return;

        RaycastHit sphereCastHit;
        Physics.SphereCast(cam.position, predictionSphereCastRadius, cam.forward, out sphereCastHit, maxSwingDistance, whatIsGrappleable);

        RaycastHit raycastHit;
        Physics.Raycast(cam.position, cam.forward, out raycastHit, maxSwingDistance, whatIsGrappleable);

        Vector3 realHitPoint;

        //finding the point
        if(raycastHit.point != Vector3.zero)
        {
            realHitPoint = raycastHit.point;
        }else if(sphereCastHit.point != Vector3.zero)
        {
            realHitPoint = sphereCastHit.point;
        }
        else
        {
            realHitPoint = Vector3.zero;
        }

        //If we found it
        if(realHitPoint != Vector3.zero)
        {
            predictionPoint.gameObject.SetActive(true);
            predictionPoint.position = realHitPoint;
        }
        else
        {
            predictionPoint.gameObject.SetActive(false);
        }

        predictionHit = raycastHit.point == Vector3.zero ? sphereCastHit : raycastHit;
    }
}

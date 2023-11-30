using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnController : MonoBehaviour
{
    [SerializeField] float safeSpeed = 5f;
    [SerializeField] float turnSwitchDelay = 1.5f;
    [SerializeField] float turnForce = 5f;
    [SerializeField] float minTurnAllowedDistance = 4f;
    [SerializeField] float minSpeed = 2f;
    [SerializeField] float minSpeedPreventionScalar = 2f;
    [SerializeField] float ninjaSlowDownScalar = 2f;


    Rigidbody rb;
    bool beenTurning = false;
    bool turnLeft = true;
    EdgeAvoidance edgeAvoidance;
    Vector3 velocityVectorBeforeTurn;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        edgeAvoidance = GetComponent<EdgeAvoidance>();
        velocityVectorBeforeTurn = Vector3.forward;
    }

    private void Update()
    {
        ControlTurning();
    }


    void ControlTurning()
    {
        if(rb.velocity.magnitude > safeSpeed)
        {


            beenTurning = true;
            // Turn 
            if (turnLeft && edgeAvoidance.prevDistRight < minTurnAllowedDistance)
                turnLeft = false;
            else if (!turnLeft && edgeAvoidance.prevDistLeft < minTurnAllowedDistance)
                turnLeft = true;

            /*
            // Preventing turning too much and starting going up
            if (Mathf.Abs(Vector3.Angle(rb.velocity.normalized, velocityVectorBeforeTurn)) > 80f)
            { 
                // We're turning too much!
                turnLeft = !turnLeft;
            }
            */

            if (turnLeft) 
            { 
                // Turn left
                Vector3 leftTurnDir = Vector3.Cross(rb.velocity, Vector3.up).normalized;
                Debug.DrawRay(transform.position, leftTurnDir, Color.red);
                Turn(leftTurnDir);
            }
            else
            {
                // Turn right
                Vector3 rightTurnDir = Vector3.Cross(rb.velocity, -Vector3.up).normalized;
                Debug.DrawRay(transform.position, rightTurnDir, Color.red);
                Turn(rightTurnDir);
            }
        }
        else
        {
            velocityVectorBeforeTurn = rb.velocity.normalized;
            if(beenTurning)
            {
                // if(!(edgeAvoidance.prevDistLeft < minTurnAllowedDistance) && !(edgeAvoidance.prevDistRight < minTurnAllowedDistance))
                turnLeft = !turnLeft;
                beenTurning = false;
            }
        }

        // What if we're too slow
        if (rb.velocity.magnitude < minSpeed)
        {
            rb.AddForce(rb.velocity.normalized * minSpeedPreventionScalar);
            Debug.DrawRay(transform.position, rb.velocity.normalized * minSpeedPreventionScalar, Color.yellow);
        }

    }

    void Turn(Vector3 dir)
    {
        rb.AddForce(dir * turnForce);
        rb.AddForce(-rb.velocity.normalized * ninjaSlowDownScalar);
    }
}

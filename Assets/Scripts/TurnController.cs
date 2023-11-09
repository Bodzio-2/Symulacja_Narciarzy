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

    Rigidbody rb;
    bool beenTurning = false;
    bool turnLeft = true;
    EdgeAvoidance edgeAvoidance;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        edgeAvoidance = GetComponent<EdgeAvoidance>();
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
            if(beenTurning)
            {
                // if(!(edgeAvoidance.prevDistLeft < minTurnAllowedDistance) && !(edgeAvoidance.prevDistRight < minTurnAllowedDistance))
                turnLeft = !turnLeft;
                beenTurning = false;
            }
        }

        if (rb.velocity.magnitude < minSpeed)
        {
            rb.AddForce(rb.velocity.normalized * minSpeedPreventionScalar);
        }

    }

    void Turn(Vector3 dir)
    {
        rb.AddForce(dir * turnForce);
    }
}

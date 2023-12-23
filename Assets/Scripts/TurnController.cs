using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnController : MonoBehaviour
{
    public float minTurnSpeed = 5f;
    public float turnSwitchDelay = 1.5f;
    public float turnForce = 5f;
    public float currentSpeed;

    public float leftDist;
    public float rightDist;
    public float minTurnAllowedDistance = 4f;
    public float minSpeed = 2f;
    public float minSpeedPreventionScalar = 2f;
    public float ninjaSlowDownScalar = 2f;
    public float turnStartTime;
    public float turnTimeoutTime;

    Rigidbody rb;
    public bool isTurning = false;
    public bool nextTurnLeft = true;
    EdgeAvoidance edgeAvoidance;
    Vector3 velocityVectorBeforeTurn;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        edgeAvoidance = GetComponent<EdgeAvoidance>();
        velocityVectorBeforeTurn = Vector3.forward;
    }

    private void FixedUpdate()
    {
        ControlTurning();
    }


    void ControlTurning()
    {
        leftDist = edgeAvoidance.prevDistLeft;
        rightDist=edgeAvoidance.prevDistRight;
        currentSpeed = rb.velocity.magnitude; 
        if(rb.velocity.magnitude > minTurnSpeed && turnTimeoutTime > turnSwitchDelay)
        {
            if(isTurning)
            {
                turnStartTime+=Time.deltaTime;
            }
            else{
                turnStartTime = 0.0f;
            }
            
            isTurning = true;
            // Turn 
            if (nextTurnLeft && edgeAvoidance.prevDistRight < minTurnAllowedDistance)
            {
                nextTurnLeft = false;
            }
            else if (!nextTurnLeft && edgeAvoidance.prevDistLeft < minTurnAllowedDistance)
            {
                nextTurnLeft = true;
            }

            
            /*
            // Preventing turning too much and starting going up
            if (Mathf.Abs(Vector3.Angle(rb.velocity.normalized, velocityVectorBeforeTurn)) > 80f)
            { 
                // We're turning too much!
                turnLeft = !turnLeft;
            }
            */

            if (nextTurnLeft) 
            {
                // Turn left
                Vector3 leftTurnDir = Vector3.Cross(rb.velocity, Vector3.up).normalized;
                // Debug.DrawRay(transform.position, leftTurnDir, Color.red);
                Turn(leftTurnDir);
            }
            else
            {
                // Turn right
                Vector3 rightTurnDir = Vector3.Cross(rb.velocity, -Vector3.up).normalized;
                // Debug.DrawRay(transform.position, rightTurnDir, Color.red);
                Turn(rightTurnDir);
                
            }
        }
        else
        {   
            
            velocityVectorBeforeTurn = rb.velocity.normalized;
            if(isTurning)
            {
                // Debug.Log("Switch things up!");
                // if(!(edgeAvoidance.prevDistLeft < minTurnAllowedDistance) && !(edgeAvoidance.prevDistRight < minTurnAllowedDistance))
                nextTurnLeft = !nextTurnLeft;
                isTurning = false;
                turnTimeoutTime = 0.0f;

            }
            turnTimeoutTime += Time.deltaTime;
        }

        // What if we're too slow
        if (rb.velocity.magnitude < minSpeed)
        {
            rb.AddForce(rb.velocity.normalized * minSpeedPreventionScalar * 10);
            // Debug.DrawRay(transform.position, rb.velocity.normalized * minSpeedPreventionScalar, Color.yellow);
        }

    }

    void Turn(Vector3 dir)
    {
        rb.AddForce(dir * turnForce * RandomGaussian(0.8f,1.2f), ForceMode.Impulse);
        Debug.DrawRay(transform.position, dir * turnForce * RandomGaussian(0.8f, 1.2f), Color.red);
        if(turnStartTime>0.1)
        {
            rb.AddForce(-rb.velocity.normalized * ninjaSlowDownScalar);
            Debug.DrawRay(transform.position, -rb.velocity.normalized * ninjaSlowDownScalar, Color.blue);
        }
        
    }

        public static float RandomGaussian(float minValue = 0.0f, float maxValue = 1.0f)
{
    float u, v, S;

    do
    {
        u = 2.0f * UnityEngine.Random.value - 1.0f;
        v = 2.0f * UnityEngine.Random.value - 1.0f;
        S = u * u + v * v;
    }
    while (S >= 1.0f);

    // Standard Normal Distribution
    float std = u * Mathf.Sqrt(-2.0f * Mathf.Log(S) / S);

    // Normal Distribution centered between the min and max value
    // and clamped following the "three-sigma rule"
    float mean = (minValue + maxValue) / 2.0f;
    float sigma = (maxValue - mean) / 3.0f;
    return Mathf.Clamp(std * sigma + mean, minValue, maxValue);
}

}


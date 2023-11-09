using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgeAvoidance : MonoBehaviour
{
    [HideInInspector]public Collider leftEdge;
    [HideInInspector]public Collider rightEdge;
    
    [SerializeField] float willToNotCrashAndDie = 10f;
    [SerializeField] float distanceInfluenceScalar = 5f;

    private Rigidbody rb;


    public float prevDistLeft = 5f;
    public float prevDistRight = 5f;


    float helpTimer = 0.0f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (leftEdge && rightEdge)
        {
            CruiseControl();
        }
    }

    float CalculateDistanceToEdge(Collider edge)
    {
        Vector3 closestPoint = edge.ClosestPoint(transform.position);
        float dist = Vector3.Distance(closestPoint, transform.position);
        Debug.DrawLine(transform.position, closestPoint, Color.green);
        return dist;
    }

    // Calculates the speed at which the skier is approaching the edge
    // Returns the speed, and the distance to the edge (used for updating prevDistance)
    (float speed, float new_distance) ApproachSpeed(Collider edge, float prevDist)
    {
        float new_dist = CalculateDistanceToEdge(edge);
        float approach_speed = (prevDist - new_dist) / Time.deltaTime;
        return (approach_speed, new_dist);
    }

    // Main function that makes sure the skier doesn't f***ing crash into the edge of our slope
    void CruiseControl()
    {
        float left_approach_speed;
        float right_approach_speed;
        (left_approach_speed, prevDistLeft) = ApproachSpeed(leftEdge, prevDistLeft);
        (right_approach_speed, prevDistRight) = ApproachSpeed(rightEdge, prevDistRight);

        if(left_approach_speed > 0)
        {
            // We're closing in on the left edge
            /** (1/(CalculateDistanceToEdge(rightEdge) * distanceInfluenceScalar)) */
            rb.AddForce(CalculateAwayVector(leftEdge) * willToNotCrashAndDie * left_approach_speed);
            // Debug.Log("Force: " + (willToNotCrashAndDie * left_approach_speed / (prevDistLeft * prevDistLeft * distanceInfluenceScalar)));
        }

        if(right_approach_speed > 0)
        {
            // We're closing in on the right edge
            /** (1/(CalculateDistanceToEdge(leftEdge) * distanceInfluenceScalar))*/
            rb.AddForce(CalculateAwayVector(rightEdge) * willToNotCrashAndDie * right_approach_speed);
            // Debug.Log("Force: " + (willToNotCrashAndDie * right_approach_speed / (prevDistRight * prevDistRight * distanceInfluenceScalar)));
        }


        // For logging purposes only
        if (Time.time > helpTimer)
        {
            Debug.Log("---------------------------------------------------------");
            Debug.Log("Approaching left edge with speed: " + left_approach_speed);
            Debug.Log("Approaching right edge with speed: " + right_approach_speed);
            helpTimer += 1f;
        }
    }

    // Calculates direction vector AWAY from the edge
    Vector3 CalculateAwayVector(Collider edge)
    {
        Vector3 closestPoint = edge.ClosestPoint(transform.position);
        // Vector3 AB = B - A (Destination - Origin) - in this case destination is skier (so that we have vector away from edge)
        Vector3 awayDirection = (transform.position - closestPoint).normalized;

        return awayDirection;
    }



}

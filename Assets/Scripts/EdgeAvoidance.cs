using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgeAvoidance : MonoBehaviour
{
    private Collider leftEdge;
    private Collider rightEdge;

    private Slope _slope;
    private TurnController _turnController;

    
    [SerializeField] float willToNotCrashAndDie = 10f;
    [SerializeField] float distanceInfluenceScalar = 5f;

    private Rigidbody rb;


    [HideInInspector]public float prevDistLeft = 5f;
    [HideInInspector]public float prevDistRight = 5f;


    float helpTimer = 0.0f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        _slope = FindObjectOfType<Slope>();
        _turnController = GetComponent<TurnController>();
    }

   

    private void FixedUpdate()
    {
        FindNearestEdges();
        if (leftEdge && rightEdge)
        {
            CruiseControl();
        }
    }

    private void FindNearestEdges()
    {
        if (!leftEdge)
            leftEdge = _slope.GetLeftEdges()[0];
        foreach(Collider edge in _slope.GetLeftEdges())
        {
            if(CalculateDistanceToEdge(edge) < CalculateDistanceToEdge(leftEdge))
            {
                leftEdge = edge;
                Vector3 point = leftEdge.ClosestPoint(transform.position);
                // Debug.DrawLine(transform.position, point, Color.green);
            }
        }

        if (!rightEdge)
            rightEdge = _slope.GetRightEdges()[0];
        foreach (Collider edge in _slope.GetRightEdges())
        {
            if (CalculateDistanceToEdge(edge) < CalculateDistanceToEdge(rightEdge))
            {
                rightEdge = edge;
                Vector3 point = rightEdge.ClosestPoint(transform.position);
                // Debug.DrawLine(transform.position, point, Color.green);
            }
        }
    }

    float CalculateDistanceToEdge(Collider edge)
    {
        Vector3 closestPoint = edge.ClosestPoint(transform.position);
        float dist = Vector3.Distance(closestPoint, transform.position);
        // Debug.DrawLine(transform.position, closestPoint, Color.green);
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

    Vector3 CalculateLeftEdgeForce(float scalar, float scalar1, Vector3 edgePoint)
    {
        Vector3 velocity = rb.velocity;
        //Debug.DrawRay(transform.position, velocity, Color.black);
        //Debug.DrawLine(transform.position, edgePoint, Color.blue);
        //Debug.DrawRay(transform.position, scalar1 * transform.up / Vector3.Distance(transform.position, edgePoint), Color.green);
        //Debug.DrawRay(transform.position, scalar * Vector3.Cross((new Vector3(velocity.x, 0f, velocity.z)), scalar1 * transform.up / Vector3.Distance(transform.position, edgePoint)), Color.cyan);
        return scalar * Vector3.Cross(( new Vector3(velocity.x, 0f, velocity.z)), scalar1 * transform.up / Vector3.Distance(transform.position, edgePoint)); 
    }

    Vector3 CalculateRightEdgeForce(float scalar, float scalar1, Vector3 edgePoint)
    {
        Vector3 velocity = rb.velocity;
        //Debug.DrawRay(transform.position, velocity, Color.black);
        //Debug.DrawLine(transform.position, edgePoint, Color.blue);
        //Debug.DrawRay(transform.position, scalar1 * -transform.up / Vector3.Distance(transform.position, edgePoint), Color.green);
        //Debug.DrawRay(transform.position, scalar * Vector3.Cross((new Vector3(velocity.x, 0f, velocity.z)), scalar1 * -transform.up / Vector3.Distance(transform.position, edgePoint)), Color.cyan);
        return scalar * Vector3.Cross((new Vector3(velocity.x, 0f, velocity.z)), scalar1 * -transform.up / Vector3.Distance(transform.position, edgePoint));
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
            //rb.AddForce(CalculateAwayVector(leftEdge) * willToNotCrashAndDie * left_approach_speed / prevDistLeft, ForceMode.Impulse);
            //Debug.DrawRay(transform.position, CalculateAwayVector(leftEdge) * willToNotCrashAndDie * left_approach_speed / prevDistLeft, Color.cyan);

            rb.AddForce(CalculateLeftEdgeForce(willToNotCrashAndDie, willToNotCrashAndDie, leftEdge.ClosestPoint(transform.position)), ForceMode.Impulse);
            //Debug.DrawRay(transform.position, CalculateEdgeForce(0.5f, transform.position - leftEdge.ClosestPoint(transform.position)), Color.cyan);
            // Debug.Log("Force: " + (willToNotCrashAndDie * left_approach_speed / (prevDistLeft * prevDistLeft * distanceInfluenceScalar)));
        }

        if (right_approach_speed > 0)
        {
            // We're closing in on the right edge
            /** (1/(CalculateDistanceToEdge(leftEdge) * distanceInfluenceScalar))*/
            //rb.AddForce(CalculateAwayVector(rightEdge) * willToNotCrashAndDie * right_approach_speed / prevDistRight, ForceMode.Impulse);
            //Debug.DrawRay(transform.position, CalculateAwayVector(rightEdge) * willToNotCrashAndDie * right_approach_speed / prevDistRight, Color.cyan);

            rb.AddForce(CalculateRightEdgeForce(willToNotCrashAndDie, willToNotCrashAndDie, rightEdge.ClosestPoint(transform.position)), ForceMode.Impulse);
            //Debug.DrawRay(transform.position, CalculateEdgeForce(0.5f, transform.position - rightEdge.ClosestPoint(transform.position)), Color.cyan);
            // Debug.Log("Force: " + (willToNotCrashAndDie * right_approach_speed / (prevDistRight * prevDistRight * distanceInfluenceScalar)));
        }

        /*
        // For logging purposes only
        if (Time.time > helpTimer)
        {
            Debug.Log("---------------------------------------------------------");
            Debug.Log("Approaching left edge with speed: " + left_approach_speed);
            Debug.Log("Approaching right edge with speed: " + right_approach_speed);
            helpTimer += 1f;
        }*/
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

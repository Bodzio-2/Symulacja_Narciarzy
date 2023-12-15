using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SocialScript : MonoBehaviour
{

    Rigidbody rb;
    public float angle;
    public float distScalar;
    public float speedTimesViewDistance=2f;


    public GameObject[] skiers;
    // Start is called before the first frame update
    void Start()
    {
        skiers = GameObject.FindGameObjectsWithTag("Skier");
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var velocityVector = rb.velocity;
        foreach(GameObject skier in skiers){
            if (skier)
            {
                if (skier.transform != transform)
                {
                    Vector3 target = skier.transform.position - transform.position;
                    angle = Vector3.Angle(target, rb.velocity.normalized);

                    if (angle < 90.0f && target.magnitude < velocityVector.magnitude * speedTimesViewDistance)
                    {
                        // Debug.DrawRay(transform.position, target);
                        distScalar = (target.magnitude) / (velocityVector.magnitude * speedTimesViewDistance) - 1;

                        // distScalar = Mathf.Pow(distScalar,3);                        


                        if (distScalar < -0.1)
                        {
                            rb.AddForce(target * distScalar * 50);
                            Debug.DrawRay(transform.position, target * distScalar, Color.red);
                        }
                    }
                }
            }
        }
        // Debug.DrawLine(transform.position, transform.position + velocityVector, Color.blue);
    }
}

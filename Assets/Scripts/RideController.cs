using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RideController : MonoBehaviour
{

    Rigidbody rb;
    public float slalomForce = 10;
    public float turnTimer = 2;

    float prevForce = 1f;
    public float forceScaler = 1f;

    float nextTime = 0;
    Vector3 forceDirection = Vector3.left;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // nextTime = (turnTimer / 2) + Time.time;
    }

    private void Update()
    {
        if(Time.time >= nextTime)
        {
            // Change direction of force applied to skier
            forceDirection = -forceDirection;
            nextTime = Time.time + turnTimer;
        }
        rb.AddForce(forceDirection * slalomForce);
        if (rb.velocity.z != 0)
        {
            slalomForce += (rb.velocity.z / prevForce) * forceScaler;
            prevForce = rb.velocity.z;
        }
    }
}

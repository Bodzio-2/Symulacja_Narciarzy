using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RideController2 : MonoBehaviour
{

    Rigidbody rb;
    public int value = 20;
    public float rotation;
    public double randomStart;

    public Quaternion rotationDegrees;

    public Vector3 angularVelocity;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        randomStart = UnityEngine.Random.Range(0, 3.0f);

    }

    private void FixedUpdate()
    {

        int val = Convert.ToInt32(Math.Cos(Time.timeAsDouble) * 10);
        int val2 = Convert.ToInt32(Math.Cos(Time.timeAsDouble + randomStart) * 10);

        angularVelocity = rb.angularVelocity;

        rotation = (float)Math.Atan(val2) / 20;

        if (rotation > 0)
        {
            rotation -= 0.01f;
        }
        else
        {
            rotation += 0.01f;
        }

        rotationDegrees = rb.rotation;
        rb.AddRelativeForce(Vector3.forward * value);
        // rb.AddRelativeForce(Vector3.back * 1);
        transform.Rotate(0, rotation * 10, 0);
    }
}

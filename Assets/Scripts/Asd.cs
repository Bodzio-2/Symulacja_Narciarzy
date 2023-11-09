using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.AI;

public class Asd : MonoBehaviour
{
    Rigidbody rb;

    public float viewRange = 50;
    public bool crossedFront;

    public float rotationDegrees;
    public bool maxPositive;
    public int maxTurnDegrees = 45;
    public int rotationSpeed = 30;
    public int speedMultiplier = 20;
    public float lastAvoidTime = 0;
    public float turnTimeout = 3;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        maxPositive = true;
    }

    // Update is called once per frame
    void Update()
    {
        lastAvoidTime += Time.deltaTime;
        Turn();
        for (int i = -2; i <= 2; i++)
        {
            Vector3 rotatedVector = Quaternion.AngleAxis(10f * i, Vector3.up) * transform.forward;
            Ray ray = new Ray(transform.position, rotatedVector);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo, viewRange))
            {
                Debug.DrawLine(ray.origin, hitInfo.point, Color.red);
                if (lastAvoidTime >= turnTimeout)
                {
                    if (i == 2 && maxPositive)
                    {
                        maxPositive = !maxPositive;
                        lastAvoidTime = 0f;
                    }
                    if (i == -2 && !maxPositive)
                    {
                        maxPositive = !maxPositive;
                        lastAvoidTime = 0f;
                    }
                }
            }
            else
            {
                Debug.DrawLine(ray.origin, ray.origin + ray.direction * viewRange, Color.green);
            }
        }

    }
    void Turn()
    {
        rotationDegrees = transform.rotation.eulerAngles.y;


        if ((maxPositive && rotationDegrees > maxTurnDegrees && rotationDegrees < 360 - maxTurnDegrees - 10) || (!maxPositive && rotationDegrees < 360 - maxTurnDegrees && rotationDegrees > maxTurnDegrees + 10))
        {
            maxPositive = !maxPositive;
            crossedFront = false;
        }

        Vector3 rot = new Vector3(0, 1, 0) * Time.deltaTime * rotationSpeed;
        if (maxPositive)
        {
            transform.Rotate(rot);

        }
        else
        {
            transform.Rotate(rot * -1);
        }

        rb.AddRelativeForce(Vector3.forward * speedMultiplier * Time.deltaTime * 1000);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlopeSector : MonoBehaviour
{
    public Collider leftEdge;
    public Collider rightEdge;

    /*
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out EdgeAvoidance avoid))
        {
            avoid.leftEdge = leftEdge;
            avoid.rightEdge = rightEdge;
        }
    }*/
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlopeSector : MonoBehaviour
{
    [SerializeField] private Collider leftEdge;
    [SerializeField] private Collider rightEdge;

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out EdgeAvoidance avoid))
        {
            avoid.leftEdge = leftEdge;
            avoid.rightEdge = rightEdge;
        }
    }
}

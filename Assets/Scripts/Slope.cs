using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slope : MonoBehaviour
{
    private List<Collider> leftEdges;
    private List<Collider> rightEdges;
    private void Start()
    {
        leftEdges = new List<Collider>();
        rightEdges = new List<Collider>();
        foreach(SlopeSector sector in GetComponentsInChildren<SlopeSector>()){
            Debug.Log(sector);
            //SlopeSector sector = _sector.GetComponent<SlopeSector>();
            leftEdges.Add(sector.leftEdge);
            rightEdges.Add(sector.rightEdge);
        }
    }

    public List<Collider> GetLeftEdges()
    {
        return leftEdges;
    }

    public List<Collider> GetRightEdges()
    {
        return rightEdges;
    }
}

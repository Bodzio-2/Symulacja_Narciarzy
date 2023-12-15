using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Despawner : MonoBehaviour
{

    List<Rigidbody> skiers = new List<Rigidbody>();
    public float despawnCooldown = 3.0f;
    private float despawnTimer = 0.0f;


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Skier")
        {
            skiers.Add(other.gameObject.GetComponent<Rigidbody>());
        }
    }

    private void Update()
    {
        if (Time.time > despawnTimer)
        {
            CheckOnSkiers();
            despawnTimer = Time.time + despawnCooldown;
        }
    }

    void CheckOnSkiers()
    {
        List<Rigidbody> toRemove = new List<Rigidbody>();
        foreach(Rigidbody rb in skiers)
        {
            if(rb.velocity.magnitude <= 0.1)
            {
                toRemove.Add(rb);
            }
        }

        foreach(Rigidbody rb in toRemove)
        {
            skiers.Remove(rb);
            Destroy(rb.gameObject);
        }
    }
}

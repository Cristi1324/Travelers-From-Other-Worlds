using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    public Rigidbody rbPlanet;
    
    void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.velocity = rbPlanet.velocity;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

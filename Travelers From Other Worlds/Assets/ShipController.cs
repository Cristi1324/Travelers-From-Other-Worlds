using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody rb;
    private float gravityConstant = 6.67408f;
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate() {
        CelestialBody[] allBodies = FindObjectsOfType<CelestialBody>();
        foreach(var body in allBodies)
        {
            if(body!=this&&body.gameObject.scene==this.gameObject.scene)
            {
                float sqrDst = (body.GetComponent<Rigidbody>().position - rb.position).sqrMagnitude;
                Vector3 forceDir = -(rb.position - body.GetComponent<Rigidbody>().position).normalized;
                Vector3 acceleration = forceDir * gravityConstant * body.mass * rb.mass / sqrDst;
                rb.AddForce (acceleration);
            }
        }
    }
}

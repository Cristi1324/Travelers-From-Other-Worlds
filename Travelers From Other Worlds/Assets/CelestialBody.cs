using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent (typeof (Rigidbody))]
public class CelestialBody : MonoBehaviour {
    private float gravityConstant = 6674300f;
    public Vector3 initialVelocity;
    Transform meshHolder;
    public float mass;
    Rigidbody rb;
    public bool isClone = false;
    void Start () {
        rb = GetComponent<Rigidbody> ();
        rb.mass = mass;
        rb.velocity = initialVelocity;
    }

    private void FixedUpdate() {
        if(!isClone)ApplyGravity();
    }
    public void ApplyGravity()
    {
        CelestialBody[] allBodies = FindObjectsOfType<CelestialBody>();
        foreach(var body in allBodies)
        {
            if(body!=this&&body.gameObject.scene==this.gameObject.scene)
            {
                float sqrDst = (body.rb.position - rb.position).sqrMagnitude;
                Vector3 forceDir = (rb.position - body.rb.position).normalized;
                Vector3 acceleration = forceDir * gravityConstant * body.mass / sqrDst;
                body.rb.AddForce(acceleration);
            }
        }
        
    }
    public Rigidbody GetRigidbody()
    {
        return this.rb;
    }
}
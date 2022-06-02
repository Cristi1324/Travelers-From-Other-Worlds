using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    int damage = 10;
    public ParticleSystem bulletExplosion;

    void Start()
    {
        bulletExplosion = GetComponentInChildren<ParticleSystem>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.GetComponent<Health>() != null)
        {
            collision.gameObject.GetComponent<Health>().TakeDamage(damage);
        }
        Debug.Log(collision.gameObject);
        Destroy(this.gameObject);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}

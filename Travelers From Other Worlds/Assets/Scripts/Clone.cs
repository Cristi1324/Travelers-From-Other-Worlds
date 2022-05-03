using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clone : MonoBehaviour
{
    // Start is called before the first frame update
    public int id;
    public GameObject parent;
    void Start()
    {
        this.gameObject.GetComponent<Rigidbody>().detectCollisions=false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

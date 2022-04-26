using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCamera : MonoBehaviour
{
    // Start is called before the first frame update
    Transform parent;
    public Vector3 Offset;
    public Vector3 mouseOffset;
    private Vector3 dragOrigin;
    public float speed = 20;
    void Start()
    {
        parent = GetComponentInParent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        float moveHorizontal = Input.GetAxis ("Mouse X");
        float moveVertical = Input.GetAxis ("Mouse Y");
        float zoom = -Input.GetAxis("Mouse ScrollWheel")*transform.position.y;
        //mouseOffset += new Vector3(moveHorizontal,0f,moveVertical) * speed;
        transform.position = parent.position + Offset + mouseOffset + new Vector3(0,zoom,0);  
    }
}

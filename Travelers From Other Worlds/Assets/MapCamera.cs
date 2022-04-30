using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCamera : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject parent;
    public Vector3 Offset;
    public Vector3 mouseOffset;
    private Vector3 dragOrigin;
    public float speed = 20;
    public float zoom;
    void Start()
    {
        parent = GameObject.Find("Ship");
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.M))
        {
            parent.GetComponent<ShipController>().hasPlayer=true;
            parent.GetComponent<ShipController>().cam.enabled=true;
            GetComponent<Camera>().enabled=false;
            FindObjectOfType<Projection>().enabled=false;
            Time.timeScale=1;
            enabled=false;
        }
        float moveHorizontal = Input.GetAxis ("Mouse X");
        float moveVertical = Input.GetAxis ("Mouse Y");
        zoom += -Input.GetAxis("Mouse ScrollWheel")*transform.position.y;
        //mouseOffset += new Vector3(moveHorizontal,0f,moveVertical) * speed;
        transform.position = parent.transform.position + Offset + mouseOffset + new Vector3(0,zoom,0);
    }
}

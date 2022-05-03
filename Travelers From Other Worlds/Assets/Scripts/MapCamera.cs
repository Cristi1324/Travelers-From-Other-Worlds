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
    ShipUI shipUI;
    Vector3 mouseWorldPosition;
    Vector3 mouseDelta;
    Vector2 mouseScreenPosition;
    Vector2 mouseScreenPositionOrigin;
    Vector3 cameraPositionOrigin;
    public Vector2 mouseScreenPositionDelta;

    void Start()
    {
        parent = GameObject.Find("Ship");
    }
    private void Awake()
    {
        shipUI = FindObjectOfType<ShipUI>();
        this.parent.transform.position = parent.transform.position;
    }
    
    // Update is called once per frame
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.M))
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            shipUI.showUI = true;
            parent.GetComponent<ShipController>().playerControled=true;
            parent.GetComponent<ShipController>().cam.enabled=true;
            GetComponent<Camera>().enabled=false;
            FindObjectOfType<Projection>().enabled=false;
            Time.timeScale=1;
            enabled=false;
        }
        /*if(Input.GetMouseButton(0))
        {
            int layerMask = 1 << 8;
            Ray ray = this.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
            {
                mouseWorldPosition = hit.point;
                Debug.Log(mouseWorldPosition);
            }
        }*/
        if (Input.GetMouseButton(0)|| Input.GetMouseButton(1))
        {
            mouseScreenPosition = this.GetComponent<Camera>().ScreenToViewportPoint(Input.mousePosition);
            if (Input.GetMouseButtonDown(0)|| Input.GetMouseButton(1))
            {
                mouseScreenPositionOrigin = mouseScreenPosition;
                cameraPositionOrigin = parent.transform.position;
            }
            if (Input.GetMouseButton(0))
            {
                mouseScreenPositionDelta = (mouseScreenPositionOrigin - mouseScreenPosition) * transform.position.y;
                parent.transform.position = cameraPositionOrigin + new Vector3(mouseScreenPositionDelta.x, 0, mouseScreenPositionDelta.y);
            }
            else
            {

            }
        }
        float moveHorizontal = Input.GetAxis ("Mouse X");
        float moveVertical = Input.GetAxis ("Mouse Y");
        zoom = -Input.GetAxis("Mouse ScrollWheel")*transform.position.y;
        //mouseOffset += new Vector3(moveHorizontal,0f,moveVertical) * speed;
        transform.position += new Vector3(0, zoom, 0);
    }
}

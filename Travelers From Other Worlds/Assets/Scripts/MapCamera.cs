using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class MapCamera : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject ship;
    public Transform parentTransform;
    public Transform cameraTransform;
    public Vector3 Offset;
    public float rotateSensitivity;
    public float translationSensitivity;
    public float zoomSensitivity;
    float zoom;
    float pitch;
    float yaw;
    ShipUI shipUI;
    Vector3 mouseWorldPosition;
    Vector3 mouseDelta;
    Vector2 mouseScreenPosition;
    Vector2 mouseScreenPositionOrigin;
    Vector3 cameraPositionOrigin;
    Vector3 cameraRotationOrigin;
    public Vector2 mouseScreenPositionDelta;
    public Canvas mapUI;
    void Start()
    {
        ship = GameObject.Find("Ship");
    }
    private void Awake()
    {
        shipUI = FindObjectOfType<ShipUI>();
        cameraTransform.position =  parentTransform.up.y*Offset;
    }
    public void CenterCamera()
    {
        ship = GameObject.Find("Ship");
        parentTransform.position = ship.transform.position;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            CenterCamera();
        }
        if(Input.GetKeyDown(KeyCode.M))
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            FindObjectOfType<EscMenu>().setOpenMenu(true);
            shipUI.showUI = true;
            ship.GetComponent<ShipController>().playerControled=true;
            ship.GetComponent<ShipController>().cam.enabled=true;
            FindObjectOfType<CameraController>().enabled=true;
            GetComponent<Camera>().enabled=false;
            FindObjectOfType<Projection>().enabled=false;
            Time.timeScale=1;
            enabled=false;
        }
        if(Input.GetKeyDown(KeyCode.T))
        {
            int layerMask = 1 << 8;
            Ray ray = this.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
            {
                ship.GetComponent<ShipController>().setTarget(hit.collider.transform);
                FindObjectOfType<Projection>().counter=0;
            }
        }
        if(Input.GetKeyDown(KeyCode.F))
        {
            int layerMask = 1 << 8;
            Ray ray = this.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
            {
                parentTransform.position = hit.transform.position;
            }
        }
        if (Input.GetMouseButton(2)|| Input.GetMouseButton(0))
        {
            mouseScreenPosition = this.GetComponent<Camera>().ScreenToViewportPoint(Input.mousePosition);
            if (Input.GetMouseButtonDown(2)|| Input.GetMouseButtonDown(0))
            {
                cameraRotationOrigin = parentTransform.eulerAngles;
                mouseScreenPositionOrigin = mouseScreenPosition;
                cameraPositionOrigin = parentTransform.position;
            }else
            if (Input.GetMouseButton(2))
            {
                mouseScreenPositionDelta = (mouseScreenPositionOrigin - mouseScreenPosition) ;
                parentTransform.position = cameraPositionOrigin + parentTransform.TransformDirection(new Vector3(mouseScreenPositionDelta.x, 0, mouseScreenPositionDelta.y) * cameraTransform.localPosition.y * translationSensitivity);
            }else
            if(Input.GetMouseButton(0))
            {
                mouseScreenPositionDelta = (mouseScreenPositionOrigin - mouseScreenPosition) * rotateSensitivity;
                yaw += -mouseScreenPositionDelta.x;
                pitch += mouseScreenPositionDelta.y;
                pitch = Mathf.Clamp(pitch,-170,10);
                mouseScreenPositionOrigin = mouseScreenPosition;
                parentTransform.rotation = Quaternion.Euler(pitch,yaw,0);
            }
        }
        float moveHorizontal = Input.GetAxis ("Mouse X");
        float moveVertical = Input.GetAxis ("Mouse Y");
        zoom = -Input.GetAxis("Mouse ScrollWheel")* cameraTransform.localPosition.y;
        if(Mathf.Abs(zoom)>0.1f)
        {
            cameraTransform.localPosition += new Vector3(0, zoomSensitivity * zoom, 0);
        }
    }
}

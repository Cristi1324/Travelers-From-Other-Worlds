using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPointer : MonoBehaviour
{
    public Transform Origin;

    //public GameObject Parent;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            CastRay();
        }
    }
    void CastRay()
    {
        int layerMask = 1 << 8;
        layerMask = ~layerMask;
        RaycastHit hit;
        if (Physics.Raycast(Origin.position, Origin.TransformDirection(Vector3.forward), out hit, 1000f, layerMask))
        {
            Debug.DrawRay(Origin.transform.position, Origin.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            EnterShip enterShip = hit.collider.GetComponent<EnterShip>();
            if(enterShip!=null)
            {
                Debug.Log("Hit Ship");
                BoardShip(enterShip.gameObject);
            }
        }
        else
        {
            Debug.DrawRay(Origin.transform.position, Origin.TransformDirection(Vector3.forward) * 1000, Color.white);
            Debug.Log("Did not Hit");
        }
    }
    void BoardShip(GameObject ship)
    {
        ship.GetComponent<ShipController>().hasPlayer = true;
        GetComponentInChildren<Camera>().enabled=false;
        GameObject.Find("ShipCamera").GetComponent<Camera>().enabled=true;
        GetComponentInChildren<MeshRenderer>().enabled=false;
        GetComponent<PlayerController>().enabled=false;
        GetComponentInChildren<Rigidbody>().detectCollisions=false;
        GetComponentInChildren<Rigidbody>().isKinematic=true;
        transform.position = ship.transform.position;
        //transform.parent=ship.transform;
        enabled=false;
    }
}

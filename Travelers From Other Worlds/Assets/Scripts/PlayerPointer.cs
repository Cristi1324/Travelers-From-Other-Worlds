using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPointer : MonoBehaviour
{
    public Transform Origin;
    public Transform playerTransform;

    //public GameObject Parent;
    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
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
            ShipController ship = hit.collider.GetComponent<ShipController>();
            if(ship!=null)
            {
                Debug.Log("Hit Ship");
                ship.BoardPlayer(gameObject);
                BoardShip(ship.gameObject);
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
        GetComponentInChildren<Camera>().enabled=false;
        GetComponentInChildren<MeshRenderer>().enabled=false;
        GetComponent<PlayerController>().enabled=false;
        GetComponentInChildren<Rigidbody>().detectCollisions=false;
        GetComponentInChildren<Rigidbody>().isKinematic=true;
        //transform.parent=ship.transform;
        enabled=false;
    }
}

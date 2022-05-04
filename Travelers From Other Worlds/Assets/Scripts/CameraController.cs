using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform target;
    public float distance;
    public float heightOffset;

    public Vector2 sensitivity;
    public Vector2 smoothing;

    private Vector2 position;
    private Vector2 mousePosition;

    internal Vector3 rawPosition;
    ShipController ship;
    public Vector3 relativeUp;
    public Vector3 relativeRight;
    public Vector3 rotation;
    public Vector3 rot;
    Projection projection;

    private void Start()
    {
        if (sensitivity == Vector2.zero) sensitivity = new Vector2(1.5f, 1f);
        if (smoothing == Vector2.zero) smoothing = Vector2.one * 3f;
        projection = FindObjectOfType<Projection>();
        ship = FindObjectOfType<ShipController>();
    }

    internal void updatePosition(float x, float y) 
    {
        var c = transform;
         c.Rotate(0, x* sensitivity.x, 0);
         c.Rotate(y* sensitivity.y, 0, 0);
         c.Rotate(0, 0, -Input.GetAxis("QandE")*90 * Time.deltaTime);

        relativeUp = transform.TransformDirection(Vector3.up);
        relativeRight = transform.TransformDirection(Vector3.right);
        Vector2 update = Vector2.zero;
        update.x = y * (sensitivity.x * smoothing.x);
        update.y = x * (sensitivity.y * smoothing.y);

        mousePosition.x = Mathf.Lerp(mousePosition.x, update.x, 1f / smoothing.x);
        mousePosition.y = Mathf.Lerp(mousePosition.y, update.y, 1f / smoothing.y);
        Vector3 rotation = relativeUp*mousePosition.y+relativeRight*mousePosition.x;
        position.x += mousePosition.x;
        position.y += mousePosition.y;

        //Clamp the Y value for vertical rotation
        //position.x = Mathf.Clamp(position.x, -85f, 85f);
    }
    void Update()
    {
        if(Input.GetMouseButtonDown(2))
        {
            CastRay();
        }
    }
    private void CastRay() {
        int layerMask = 1<<8;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))
        {
            Debug.DrawRay(transform.transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            if(hit.collider.GetComponent<CelestialBody>()){
                ship.setTarget(hit.collider.transform);
            }
        }
        else
        {
            projection.hasReferenceBody = false;
            ship.GetComponent<ShipController>().hasTarget = false;
        }
    }
    void LateUpdate()
    {
        var newRotation = transform.rotation;
        rot=transform.up;
        var newPosition = newRotation * new Vector3(0, heightOffset, -distance) + target.position;
        var newDirection = target.position - newPosition;

        //transform.rotation = Quaternion.LookRotation(newDirection, ship.up);
        transform.position = newPosition + new Vector3(heightOffset*transform.up.x, heightOffset*transform.up.y, 0);

        rawPosition = newPosition;
    }
}

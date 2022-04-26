using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterWorld : MonoBehaviour
{
    public float distanceThreshold = 1000;
    private float distanceFromOrigin;
    private List<Transform> physicsObjects;
    public GameObject player;
    public GameObject ship;
    private Vector3 originOffset;
    Camera playerCamera;
    public CelestialBody[] bodies;
    public event System.Action PostFloatingOriginUpdate;

    void Awake () {
        bodies = FindObjectsOfType<CelestialBody> ();
        ship=FindObjectOfType<ShipController>().gameObject;
        playerCamera = Camera.main;
        physicsObjects = new List<Transform> ();
        physicsObjects.Add (player.transform);
        physicsObjects.Add(ship.transform);
        foreach (var c in bodies) {
            physicsObjects.Add (c.transform);
        }
    }

    void FixedUpdate () {
        UpdateFloatingOrigin ();
        if (PostFloatingOriginUpdate != null) {
            PostFloatingOriginUpdate ();
        }
    }

    void UpdateFloatingOrigin () {
        originOffset = player.transform.position;
        float distanceFromOrigin = originOffset.magnitude;

        if (distanceFromOrigin > distanceThreshold) {
            foreach (Transform t in physicsObjects) {
                t.position -= originOffset;
            }
        }
        
    }
}

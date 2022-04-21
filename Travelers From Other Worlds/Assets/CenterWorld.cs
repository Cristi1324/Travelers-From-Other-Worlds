using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterWorld : MonoBehaviour
{
    public float distanceThreshold = 1000;
    List<Transform> physicsObjects;
    public GameObject player;

    public event System.Action PostFloatingOriginUpdate;

    void Awake () {

        var bodies = FindObjectsOfType<CelestialBody> ();
        physicsObjects = new List<Transform> ();
        physicsObjects.Add (player.transform);
        foreach (var c in bodies) {
            physicsObjects.Add (c.transform);
        }
    }

    void LateUpdate () {
        UpdateFloatingOrigin ();
        if (PostFloatingOriginUpdate != null) {
            PostFloatingOriginUpdate ();
        }
    }

    void UpdateFloatingOrigin () {
        Vector3 originOffset = player.transform.position;
        float dstFromOrigin = originOffset.magnitude;

        if (dstFromOrigin > distanceThreshold) {
            foreach (Transform t in physicsObjects) {
                t.position -= originOffset;
            }
        }
    }
}
